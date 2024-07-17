cd ../src
dotnet restore Hng.Web/Hng.Web.csproj
cd Hng.Web
dotnet build
dotnet publish
chmod +x ./bin/Release/net8.0/Hng.Web
./bin/Release/net8.0/Hng.Web
