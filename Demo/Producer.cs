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

    public class Producer : IHostedService
    {
        private readonly ILogger<Producer> _logger;
        private readonly IProducer<int, string> _producer;
        private readonly string _topic;

        public Producer(ILogger<Producer> logger, IConfiguration config)
        {
            var settings = config.GetSection("Kafka").Get<Kafka>();
            var producerConfig = new ProducerConfig()
            {
                BootstrapServers = settings.ProducerSettings.BootstrapServers
            };
            this._topic = settings.Topic;

            _producer = new ProducerBuilder<int, string>(producerConfig).Build();
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                for (var i = 0; i < 5; ++i)
                {
                    var value = $"The topic is \"{this._topic}\" : GO! {i}";
                    await _producer.ProduceAsync(this._topic, new Message<int, string>()
                    {
                        Key = i,
                        Value = value
                    }, cancellationToken);

                    _logger.LogInformation($"topic={this._topic}, key={i} value={value}");
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Producer error: {e}");
            }
            finally
            {
                _producer.Flush(TimeSpan.FromSeconds(10));
            }

        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _producer?.Dispose();
            return Task.CompletedTask;
        }
    }
}
