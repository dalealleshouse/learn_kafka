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
# docker run -d --network=kafka \
#     --name=kafka \
#     -e KAFKA_ZOOKEEPER_CONNECT=zookeeper:2181 \
#     -e KAFKA_ADVERTISED_LISTENERS=PLAINTEXT://localhost:9092 \
#     -e KAFKA_OFFSETS_TOPIC_REPLICATION_FACTOR=1 \
#     -e KAFKA_BROKER_ID=1 \
#     -p 9092:9092 \
#     confluentinc/cp-kafka

docker run -d --network=kafka \
    --name=kafka \
    -e KAFKA_ZOOKEEPER_CONNECT=zookeeper:2181 \
    -e KAFKA_LISTENERS=LISTENER_INTERNAL://kafka:29092,LISTENER_EXTERNAL://kafka:9092 \
    -e KAFKA_ADVERTISED_LISTENERS=LISTENER_INTERNAL://kafka:29092,LISTENER_EXTERNAL://localhost:9092 \
    -e KAFKA_LISTENER_SECURITY_PROTOCOL_MAP=LISTENER_INTERNAL:PLAINTEXT,LISTENER_EXTERNAL:PLAINTEXT \
    -e KAFKA_INTER_BROKER_LISTENER_NAME=LISTENER_INTERNAL \
    -e KAFKA_OFFSETS_TOPIC_REPLICATION_FACTOR=2 \
    -e KAFKA_BROKER_ID=1 \
    -p 9092:9092 \
    confluentinc/cp-kafka

docker run -d --network=kafka \
    --name=kafka2 \
    -e KAFKA_ZOOKEEPER_CONNECT=zookeeper:2181 \
    -e KAFKA_LISTENERS=LISTENER_INTERNAL://kafka2:29093,LISTENER_EXTERNAL://kafka2:9093 \
    -e KAFKA_ADVERTISED_LISTENERS=LISTENER_INTERNAL://kafka2:29093,LISTENER_EXTERNAL://localhost:9093 \
    -e KAFKA_LISTENER_SECURITY_PROTOCOL_MAP=LISTENER_INTERNAL:PLAINTEXT,LISTENER_EXTERNAL:PLAINTEXT \
    -e KAFKA_INTER_BROKER_LISTENER_NAME=LISTENER_INTERNAL \
    -e KAFKA_OFFSETS_TOPIC_REPLICATION_FACTOR=2 \
    -e KAFKA_BROKER_ID=2 \
    -p 9093:9093 \
    confluentinc/cp-kafka
