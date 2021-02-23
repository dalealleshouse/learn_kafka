#!/bin/bash
docker network create kafka

# Zookeeper
docker run -d --network=kafka \
    --name=zookeeper \
    -e ZOOKEEPER_CLIENT_PORT=2181 \
    -e ZOOKEEPER_TICK_TIME=2000 \
    -p 2181:2181  \
    confluentinc/cp-zookeeper

# Kafka
docker run -d --network=kafka \
    --name=kafka \
    -e KAFKA_ZOOKEEPER_CONNECT=zookeeper:2181 \
    -e KAFKA_ADVERTISED_LISTENERS=PLAINTEXT://localhost:9092 \
    -e KAFKA_OFFSETS_TOPIC_REPLICATION_FACTOR=1 \
    -p 9092:9092 \
    confluentinc/cp-kafka
