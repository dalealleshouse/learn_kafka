namespace Demo.Configuration
{
    using Microsoft.Extensions.Configuration;
    using System;

    public class AppConfig
    {
        public static void InitConfg()
        {
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var builder = new ConfigurationBuilder()
                .AddJsonFile($"appsettings.json", true, true)
                .AddJsonFile($"appsettings.{env}.json", true, true)
                .AddEnvironmentVariables();
        }

        public Kafka Kafka { get; set; }
    }

    public record ProducerSettings
    {
        public string BootstrapServers { get; set; }
    }

    public record ConsumerSettings
    {
        public string BootstrapServers { get; set; }
        public string GroupId { get; set; }
    }

    public record Kafka
    {
        public string Topic { get; set; }
        public ProducerSettings ProducerSettings { get; set; }
        public ConsumerSettings ConsumerSettings { get; set; }
    }
}
