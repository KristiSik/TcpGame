using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using TCPGame.Extensions;
using TCPGame.GameInfo;
using TCPGame.Options;

namespace TCPGame
{
    public class Program
    {
        private static ServiceProvider _serviceProvider;

        private static AppSettings _appSettings;

        static void Main(string[] args)
        {
            InterfaceTest interfaceTest = new InterfaceTest();
            Game game = _serviceProvider.GetService<Game>();

            configureServiceProvider();

            Console.ReadKey();
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

            _appSettings = _serviceProvider.GetService<IOptions<AppSettings>>().Value;
        }

        private static void configureServices(IServiceCollection services)
        {
            services.AddTransient<ConnectionManager, ConnectionManager>();
            services.AddTransient<Game, Game>();
        }
    }
}
