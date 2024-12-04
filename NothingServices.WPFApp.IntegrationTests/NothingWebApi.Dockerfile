# Base container
FROM mcr.microsoft.com/dotnet/sdk:9.0-windowsservercore-ltsc2022 AS sdk
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
    --output /app/publish/ \
    --no-build \
    --no-restore

# Project container
FROM sdk AS build
WORKDIR "/src"

# Copy project
COPY "./NothingWebApi/" "."
COPY "./.certificates/" "../.certificates/"
COPY --from=nuget "/app/publish/*.nupkg" "/root/.nuget/NuGet/"

# Add Nuget
RUN dotnet nuget add source "." --name "NothingServices.Abstractions"

# Restore project
RUN dotnet restore "NothingWebApi.csproj"

# Publish project
RUN dotnet publish "NothingWebApi.csproj" \
    --configuration $CONFIGURATION \
    --framework net8.0 \
    --output /app/publish/ \
    --no-restore

# Application container
FROM mcr.microsoft.com/dotnet/aspnet:8.0-windowsservercore-ltsc2022
WORKDIR "/app"

# Get arguments
ARG CERTIFICATE_PASSWORD="localhost"

# Set environment
ENV ASPNETCORE_HTTP_PORTS=8100
ENV ASPNETCORE_HTTPS_PORTS=8200
ENV ASPNETCORE_Kestrel__Certificates__Default__Path="/app/localhost.crt"
ENV ASPNETCORE_Kestrel__Certificates__Default__Password=$CERTIFICATE_PASSWORD

# Set ports
EXPOSE 8100
EXPOSE 8200

# Copy build application
COPY --from=build "/app/publish" "."

# Run application
ENTRYPOINT ["dotnet", "NothingWebApi.dll"]