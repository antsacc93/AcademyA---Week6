using System;
using System.Collections.Generic;
using Week6.ADO.Esercitazioni.Lib;

namespace Week6.ADO.Esercitazione2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Esercitazione 2 Week 4 ===");

            #region Main loop

            bool quit = false;
            do
            {
                string command = ConsoleHelpers.BuildMenu("Main Menu",
                    new List<string> {
                        "[ 1 ] - Elenco Tickets",
                        "[ 2 ] - Aggiungi Ticket",
                        "[ 3 ] - Cancella Ticket",
                        "[ 4 ] - Modifica stato Ticket",
                        "[ q ] - QUIT"
                    });

                switch (command)
                {
                    case "1":
                        // list tickets
                        DisconnectedModeClient.ListTickets();
                        break;
                    case "2":
                        // add new ticket
                        DisconnectedModeClient.AddTicket();
                        break;
                    case "3":
                        // delete ticket
                        DisconnectedModeClient.DeleteTicket();
                        break;
                    case "4":
                        //DisconnectedModeClient.Refresh();
                        break;
                    case "q":
                        quit = true;
                        break;
                    default:
                        Console.WriteLine("Comando sconosciuto.");
                        break;
                }

            } while (!quit);

            #endregion
        }
    }
}
