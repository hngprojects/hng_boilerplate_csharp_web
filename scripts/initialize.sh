#!/bin/bash

# Function to show usage
usage() {
  echo "Usage: $0 <APP_NAME> <DOMAIN> [--force]"
  exit 1
}

# Function to find the next available port
find_next_available_port() {
    local port=$1
    while true; do
        if ! ss -tuln | grep -q ":${port}"; then
            echo $port
            return
        fi
        port=$((port + 1))
    done
}

# Function to create and start a Redis instance
create_redis_instance() {
    local instance_name=$1
    local config_file=$2
    local port=$3

    echo "Creating Redis instance: ${instance_name} on port ${port}"

    # Create Redis configuration file
    echo "port ${port}" > ${config_file}
    echo "dir /var/lib/redis/${instance_name}" >> ${config_file}
    mkdir -p /var/lib/redis/${instance_name}
    chown redis:redis /var/lib/redis/${instance_name}

    # Start Redis instance with the new configuration
    redis-server ${config_file} &
    echo "Redis instance ${instance_name} started on port ${port}"
}

# Function to generate a password
generate_password() {
    local user=$1
    local extra_info=$2
    local password=$(tr -dc A-Za-z0-9 < /dev/urandom | head -c 12)

    if [ -z "$user" ]; then
        echo "Error: Failed to pass user to generate password."
        usage
    fi

    # Save the password and extra info if provided
    if ! grep -q "^$user:" "$APP_CONFIG_FILE"; then
        echo "$user: $password $extra_info" | sudo tee -a "$APP_CONFIG_FILE" > /dev/null
    else
        password=$(grep "^$user:" "$APP_CONFIG_FILE" | cut -d ' ' -f 2)
    fi
    echo "$password"
}

# Ensure script is run with sudo
if [ "$EUID" -ne 0 ]; then
    echo "Please run as root."
    exit 1
fi

# Initialize variables
FORCE=false

# Parse arguments
while [[ "$#" -gt 0 ]]; do
  case "$1" in
    --force) FORCE=true; shift ;;
    *) 
      if [ -z "$APP_NAME" ]; then
        APP_NAME="$1"
      elif [ -z "$DOMAIN" ]; then
        DOMAIN="$1"
      else
        usage
      fi
      shift ;;
  esac
done

# Check if APP_NAME is provided
if [ -z "$APP_NAME" ]; then
  echo "Error: Application name is missing."
  usage
fi

# Check if DOMAIN is provided
if [ -z "$DOMAIN" ]; then
  echo "Error: Domain is missing."
  usage
fi

# Server Configuration
SERVER_CONF_DIR="/etc/server.d"
APP_CONFIG_FILE="${SERVER_CONF_DIR}/${APP_NAME}.conf"

# Check if configuration file already exists
if [ -f "$APP_CONFIG_FILE" ]; then
  if [ "$FORCE" = false ]; then
    echo "Error: Configuration for '$APP_NAME' already exists. Use --force to override."
    exit 1
  else
    echo "Warning: Overriding existing configuration for '$APP_NAME'."
  fi
fi

# Create and write configuration
USER=$(logname)
DEV_DOMAIN="deployment.${DOMAIN}"
STAGING_DOMAIN="staging.${DOMAIN}"

# Print the user running the script
echo "Script is being run by user: $USER"

sudo mkdir -p "$SERVER_CONF_DIR"
sudo chmod 600 "$APP_CONFIG_FILE"
echo '[ Server Configuration ]' | sudo tee $APP_CONFIG_FILE > /dev/null
echo "USER=${USER}" | sudo tee -a $APP_CONFIG_FILE > /dev/null
echo "DOMAIN=${DOMAIN}" | sudo tee -a $APP_CONFIG_FILE > /dev/null
echo "DEV_DOMAIN=${DEV_DOMAIN}" | sudo tee -a $APP_CONFIG_FILE > /dev/null
echo "STAGING_DOMAIN=${STAGING_DOMAIN}" | sudo tee -a $APP_CONFIG_FILE > /dev/null

# Update packages
sudo apt-get update

# Redis setup
if ! command -v redis-server &> /dev/null; then
    echo "Redis not found. Installing Redis..."
    sudo apt-get install redis-server -y
else
    echo "Redis is already installed."
fi

if sudo systemctl is-active --quiet redis-server; then
    echo "Redis service is already running."
else
    echo "Starting Redis service..."
    sudo systemctl enable redis-server
    sudo systemctl start redis-server
fi

if redis-cli ping | grep -q "PONG"; then
    echo "Redis installation and setup verified. Redis is running correctly."
else
    echo "Redis setup verification failed. Please check the Redis installation."
    exit 1
fi

# Initial Redis port
redis_port_start=6379

# Define environment names and their configurations
environments=("dev" "staging" "prod")

# Create Redis instances and users
echo "[ Redis Credentials ]" | sudo tee -a $APP_CONFIG_FILE > /dev/null

for i in "${!environments[@]}"; do
    env="${environments[$i]}"
    instance_name="${APP_NAME}-${env}"
    config_file="/etc/redis/${APP_NAME}-${env}.conf"
    port=$(find_next_available_port $((redis_port_start + i)))

    # Create Redis instance
    create_redis_instance "$instance_name" "$config_file" "$port"

    # Create Redis users with appropriate permissions
    for role in user admin; do
        user_name="${APP_NAME}-${env}-${role}"
        if ! redis-cli -p ${port} ACL LIST | grep -q "user:${user_name}"; then
            password=$(generate_password "${user_name}" "port=${port}")
            if [ "$role" = "admin" ]; then
                # Admin user with full permissions
                redis-cli -p ${port} <<EOF
ACL SETUSER ${user_name} on +@all >$password ~*
EOF
            else
                # Normal user with limited permissions
                redis-cli -p ${port} <<EOF
ACL SETUSER ${user_name} on +@read +@write >$password ~*
EOF
            fi
            echo "Created Redis user: ${user_name} for ${instance_name} with password: $password"
        else
            echo "Redis user ${user_name} already exists for ${instance_name}."
        fi
    done
done

echo "Redis instances and users created and configured."

# NGINX
if ! command -v nginx &> /dev/null
then
    echo "NGINX not found. Installing NGINX..."
    sudo apt-get install nginx -y
else
    echo "NGINX is already installed."
fi

if sudo systemctl is-active --quiet nginx
then
    echo "NGINX service is already running."
else
    echo "Starting NGINX service..."
    sudo systemctl enable nginx
    sudo systemctl start nginx
fi

sudo bash -c 'cat > /etc/nginx/sites-available/api.conf <<EOF
server {
    listen 80;
    server_name api-csharp.boilerplate.hng.tech;

    location / {
        proxy_pass http://localhost:5000;
        proxy_set_header Host \$host;
        proxy_set_header X-Real-IP \$remote_addr;
        proxy_set_header X-Forwarded-For \$proxy_add_x_forwarded_for;
        proxy_set_header X-Forwarded-Proto \$scheme;
        proxy_redirect off;
    }
}

server {
    listen 80;
    server_name staging.api-csharp.boilerplate.hng.tech;

    location / {
        proxy_pass http://localhost:5001;
        proxy_set_header Host \$host;
        proxy_set_header X-Real-IP \$remote_addr;
        proxy_set_header X-Forwarded-For \$proxy_add_x_forwarded_for;
        proxy_set_header X-Forwarded-Proto \$scheme;
        proxy_redirect off;
    }
}

server {
    listen 80;
    server_name deployment.api-csharp.boilerplate.hng.tech;

    location / {
        proxy_pass http://localhost:5002;
        proxy_set_header Host \$host;
        proxy_set_header X-Real-IP \$remote_addr;
        proxy_set_header X-Forwarded-For \$proxy_add_x_forwarded_for;
        proxy_set_header X-Forwarded-Proto \$scheme;
        proxy_redirect off;
    }
}
EOF'

    # Create symbolic link for NGINX configuration
    sudo ln -sf /etc/nginx/sites-available/api.conf /etc/nginx/sites-enabled/

    # Remove default symbolic link if it exists
    if [ -f /etc/nginx/sites-enabled/default ]
    then
        sudo rm /etc/nginx/sites-enabled/default
        echo "Removed default NGINX configuration from sites-enabled."
    fi

    # Verify NGINX configuration syntax
    sudo nginx -t

    # Reload NGINX to apply changes
    sudo systemctl reload nginx

    # Verify NGINX is running with the new configuration
    if sudo systemctl is-active --quiet nginx
    then
        echo "NGINX configuration applied successfully."
    else
        echo "NGINX configuration failed to apply. Please check the configuration."
    fi
else
    echo "NGINX configuration file api.conf already exists. Skipping configuration."
fi

# Configure ssl
sudo apt install -y snap
sudo snap install --classic certbot
sudo certbot --nginx -d api-csharp.boilerplate.hng.tech -d deployment.api-csharp.boilerplate.hng.tech -d staging.api-csharp.boilerplate.hng.tech --agree-tos --no-eff-email --email devops@hng.tech

exit 0

# PostgreSQL
if ! command -v psql &> /dev/null
then
    echo "PostgreSQL not found. Installing PostgreSQL..."
    sudo apt-get install postgresql postgresql-contrib -y
else
    echo "PostgreSQL is already installed."
fi

if sudo systemctl is-active --quiet postgresql
then
    echo "PostgreSQL service is already running."
else
    echo "Starting PostgreSQL service..."
    sudo systemctl enable postgresql
    sudo systemctl start postgresql
fi

if [ -s "$APP_CONFIG_FILE" ] && grep -q "dbadmin:" "$APP_CONFIG_FILE"; then
    echo "Password file already exists and is not empty. Skipping password generation."
    # Read passwords from the file
    dbadmin_PASSWORD=$(grep 'dbadmin:' $APP_CONFIG_FILE | cut -d ' ' -f 2)
    DEV_USER_PASSWORD=$(grep 'dev_user:' $APP_CONFIG_FILE | cut -d ' ' -f 2)
    PROD_USER_PASSWORD=$(grep 'prod_user:' $APP_CONFIG_FILE | cut -d ' ' -f 2)
    STAGING_USER_PASSWORD=$(grep 'staging_user:' $APP_CONFIG_FILE | cut -d ' ' -f 2)
else
    # Generate random passwords
    dbadmin_PASSWORD=$(openssl rand -base64 12)
    DEV_USER_PASSWORD=$(openssl rand -base64 12)
    PROD_USER_PASSWORD=$(openssl rand -base64 12)
    STAGING_USER_PASSWORD=$(openssl rand -base64 12)

    store_password "dbadmin" "$dbadmin_PASSWORD"
    store_password "dev_user" "$DEV_USER_PASSWORD"
    store_password "prod_user" "$PROD_USER_PASSWORD"
    store_password "staging_user" "$STAGING_USER_PASSWORD"

    echo "Passwords generated and stored in $APP_CONFIG_FILE."
fi

USER_EXISTS=$(sudo -i -u postgres psql -tAc "SELECT 1 FROM pg_roles WHERE rolname='dbadmin'")
if [ "$USER_EXISTS" != "1" ]; then
    sudo -i -u postgres psql <<EOF
    -- Create a user with all privileges
    CREATE USER dbadmin WITH PASSWORD '$dbadmin_PASSWORD';

    -- Create 3 users to manage the databases (Dev, Prod, Staging)
    CREATE USER dev_user WITH PASSWORD '$DEV_USER_PASSWORD';
    CREATE USER prod_user WITH PASSWORD '$PROD_USER_PASSWORD';
    CREATE USER staging_user WITH PASSWORD '$STAGING_USER_PASSWORD';

    -- Create databases
    CREATE DATABASE dev_db;
    CREATE DATABASE prod_db;
    CREATE DATABASE staging_db;

    -- Revoke all privileges from these users
    REVOKE ALL PRIVILEGES ON DATABASE dev_db FROM dev_user;
    REVOKE ALL PRIVILEGES ON DATABASE prod_db FROM prod_user;
    REVOKE ALL PRIVILEGES ON DATABASE staging_db FROM staging_user;

    -- Give them read, write, update, and delete access for their respective databases
    GRANT CONNECT ON DATABASE dev_db TO dev_user;
    GRANT CONNECT ON DATABASE prod_db TO prod_user;
    GRANT CONNECT ON DATABASE staging_db TO staging_user;
    GRANT USAGE ON SCHEMA public TO dev_user;
    GRANT USAGE ON SCHEMA public TO prod_user;
    GRANT USAGE ON SCHEMA public TO staging_user;
    GRANT SELECT, INSERT, UPDATE, DELETE ON ALL TABLES IN SCHEMA public TO dev_user;
    GRANT SELECT, INSERT, UPDATE, DELETE ON ALL TABLES IN SCHEMA public TO prod_user;
    GRANT SELECT, INSERT, UPDATE, DELETE ON ALL TABLES IN SCHEMA public TO staging_user;
    ALTER DEFAULT PRIVILEGES IN SCHEMA public GRANT SELECT, INSERT, UPDATE, DELETE ON TABLES TO dev_user;
    ALTER DEFAULT PRIVILEGES IN SCHEMA public GRANT SELECT, INSERT, UPDATE, DELETE ON TABLES TO prod_user;
    ALTER DEFAULT PRIVILEGES IN SCHEMA public GRANT SELECT, INSERT, UPDATE, DELETE ON TABLES TO staging_user;

    -- Grant privileges
    GRANT ALL PRIVILEGES ON DATABASE dev_db TO dbadmin;
    GRANT ALL PRIVILEGES ON DATABASE prod_db TO dbadmin;
    GRANT ALL PRIVILEGES ON DATABASE staging_db TO dbadmin;
EOF
    echo "PostgreSQL user and databases created."
else
    echo "PostgreSQL user dbadmin already exists."
fi

sudo -i -u postgres psql -c "\du"
sudo -i -u postgres psql -c "\l"

# C# setup
if ! command -v dotnet &> /dev/null
then
    echo "dotnet not found. Installing dotnet..."
    sudo apt-get update && \
      sudo apt-get install -y dotnet-sdk-8.0
else
    echo "dotnet is already installed."
fi

if ! command -v dotnet &> /dev/null
then
    echo "dotnet installation failed. Please check the installation."
    exit 1
else
    echo "dotnet installation successful."
fi

# Project Setup
cd /home/$USER

mkdir -p hng_boilerplate_csharp_web
cd hng_boilerplate_csharp_web
if [ ! -d ".git" ]; then
  echo "Initializing a new git repository..."
  git init
fi

ls -al

REPO_URL="https://github.com/hngprojects/hng_boilerplate_csharp_web.git"

if ! git ls-remote --exit-code origin &> /dev/null; then
    echo "Remote 'origin' does not exist. Adding it now..."
    git remote add origin "$REPO_URL"
else
    echo "Remote 'origin' already exists."
    CURRENT_URL=$(git config --get remote.origin.url)
    if [ "$CURRENT_URL" != "$REPO_URL" ]; then
        echo "Updating remote 'origin' URL to $REPO_URL"
        git remote set-url origin "$REPO_URL"
    else
        echo "Remote 'origin' URL is already set correctly."
    fi
fi

git pull origin main

cd /home/$USER

echo "HNG C# Web Server installation completed. Ending script."
