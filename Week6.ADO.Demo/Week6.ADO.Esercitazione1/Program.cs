using System;
using System.Collections.Generic;
using Week6.ADO.Esercitazioni.Lib;

namespace Week6.ADO.Esercitazione1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            bool quit = false;
            do
            {
                string command = ConsoleHelpers.BuildMenu("Main Menu",
                    new List<string> {
                        "[ 1 ] - Elenco Tickets",
                        "[ 2 ] - Aggiungi Ticket",
                        "[ 3 ] - Cancella Ticket",
                        "[ q ] - QUIT"
                    });

                switch (command)
                {
                    case "1":
                        // list tickets
                        ConnectedModeClient.ListTickets();
                        break;
                    case "2":
                        // add new ticket
                        ConnectedModeClient.AddTicket();
                        break;
                    case "3":
                        // delete ticket
                        ConnectedModeClient.DeleteTicket();
                        break;
                    case "q":
                        quit = true;
                        break;
                    default:
                        Console.WriteLine("Comando sconosciuto.");
                        break;
                }

            } while (!quit);

        }
    }
}
