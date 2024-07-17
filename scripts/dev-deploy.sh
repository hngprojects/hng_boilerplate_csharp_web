# navigate to repo root
cd  $(git rev-parse --show-toplevel)

# install dependencies
dotnet restore Hng.Csharp.Web.sln

# build app
dotnet build

# publish app
dotnet publish

# restart the systemd service
sudo systemctl restart hng-web-dev
