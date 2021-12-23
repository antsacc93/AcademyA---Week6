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
                        "[ 5 ] - Visualizza i Ticket risolti",
                        "[ 6 ] - Visualizza i Ticket degli utenti che cominciano con la A",
                        "[ 7 ] - Visualizza i Ticket antecedenti ad un mese fa",
                        "[ 8 ] - Ordinare i ticket per data",
                        "[ 9 ] - Ordinare i ticket per descrizione",
                        "[ 10 ] - Raggruppa i ticket per stato",
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
                        DisconnectedModeClient.ModifyTicket();
                        break;
                    case "5":
                        DisconnectedModeClient.GetResolvedTickets();
                        break;
                    case "6":
                        DisconnectedModeClient.GetTicketsWithA();
                        break;
                    case "7":
                        DisconnectedModeClient.GetOldTickets();
                        break;
                    case "8":
                        DisconnectedModeClient.OrderData();
                        break;
                    case "9":
                        DisconnectedModeClient.OrderDescription();
                        break;
                    case "10":
                        DisconnectedModeClient.GroupByStatus();
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
