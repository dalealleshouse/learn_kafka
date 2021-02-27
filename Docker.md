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
kafka-topics --describe --zookeeper zookeeper:2181 --topic <TOPIC>

# Create a console producer
kafka-console-producer --bootstrap-server kafka:29092 --topic <TOPIC>

# Create a console consumer
kafka-console-consumer --from-beginning --bootstrap-server kafka:29092 --property print.key=true --topic <TOPIC>

# ZooKeeper terminal
zookeeper-shell kafka:2181

# Generate test messages
kafka-producer-perf-test \
    --num-records 50 \
    --record-size 1 \
    --throughput 10 \
    --producer-props bootstrap.servers=kafka:29092 value.serializer=org.apache.kafka.common.serailization.StringSerializer \
    --topic <TOPIC>

# Consumer offsets
kafka-topics --zookeeper zookeeper:2181 --describe --topic __consumer_offsets
```
