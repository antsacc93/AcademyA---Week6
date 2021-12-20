using System;
using System.Collections.Generic;

namespace Week6.ADO.Esercitazioni.Lib
{
    public class ConsoleHelpers
    {
        public static string BuildMenu(string title, List<string> menuItems)
        {
            Console.Clear();
            Console.WriteLine($"============= {title} =============");
            Console.WriteLine();

            foreach (var item in menuItems)
            {
                Console.WriteLine(item);
                Console.WriteLine();
            }

            // get command
            Console.Write("> ");
            string command = Console.ReadLine();
            Console.WriteLine();

            return command;
        }
        public static string GetData(string message)
        {
            Console.Write(message + ": ");
            var value = Console.ReadLine();
            return value;
        }

        public static string GetData(string message, string initialValue)
        {
            Console.Write(message + $" ({initialValue}): ");
            var value = Console.ReadLine();
            return string.IsNullOrEmpty(value) ? initialValue : value;
        }
    }
}
