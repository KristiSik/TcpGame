using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using TCPGameLib.Options;

namespace TCPGameServer
{
    public class Program
    {
        private static ServiceProvider _serviceProvider;
        private static ServerSettings _appSettings;

        static void Main(string[] args)
        {
            configureServiceProvider();

            var server = _serviceProvider.GetService<Server>();
            server.Start();

            Console.ReadKey();
        }

        private static void configureServiceProvider()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();

            var serviceCollection = new ServiceCollection();
            serviceCollection.AddOptions();
            serviceCollection.Configure<ServerSettings>(config);
            configureServices(serviceCollection);

            _serviceProvider = serviceCollection.BuildServiceProvider();

            _appSettings = _serviceProvider.GetService<IOptions<ServerSettings>>().Value;
        }

        private static void configureServices(IServiceCollection services)
        {
            services.AddTransient<Server, Server>();
        }
    }
}
