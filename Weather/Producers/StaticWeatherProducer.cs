namespace Producers
{
    using System;
    using System.Threading;
    using Confluent.Kafka;

    public static class StaticWeatherProducer
    {
        private const string TOPIC = "weather";
        private const int PAUSE = 1000;
        private static ProducerConfig config = new()
        {
            BootstrapServers = "localhost:9092,localhost:9093,localhost:9094"
        };

        private static Action<DeliveryReport<string, string>> handler = r =>
                    Console.WriteLine(!r.Error.IsError
                        ? $"Delivered {r.Key}, {r.Value} to {r.TopicPartitionOffset}"
                        : $"Delivery Error: {r.Error.Reason}");

        public static void Start()
        {
            var cancelled = false;
            Console.CancelKeyPress += (_, e) =>
            {
                e.Cancel = true;
                cancelled = true;
            };

            using (var producer = new ProducerBuilder<string, string>(config).Build())
            {
                Console.WriteLine("Producer Started");
                int generation = 0;
                var currentDateTime = new DateTime(2020, 1, 1);

                while (!cancelled)
                {
                    foreach (var (location, weather) in WeatherData.GetWeather(currentDateTime, generation))
                    {
                        try
                        {
                            producer.Produce(TOPIC, new Message<string, string>
                            {
                                Key = $"ReproducibleWeatherSource${location}",
                                Value = weather
                            }, handler);
                        }
                        catch (ProduceException<string, string> e)
                        {
                            Console.WriteLine($"failed to deliver message: {e.Message} [{e.Error.Code}]");
                        }
                        generation++;
                        currentDateTime = currentDateTime.AddHours(1);
                    }

                    Thread.Sleep(PAUSE);
                }

                producer.Flush();
            }

            Console.WriteLine("Producer Exited");
        }
    }
}
