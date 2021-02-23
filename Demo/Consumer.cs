namespace Demo
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Confluent.Kafka;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    public class Consumer : IHostedService
    {
        private readonly ILogger<Consumer> _logger;
        private readonly IConsumer<int, string> _consumer;
        private readonly string _topic = "demo";

        public Consumer(ILogger<Consumer> logger)
        {
            _logger = logger;
            var config = new ConsumerConfig
            {
                BootstrapServers = "localhost:9092",
                AutoOffsetReset = AutoOffsetReset.Earliest,
                GroupId = "web-example-group"
            };
            _consumer = new ConsumerBuilder<int, string>(config).Build();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            new Thread(() => StartConsumerLoop(cancellationToken)).Start();
            return Task.CompletedTask;
        }

        private void StartConsumerLoop(CancellationToken cancellationToken)
        {
            _consumer.Subscribe(this._topic);

            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var cr = this._consumer.Consume(cancellationToken);
                    _logger.LogInformation($"Received:{cr.Message.Key} {cr.Message.Value}");
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (ConsumeException e)
                {
                    // Consumer errors should generally be ignored (or logged) unless fatal. 
                    _logger.LogWarning($"Consume error: {e.Error.Reason}");

                    if (e.Error.IsFatal)
                    {
                        // https://github.com/edenhill/librdkafka/blob/master/INTRODUCTION.md#fatal-consumer-errors
                        break;
                    }
                }
                catch (Exception e)
                {
                    _logger.LogError($"Consume error: {e}");
                    break;
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _consumer?.Close();
            _consumer?.Dispose();
            return Task.CompletedTask;
        }
    }
}
