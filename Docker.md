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
kafka-topics --describe --topic <TOPIC> --zookeeper zookeeper:2181

# Create a console producer
kafka-console-producer --topic <TOPIC> --bootstrap-server kafka:9092

# Create a console consumer
kafka-console-consumer --topic <TOPIC> --from-beginning --bootstrap-server kafka:9092

# ZooKeeper terminal
zookeeper-shell kafka:2181

# Generate test messages
kafka-producer-perf-test \
    --topic <TOPIC> \
    --num-records 50 \
    --record-size 1 \
    --throughput 10 \
    --producer-props bootstrap.servers=kafka:9092 key.serializer=org.apache.kafka.common.serialization.IntegerSerializer value.serializer=org.apache.kafka.common.serailization.StringSerializer

# Consumer offsets
kafka-topics --zookeeper zookeeper:2181 --describe --topic __consumer_offsets
```
