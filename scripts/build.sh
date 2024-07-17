# get to project root
cd $(git rev-parse --show-toplevel)/src/Hng.Web
# install dependencies
dotnet restore ./Hng.Web.csproj
# build app
dotnet build