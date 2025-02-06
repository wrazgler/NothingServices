# NothingKafka
## Начало работы
### 1. [Установить Docker](https://www.docker.com)
### 2. Открыть каталог `NothingServices` в терминале
#### Например 
```shell
cd Documents/GitHub/NothingServices
```
### 3. [Создать сеть](https://docs.docker.com/engine/network/)
```shell
docker network create -d bridge nothing_kafka_network
```
### 4. [Создать контейнер базы данных Postgres](https://hub.docker.com/_/postgres)
```shell
docker run -d \
       --name postgres_nothing_kafka_db \
       --network=nothing_kafka_network \
       -p 5500:5432 \
       -e POSTGRES_DB=nothing_kafka_db \
       -e POSTGRES_USER=nothing_kafka \
       -e POSTGRES_PASSWORD=nothing_kafka \
       -v postgres_nothing_kafka_db:/var/lib/postgres_nothing_kafka_db \
       postgres
```
### 5. [Создать образ приложения](https://docs.docker.com/reference/cli/docker/build-legacy/)
```shell
docker build . -f NothingKafka/Dockerfile -t nothing_kafka
```
### 6. [Создать контейнер приложения](https://docs.docker.com/engine/containers/run/)
```shell
docker run -d \
       --name nothing_kafka \
       --network=nothing_kafk_network \
       -e POSTGRES_HOST=postgres_nothing_kafka_db \
       -e POSTGRES_PORT=5432 \
       -e POSTGRES_DB=nothing_kafka_db \
       -e POSTGRES_USER=nothing_kafka \
       -e POSTGRES_PASSWORD=nothing_kafka \
       nothing_grpc_api
```