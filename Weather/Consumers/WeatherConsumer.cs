namespace Consumers
{
    using System;
    using System.Threading;
    using System.Linq;
    using Confluent.Kafka;
    using System.Collections.Generic;

    public static class WeatherConsumer
    {
        private const string TOPIC = "weather";
        private static ConsumerConfig config = new()
        {
            BootstrapServers = "localhost:9092,localhost:9093,localhost:9094",
            AutoOffsetReset = AutoOffsetReset.Earliest,
            GroupId = "weather-consumer"
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

            using (var consumer = new ConsumerBuilder<string, string>(config)
                    .SetPartitionsAssignedHandler(PrintParitionAssignment)
                    .SetPartitionsRevokedHandler(PrintParitionRevoked)
                    .Build())
            {
                consumer.Subscribe(TOPIC);
                while (!token.IsCancellationRequested)
                {
                    try
                    {
                        var cr = consumer.Consume(token);
                        var data = new WeatherData(cr.Message.Key, cr.Message.Value);
                        Console.WriteLine($"{data}");
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
