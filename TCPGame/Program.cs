using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using TCPGame.Options;

namespace TCPGame
{
    public class Program
    {
        private static ServiceProvider _serviceProvider;
        static void Main(string[] args)
        {
            configureServiceProvider();
            ConnectionManager connectionManager = _serviceProvider.GetService<ConnectionManager>();

        }

        private static void configureServiceProvider()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();

            var serviceCollection = new ServiceCollection();
            serviceCollection.AddOptions();
            serviceCollection.Configure<AppSettings>(config);
            configureServices(serviceCollection);

            _serviceProvider = serviceCollection.BuildServiceProvider();
        }

        private static void configureServices(IServiceCollection services)
        {
            services.AddTransient<ConnectionManager, ConnectionManager>();
        }
    }
}
