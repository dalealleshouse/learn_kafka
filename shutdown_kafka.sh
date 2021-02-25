#!/bin/bash
docker stop kafka
docker rm kafka

docker stop kafka2
docker rm kafka2

docker stop zookeeper
docker rm zookeeper

docker network rm kafka
