﻿using System;

namespace TCPGameLib.Extensions
{
    public static class ConsoleExtensions
    {
        public static void WriteErrorMessage(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        public static void WriteSuccessMessage(string message)
        {

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(message);
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        public static string RequestPlayerName()
        {
            Console.Write("Player name: ");
            return Console.ReadLine();
        }
    }
}
