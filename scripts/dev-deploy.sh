# get to project root
cd $(git rev-parse --show-toplevel)/src/Hng.Web
# install dependencies
dotnet restore ./Hng.Web.csproj
# build app
dotnet build
# ensure app is executable
chmod +x ./bin/Release/net8.0/Hng.Web
# kill any existing instance
echo "$PASSWORD" | sudo kill -9 $(sudo lsof -t -i:5288) &> /dev/null || true
# run app - dev
nohup dotnet run &