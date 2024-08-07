# Stage 1: Base Image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5000

ENV ASPNETCORE_URLS=http://+:5000
ENV ASPNETCORE_ENVIRONMENT=Development

# Stage 2: Build Image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release

# Copy project file and restore as distinct layers
# COPY Hng.Application/Hng.Application.csproj Hng.Application/
# COPY Hng.Domain/Hng.Domain.csproj Hng.Domain/
# COPY Hng.Infrastructure/Hng.Infrastructure.csproj Hng.Infrastructure/
# COPY Hng.Web/Hng.Web.csproj Hng.Web/
COPY . .
RUN dotnet restore Hng.Csharp.Web.sln

# Copy everything else and build
WORKDIR /src/Hng.Web
RUN dotnet build Hng.Web.csproj -c $BUILD_CONFIGURATION -o /app/build

# Stage 3: Publish Image
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish Hng.Web.csproj -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Stage 4: Final Image
FROM base AS final
WORKDIR /app

# Copy the published output from the publish stage
COPY --from=publish /app/publish .

# Set the user to non-root only in the final stage
USER app

# Command to run the application
ENTRYPOINT ["dotnet", "Hng.Web.dll"]