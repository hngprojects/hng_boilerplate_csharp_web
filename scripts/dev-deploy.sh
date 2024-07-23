#!/bin/bash
set -e

# navigate to repo root and fetch latest changes
cd "$(git rev-parse --show-toplevel)"

git checkout dev
git pull origin dev

# install dependencies
dotnet restore ./Hng.Csharp.Web.sln

# build app
dotnet build --no-restore -c Debug

# publish app
dotnet publish ./src/Hng.Web/Hng.Web.csproj --no-build -c Debug

# restart the systemd service
sudo systemctl restart hng-web-dev
