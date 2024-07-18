#!/bin/bash

export DOMAIN_NAME="api-csharp.boilerplate.hng.tech"
export DEV_DOMAIN_NAME="staging.api-csharp.boilerplate.hng.tech"
export STAGING_DOMAIN_NAME="deployment.api-csharp.boilerplate.hng.tech"

export DOMAIN_IP="91.229.239.238"

echo "DOMAIN_NAME=${DOMAIN_NAME}" >> ~/.bashrc
echo "DEV_DOMAIN_NAME=${DEV_DOMAIN_NAME}" >> ~/.bashrc
echo "STAGING_DOMAIN_NAME=${STAGING_DOMAIN_NAME}" >> ~/.bashrc
echo "DOMAIN_IP=${DOMAIN_IP}" >> ~/.bashrc

source ~/.bashrc

# Verify the environment variables
echo "DOMAIN_NAME: $DOMAIN_NAME"
echo "DEV_DOMAIN_NAME: $DEV_DOMAIN_NAME"
echo "STAGING_DOMAIN_NAME: $STAGING_DOMAIN_NAME"
echo "DOMAIN_IP: $DOMAIN_IP"

sudo apt-get update
# get logged-in user
USER=$(logname)

# get from env
DOMAIN_NAME=$DOMAIN_NAME
DEV_DOMAIN_NAME=$DEV_DOMAIN_NAME
STAGING_DOMAIN_NAME=$STAGING_DOMAIN_NAME

# Credentials
CREDENTIALS_FILE="/etc/server_creds/credentials.txt"

if [ ! -f "$CREDENTIALS_FILE" ]; then
    sudo mkdir -p /etc/server_creds
    sudo touch "$CREDENTIALS_FILE"
    sudo chmod 600 "$CREDENTIALS_FILE"
fi

generate_password() {
    tr -dc A-Za-z0-9 < /dev/urandom | head -c 12
}

store_password() {
    local user=$1
    local password=$2
    if ! grep -q "^$user:" "$CREDENTIALS_FILE"; then
        echo "$user: $password" | sudo tee -a "$CREDENTIALS_FILE" > /dev/null
    else
        password=$(grep "^$user:" "$CREDENTIALS_FILE" | cut -d ' ' -f 2)
    fi
    echo "$password"
}

# Redis
if ! command -v redis-server &> /dev/null
then
    echo "Redis not found. Installing Redis..."
    sudo apt-get install redis-server -y
else
    echo "Redis is already installed."
fi

if sudo systemctl is-active --quiet redis-server
then
    echo "Redis service is already running."
else
    echo "Starting Redis service..."
    sudo systemctl enable redis-server
    sudo systemctl start redis-server
fi

if redis-cli ping | grep -q "PONG"
then
    echo "Redis installation and setup verified. Redis is running correctly."
else
    echo "Redis setup verification failed. Please check the Redis installation."
    exit 1
fi

# Create Redis users with passwords (using ACL) if not already present
if ! redis-cli ACL LIST | grep -q "user:redis_dev"; then
    DEV_PASSWORD=$(store_password "redis_dev" $(generate_password))
    redis-cli ACL SETUSER redis_dev on >"$DEV_PASSWORD" ~* &> /dev/null
fi

if ! redis-cli ACL LIST | grep -q "user:redis_staging"; then
    STAGING_PASSWORD=$(store_password "redis_staging" $(generate_password))
    redis-cli ACL SETUSER redis_staging on >"$STAGING_PASSWORD" ~* &> /dev/null
fi

if ! redis-cli ACL LIST | grep -q "user:redis_prod"; then
    PROD_PASSWORD=$(store_password "redis_prod" $(generate_password))
    redis-cli ACL SETUSER redis_prod on >"$PROD_PASSWORD" ~* &> /dev/null
fi

echo "Redis users created and configured."

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

if [ ! -f /etc/nginx/sites-available/api.conf ]
then
    # Create NGINX configuration file for API
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

if [ -s "$CREDENTIALS_FILE" ] && grep -q "dbadmin:" "$CREDENTIALS_FILE"; then
    echo "Password file already exists and is not empty. Skipping password generation."
    # Read passwords from the file
    dbadmin_PASSWORD=$(grep 'dbadmin:' $CREDENTIALS_FILE | cut -d ' ' -f 2)
    DEV_USER_PASSWORD=$(grep 'dev_user:' $CREDENTIALS_FILE | cut -d ' ' -f 2)
    PROD_USER_PASSWORD=$(grep 'prod_user:' $CREDENTIALS_FILE | cut -d ' ' -f 2)
    STAGING_USER_PASSWORD=$(grep 'staging_user:' $CREDENTIALS_FILE | cut -d ' ' -f 2)
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

    echo "Passwords generated and stored in $CREDENTIALS_FILE."
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
