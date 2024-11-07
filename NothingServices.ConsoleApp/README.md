# NothingServices.ConsoleApp
## Начало работы
### 1. [Установить Docker](https://www.docker.com)
### 2. Открыть каталог `NothingServices` в терминале
#### Например 
```shell
cd Documents/GitHub/NothingServices
```
### 3. [Создать контейнер с сервисами NothingRpcApi и NothingWebApi](https://docker-docs.uclv.cu/compose/reference/up/)
```shell
docker compose up -d
```
### 4. Добавить Nuget пакет NothingServices.Abstractions
```shell
dotnet restore NothingServices.Abstractions/NothingServices.Abstractions.csproj
```
```shell
dotnet build NothingServices.Abstractions/NothingServices.Abstractions.csproj \
  --configuration "Release" \
  --no-restore
```
```shell
dotnet nuget add source "NothingServices.Abstractions/bin/Release" --name "NothingServices.Abstractions"
```
### 5. Собрать консольное приложение
```shell
dotnet restore NothingServices.ConsoleApp/NothingServices.ConsoleApp.csproj
```
```shell
dotnet build NothingServices.ConsoleApp/NothingServices.ConsoleApp.csproj \
  --configuration "Release" \
  --no-restore
```
### 5. Создать сертификат
```shell
dotnet dev-certs https --clean
```
```shell
dotnet dev-certs https -ep .certificates/localhost.crt -p localhost --trust
```
### 6. Запустить консольное приложение
```shell
dotnet run \
  -e NOTHING_GRPC_API_URL="https://localhost:8600/nothing-grpc-api" \
  -e NOTHING_WEB_API_URL="https://localhost:8800/nothing-web-api" \
  -e NOTHING_CERTIFICATE_FILE_NAME=".certificates/localhost.crt" \
  -e NOTHING_CERTIFICATE_PASSWORD="localhost" \
  --project NothingServices.ConsoleApp/NothingServices.ConsoleApp.csproj \
  --configuration "Release" \
  --no-restore
```