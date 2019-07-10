using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using TCPGameLib.Options;

namespace TCPGame
{
    public class Program
    {
        private static ServiceProvider _serviceProvider;
        private static ClientSettings _appSettings;

        static void Main(string[] args)
        {
            configureServiceProvider();

            var client = _serviceProvider.GetService<Client>();
            client.ConnectToServer();

            Console.ReadKey();
        }

        private static void configureServiceProvider()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();

            var serviceCollection = new ServiceCollection();
            serviceCollection.AddOptions();
            serviceCollection.Configure<ClientSettings>(config);
            configureServices(serviceCollection);

            _serviceProvider = serviceCollection.BuildServiceProvider();

            _appSettings = _serviceProvider.GetService<IOptions<ClientSettings>>().Value;
        }

        private static void configureServices(IServiceCollection services)
        {
            services.AddTransient<Client, Client>();
        }
    }
}
