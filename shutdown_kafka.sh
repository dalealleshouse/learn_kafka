#!/bin/bash
docker stop kafka
docker rm kafka
docker stop zookeeper
docker rm zookeeper
docker network rm kafka
