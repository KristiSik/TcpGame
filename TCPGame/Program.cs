using System;
using TCPGame.GameInfo;

namespace TCPGame
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Game game = new Game();
                game.Field.Display();

                Player player1 = new Player("Jon", 0);
                Player player2 = new Player("Tom", 1);

                game.setPlayers(player1, player2);

                game.Field.Move(4, player1);
                game.Field.Display();


            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }


        }
    }
}
