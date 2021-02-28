namespace Consumers
{
    using System;
    using System.Threading;
    using System.Linq;
    using Confluent.Kafka;
    using System.Collections.Generic;
    using Confluent.SchemaRegistry.Serdes;
    using Weather.Domain;
    using Confluent.SchemaRegistry;
    using Confluent.Kafka.SyncOverAsync;

    public static class WeatherConsumer
    {
        private const string TOPIC = "weather";
        private static ConsumerConfig config = new()
        {
            BootstrapServers = "localhost:9092,localhost:9093,localhost:9094",
            AutoOffsetReset = AutoOffsetReset.Earliest,
            GroupId = "weather-consumer"
        };
        private static SchemaRegistryConfig registryConfig = new()
        {
            Url = "localhost:8081"

        };

        public static void PrintParitionAssignment<TKey, TValue>(IConsumer<TKey, TValue> consumer,
                List<TopicPartition> partitions)
        {
            var plist = partitions.Select(p => p.Partition).PrettyFormat();
            Console.WriteLine($"{consumer.Name} now has paritions {plist}");
        }

        public static void PrintParitionRevoked<TKey, TValue>(IConsumer<TKey, TValue> consumer,
                List<TopicPartitionOffset> partitions)
        {
            var plist = partitions.Select(p => p.Partition).PrettyFormat();
            Console.WriteLine($"{consumer.Name} no longer has paritions {plist}");
        }

        public static void Start()
        {
            Console.WriteLine("Consumer Start Polling");
            var tokenSource = new CancellationTokenSource();
            var token = tokenSource.Token;
            Console.CancelKeyPress += (s, e) => { e.Cancel = true; tokenSource.Cancel(); };

            using (var schemaRegistry = new CachedSchemaRegistryClient(registryConfig))
            {
                using (var consumer = new ConsumerBuilder<string, WeatherData>(config)
                        .SetPartitionsAssignedHandler(PrintParitionAssignment)
                        .SetPartitionsRevokedHandler(PrintParitionRevoked)
                        .SetValueDeserializer(new AvroDeserializer<WeatherData>(schemaRegistry).AsSyncOverAsync())
                        .Build())
                {
                    consumer.Subscribe(TOPIC);
                    while (!token.IsCancellationRequested)
                    {
                        try
                        {
                            var cr = consumer.Consume(token);
                            Console.WriteLine($"{cr.Message.Value.PrettyFormat()}");
                        }
                        catch (OperationCanceledException)
                        {
                            // ignore
                        }
                        catch (ConsumeException e)
                        {
                            if (e.Error.IsFatal)
                            {
                                // https://github.com/edenhill/librdkafka/blob/master/INTRODUCTION.md#fatal-consumer-errors
                                Console.WriteLine($"Fatal Consume error: {e.Error.Reason}");
                                break;
                            }

                            Console.WriteLine($"Consume error: {e.Error.Reason}");
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine($"Fatal Consume error: {e}");
                            break;
                        }
                    }

                    Console.WriteLine("Consumer Closing");
                    consumer.Close();
                }
            }
        }
    }
}
