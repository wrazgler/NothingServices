# Base container
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS sdk
WORKDIR "/src"

# Get arguments
ARG CONFIGURATION="Release"

# Echo
RUN echo $CONFIGURATION

# Nuget container
FROM sdk AS nuget
WORKDIR "/src"

# Copy project
COPY "./NothingServices.Abstractions/" "."

# Restore Nuget
RUN dotnet restore "NothingServices.Abstractions.csproj"

# Build Nuget
RUN dotnet build "NothingServices.Abstractions.csproj" \
    --configuration $CONFIGURATION \
    --no-restore

# Pack Nuget
RUN dotnet pack "NothingServices.Abstractions.csproj" \
    --configuration $CONFIGURATION \
    --output /app/publish/ \
    --no-build \
    --no-restore

# Project container
FROM sdk AS build
WORKDIR "/src"

# Copy project
COPY "./NothingKafka/" "."
COPY --from=nuget "/app/publish/*.nupkg" "/root/.nuget/NuGet/"

# Add Nuget
RUN dotnet nuget add source "." --name "NothingServices.Abstractions"

# Restore project
RUN dotnet restore "NothingKafka.csproj"

# Publish project
RUN dotnet publish "NothingKafka.csproj" \
    --configuration $CONFIGURATION \
    --framework net8.0 \
    --output /app/publish/ \
    --no-restore

# Application container
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR "/app"

# Copy build application
COPY --from=build "/app/publish" "."

# Run application
ENTRYPOINT ["dotnet", "NothingKafka.dll"]