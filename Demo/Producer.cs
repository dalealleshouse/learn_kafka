namespace Demo
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Confluent.Kafka;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    public class Producer : IHostedService
    {
        private readonly ILogger<Producer> _logger;
        private readonly IProducer<int, string> _producer;
        private readonly string _topic = "demo";

        public Producer(ILogger<Producer> logger)
        {
            _logger = logger;
            var config = new ProducerConfig() { BootstrapServers = "localhost:9092" };
            _producer = new ProducerBuilder<int, string>(config).Build();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            for (var i = 0; i < 100; ++i)
            {
                var value = $"I'm a Kafka Message {i}";
                await _producer.ProduceAsync(this._topic, new Message<int, string>()
                {
                    Key = i,
                    Value = value
                }, cancellationToken);

                _logger.LogInformation($"topic={this._topic}, key={i} value={value}");
            }

            _producer.Flush(TimeSpan.FromSeconds(10));
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _producer?.Dispose();
            return Task.CompletedTask;
        }
    }
}
