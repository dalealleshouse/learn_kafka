Start up script:
``` bash
./start_kafka.sh
```

Shutdown script:
``` bash
./shutdown_kafka.sh
```

For a Kafka CLI
``` bash
docker exec -it kafka bash
```
Quick start commands: <https://kafka.apache.org/quickstart>

Inside docker, logs are stored here: `/var/lib/kafka/data/`

Helpful Kafka CLI commands
``` bash
# Topic health
kafka-topics --describe --topic demo --zookeeper zookeeper:2181

# Create a console producer
kafka-console-producer --topic demo --bootstrap-server kafka:9092

# Create a console consumer
kafka-console-consumer --topic demo --from-beginning --bootstrap-server kafka:9092

# ZooKeeper terminal
zookeeper-shell kafka:2181
```
