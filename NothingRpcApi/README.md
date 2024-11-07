# NothingRpcApi
## Начало работы
### 1. [Установить Docker](https://www.docker.com)
### 2. Открыть каталог `NothingServices` в терминале
#### Например 
```shell
cd Documents/GitHub/NothingServices
```
### 3. [Создать сеть](https://docs.docker.com/engine/network/)
```shell
docker network create -d bridge nothing_grpc_api_network
```
### 4. [Создать контейнер базы данных Postgres](https://hub.docker.com/_/postgres)
```shell
docker run -d \
       --name postgres_nothing_grpc_api_db \
       --network=nothing_grpc_api_network \
       -p 5400:5432 \
       -e POSTGRES_DB=nothing_grpc_api_db \
       -e POSTGRES_USER=nothing_grpc_api \
       -e POSTGRES_PASSWORD=nothing_grpc_api \
       -v postgres_nothing_grpc_api_db:/var/lib/postgres_nothing_grpc_api_db \
       postgres
```
### 5. [Создать образ приложения](https://docs.docker.com/reference/cli/docker/build-legacy/)
```shell
docker build . -f NothingRpcApi/Dockerfile -t nothing_grpc_api
```
### 6. [Создать контейнер приложения](https://docs.docker.com/engine/containers/run/)
```shell
docker run -d \
       --name nothing_grpc_api \
       --network=nothing_grpc_api_network \
       -p 8300:8300 \
       -p 8400:8400 \
       -e ASPNETCORE_HTTP_PORTS=8300 \
       -e ASPNETCORE_HTTPS_PORTS=8400 \
       -e PATH_BASE=/nothing-grpc-api \
       -e POSTGRES_HOST=postgres_nothing_grpc_api_db \
       -e POSTGRES_PORT=5432 \
       -e POSTGRES_DB=nothing_grpc_api_db \
       -e POSTGRES_USER=nothing_grpc_api \
       -e POSTGRES_PASSWORD=nothing_grpc_api \
       nothing_grpc_api
```
### 7. [Открыть в браузере](https://localhost:8400/nothing-grpc-api/swagger/index.html)
#### http
```url
http://localhost:8300/nothing-grpc-api/swagger/index.html
```
#### https
```url
https://localhost:8400/nothing-grpc-api/swagger/index.html
```