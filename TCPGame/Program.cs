using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using TCPGameLib.GameInfo;
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
            InterfaceTest interfaceTest = new InterfaceTest();
            Game game = _serviceProvider.GetService<Game>();

            try
            {
                Player player1 = new Player("Jon");
                Player player2 = new Player("Tom");
                
                game.AddPlayer(player1);
                game.AddPlayer(player2);

                game.Start();

                // Horizontal
                //game.Field.Move(6, player1);
                //game.Field.Display();

                //game.Field.Move(7, player1);
                //game.Field.Display();

                //game.Field.Move(8, player1);
                //game.Field.Display();

                //Vertical
                //game.Field.Move(2, player1);
                //game.Field.Display();

                //game.Field.Move(5, player1);
                //game.Field.Display();

                //game.Field.Move(5, player1);
                //game.Field.Display();

                //DiagonalA
                //game.Field.Move(0, player1);
                //game.Field.Display();
                //
                //game.Field.Move(4, player1);
                //game.Field.Display();
                //
                //game.Field.Move(8, player1);
                //game.Field.Display();
                
                //DiagonalB
                game.Field.Move(2, player1);
                game.Field.Display();

                game.Field.Move(4, player1);
                game.Field.Display();

                game.Field.Move(6, player1);
                game.Field.Display();

                if (game.Field.CheckField())
                {
                    Console.WriteLine("Success");
                }
                else
                {
                    Console.WriteLine("Error");
                }
            }
            catch (Exception ex)
            {
                ConsoleExtensions.WriteErrorMessage(ex.Message);
            }
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
