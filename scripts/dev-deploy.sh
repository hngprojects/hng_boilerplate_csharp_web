# navigate to repo root
cd  $(git rev-parse --show-toplevel)

# install dependencies
dotnet restore Hng.Csharp.Web.sln

# build app
dotnet build

# navigate to project root
cd src/Hng.Web

# kill any existing instance on port 80
sudo kill -9 $(sudo lsof -t -i:80) &> /dev/null || true

# run app on port 80
nohup sudo dotnet run --urls "http://0.0.0.0:80" > nohup.out 2> nohup.err < /dev/null &

