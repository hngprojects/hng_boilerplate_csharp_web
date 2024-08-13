# Stage 1: Base Image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5000

ENV ASPNETCORE_URLS=http://+:5000

# Stage 2: Build Image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Copy everything and restore solution
COPY . .
RUN dotnet restore Hng.Csharp.Web.sln

# Build
WORKDIR /src/Hng.Web
RUN dotnet build Hng.Web.csproj -c Release -o /app/build

# Stage 3: Publish Image
FROM build AS publish
RUN dotnet publish Hng.Web.csproj -c Release -o /app/publish /p:UseAppHost=false

# Stage 4: Final Image
FROM base AS final
WORKDIR /app

# Copy the published output from the publish stage
COPY --from=publish /app/publish .

# Command to run the application
ENTRYPOINT ["dotnet", "Hng.Web.dll"]
