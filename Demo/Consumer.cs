namespace Demo
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Confluent.Kafka;
    using Demo.Configuration;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    public class Consumer : IHostedService
    {
        private readonly ILogger<Consumer> _logger;
        private readonly IConsumer<string, string> _consumer;
        private readonly string _topic;

        public Consumer(ILogger<Consumer> logger, IConfiguration config)
        {
            var settings = config.GetSection("Kafka").Get<Kafka>();
            var consumerConfig = new ConsumerConfig
            {
                BootstrapServers = settings.ConsumerSettings.BootstrapServers,
                AutoOffsetReset = AutoOffsetReset.Earliest,
                GroupId = settings.ConsumerSettings.GroupId,
            };
            _topic = settings.Topic;

            _consumer = new ConsumerBuilder<string, string>(consumerConfig).Build();
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            new Thread(() => StartConsumerLoop(cancellationToken, "1")).Start();
            new Thread(() => StartConsumerLoop(cancellationToken, "2")).Start();
            return Task.CompletedTask;
        }

        private void StartConsumerLoop(CancellationToken cancellationToken, string id)
        {
            _consumer.Subscribe(this._topic);

            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var cr = this._consumer.Consume(cancellationToken);
                    _logger.LogInformation($"{id}, {cr.Partition} - Received:{cr.Message.Key} {cr.Message.Value}");
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
