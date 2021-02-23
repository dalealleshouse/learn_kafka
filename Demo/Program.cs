namespace Demo
{
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    class Program
    {
        static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args).ConfigureServices((context, collection) =>
            {
                collection.AddHostedService<Consumer>();
                collection.AddHostedService<Producer>();
            });
    }
}
