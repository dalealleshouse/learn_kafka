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
    -e KAFKA_LISTENERS=LISTENER_INT://kafka:29092,LISTENER_EXT://kafka:9092 \
    -e KAFKA_ADVERTISED_LISTENERS=LISTENER_INT://kafka:29092,LISTENER_EXT://localhost:9092 \
    -e KAFKA_LISTENER_SECURITY_PROTOCOL_MAP=LISTENER_INT:PLAINTEXT,LISTENER_EXT:PLAINTEXT \
    -e KAFKA_INTER_BROKER_LISTENER_NAME=LISTENER_INT \
    -e KAFKA_OFFSETS_TOPIC_REPLICATION_FACTOR=2 \
    -e KAFKA_NUM_PARTITIONS=3 \
    -e KAFKA_BROKER_ID=1 \
    -p 9092:9092 \
    confluentinc/cp-kafka

docker run -d --network=kafka \
    --name=kafka2 \
    -e KAFKA_ZOOKEEPER_CONNECT=zookeeper:2181 \
    -e KAFKA_LISTENERS=LISTENER_INT://kafka2:29093,LISTENER_EXT://kafka2:9093 \
    -e KAFKA_ADVERTISED_LISTENERS=LISTENER_INT://kafka2:29093,LISTENER_EXT://localhost:9093 \
    -e KAFKA_LISTENER_SECURITY_PROTOCOL_MAP=LISTENER_INT:PLAINTEXT,LISTENER_EXT:PLAINTEXT \
    -e KAFKA_INTER_BROKER_LISTENER_NAME=LISTENER_INT \
    -e KAFKA_OFFSETS_TOPIC_REPLICATION_FACTOR=2 \
    -e KAFKA_NUM_PARTITIONS=3 \
    -e KAFKA_BROKER_ID=2 \
    -p 9093:9093 \
    confluentinc/cp-kafka

docker run -d --network=kafka \
    --name=kafka3 \
    -e KAFKA_ZOOKEEPER_CONNECT=zookeeper:2181 \
    -e KAFKA_LISTENERS=LISTENER_INT://kafka3:29094,LISTENER_EXT://kafka3:9094 \
    -e KAFKA_ADVERTISED_LISTENERS=LISTENER_INT://kafka3:29094,LISTENER_EXT://localhost:9094 \
    -e KAFKA_LISTENER_SECURITY_PROTOCOL_MAP=LISTENER_INT:PLAINTEXT,LISTENER_EXT:PLAINTEXT \
    -e KAFKA_INTER_BROKER_LISTENER_NAME=LISTENER_INT \
    -e KAFKA_OFFSETS_TOPIC_REPLICATION_FACTOR=2 \
    -e KAFKA_NUM_PARTITIONS=3 \
    -e KAFKA_BROKER_ID=3 \
    -p 9094:9094 \
    confluentinc/cp-kafka
