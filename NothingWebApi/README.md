# NothingWebApi
## Начало работы
### 1. [Установить Docker](https://www.docker.com)
### 2. Открыть каталог `NothingServices` в терминале
#### Например 
```shell
cd Documents/GitHub/NothingServices
```
### 3. [Создать сеть](https://docs.docker.com/engine/network/)
```shell
docker network create -d bridge nothing_web_api_network
```
### 4. [Создать контейнер базы данных Postgres](https://hub.docker.com/_/postgres)
```shell
docker run -d \
       --name postgres_nothing_web_api_db \
       --network=nothing_web_api_network \
       -p 5300:5432 \
       -e POSTGRES_DB=nothing_web_api_db \
       -e POSTGRES_USER=nothing_web_api \
       -e POSTGRES_PASSWORD=nothing_web_api \
       -v postgres_nothing_web_api_db:/var/lib/postgres_nothing_web_api_db \
       postgres
```
### 5. [Создать образ приложения](https://docs.docker.com/reference/cli/docker/build-legacy/)
```shell
docker build . -f NothingWebApi/Dockerfile -t nothing_web_api
```
### 6. [Создать контейнер приложения](https://docs.docker.com/engine/containers/run/)
```shell
docker run -d \
       --name nothing_web_api \
       --network=nothing_web_api_network \
       -p 8100:8100 \
       -p 8200:8200 \
       -e ASPNETCORE_HTTP_PORTS=8100 \
       -e ASPNETCORE_HTTPS_PORTS=8200 \
       -e PATH_BASE=/nothing-web-api \
       -e POSTGRES_HOST=postgres_nothing_web_api_db \
       -e POSTGRES_PORT=5432 \
       -e POSTGRES_DB=nothing_web_api_db \
       -e POSTGRES_USER=nothing_web_api \
       -e POSTGRES_PASSWORD=nothing_web_api \
       nothing_web_api
```
### 7. [Открыть в браузере](https://localhost:8200/nothing-web-api/swagger/index.html)
#### http
```url
http://localhost:8100/nothing-web-api/swagger/index.html
```
#### https
```url
https://localhost:8200/nothing-web-api/swagger/index.html
```