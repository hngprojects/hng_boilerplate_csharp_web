#!/bin/bash

sudo apt-get update
# get logged-in user
USER=$(logname)

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
fi

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

if nginx -v &> /dev/null
then
    echo "NGINX installation and setup verified."
else
    echo "NGINX setup verification failed. Please check the NGINX installation."
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

USER_EXISTS=$(sudo -i -u postgres psql -tAc "SELECT 1 FROM pg_roles WHERE rolname='devopsintern'")
if [ "$USER_EXISTS" != "1" ]; then
    sudo -i -u postgres psql <<EOF
    -- Create a user
    CREATE USER devopsintern WITH PASSWORD 'devops#HNG11';

    -- Create databases
    CREATE DATABASE dev_db;
    CREATE DATABASE prod_db;
    CREATE DATABASE staging_db;

    -- Grant privileges
    GRANT ALL PRIVILEGES ON DATABASE dev_db TO devopsintern;
    GRANT ALL PRIVILEGES ON DATABASE prod_db TO devopsintern;
    GRANT ALL PRIVILEGES ON DATABASE staging_db TO devopsintern;
EOF
    echo "PostgreSQL user and databases created."
else
    echo "PostgreSQL user devopsintern already exists."
fi

sudo -i -u postgres psql -c "\du"
sudo -i -u postgres psql -c "\l"

# Project Setup
cd ~

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

cd ~

# Systemd
create_systemd_service() {
    local service_name=$1
    local exec_start=$2
    local environment=$3
    local service_file_path="/etc/systemd/system/${service_name}.service"
    
    echo "[Unit]
Description=Hng.Web .NET Application
After=network.target

[Service]
ExecStart=${exec_start}
Restart=always
RestartSec=10
KillSignal=SIGINT
SyslogIdentifier=hng-web
Environment=ASPNETCORE_ENVIRONMENT=${environment}
Restart=on-failure
RestartSec=5

[Install]
WantedBy=multi-user.target" | sudo tee ${service_file_path} > /dev/null

    echo "Created ${service_file_path}"
}

if systemctl list-units --full -all | grep -Fq "hng-web.service"; then
    echo "Service hng-web.service already exists."
else
    create_systemd_service "hng-web" "dotnet /home/${USER}/hng_boilerplate_csharp_web/src/Hng.Web/bin/Release/net8.0/Hng.Web.dll" "Production"
fi

if systemctl list-units --full -all | grep -Fq "hng-web-dev.service"; then
    echo "Service hng-web-dev.service already exists."
else
    create_systemd_service "hng-web-dev" "dotnet /home/${USER}/hng_boilerplate_csharp_web/src/Hng.Web/bin/Release/net8.0/Hng.Web.dll --urls=http://localhost:5001" "Development"
fi

sudo systemctl daemon-reload

sudo systemctl enable hng-web
sudo systemctl start hng-web

sudo systemctl enable hng-web-dev
sudo systemctl start hng-web-dev

if systemctl is-active --quiet hng-web
then
    echo "HNG-Web service is running."
else
    echo "HNG-Web service is not running. Please check the HNG-Web installation."
fi

if systemctl is-active --quiet hng-web-dev
then
    echo "HNG-Web-dev service is running."
else
    echo "HNG-Web-dev service is not running. Please check the HNG-Web-dev installation."
fi

echo "HNG C# Web Server installation and setup verified. Ending script."
