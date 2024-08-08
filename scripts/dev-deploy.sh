#!/bin/bash
set -e

# Load the Docker image
docker load -i /home/${USER}/csharp_dev.tar

# Navigate to the project directory
cd ~/development
git checkout dev
git pull origin dev

docker compose down
# Use Docker Compose to start the containers
docker compose up -d

# Cleanup: Remove the tar file if no longer needed
rm -f /home/${USER}/csharp_dev.tar
