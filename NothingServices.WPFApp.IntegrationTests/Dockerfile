# Base container
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS sdk
WORKDIR "/src"

# Get arguments
ARG CERTIFICATE_PASSWORD="localhost"
ARG CONFIGURATION="Release"

# Echo
RUN echo $CERTIFICATE_PASSWORD
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
    --output /app/test/ \
    --no-build \
    --no-restore

# Test container
FROM mcr.microsoft.com/dotnet/sdk:8.0-windowsservercore-ltsc2022 AS test
WORKDIR "/src"

# Copy project
COPY "./NothingServices.WPFApp/" "./NothingServices.WPFApp/"
COPY "./NothingServices.WPFApp.IntegrationTests/" "./NothingServices.WPFApp.IntegrationTests/"
COPY "./.certificates/" "./.certificates/"
COPY --from=nuget "/app/test/*.nupkg" "/root/.nuget/NuGet/"

# Add Nuget
RUN dotnet nuget add source "." --name "NothingServices.Abstractions"

# Restore project
RUN dotnet restore "NothingServices.WPFApp/NothingServices.WPFApp.csproj"

# Build project
RUN dotnet build "NothingServices.WPFApp/NothingServices.WPFApp.csproj" \
    --configuration $CONFIGURATION \
    --no-restore

# Restore tests
RUN dotnet restore "NothingServices.WPFApp.IntegrationTests/NothingServices.WPFApp.IntegrationTests.csproj"

# Build tests
RUN dotnet build "NothingServices.WPFApp.IntegrationTests/NothingServices.WPFApp.IntegrationTests.csproj" \
    --configuration $CONFIGURATION \
    --no-restore

# Test tests
RUN dotnet test "NothingServices.WPFApp.IntegrationTests/NothingServices.WPFApp.IntegrationTests.csproj" \
    --configuration $CONFIGURATION \
    --no-build \
    --no-restore