using System;
using System.Collections.Generic;

namespace SupportTicketSystem
{
    internal class Program
    {
        static void Main(string[] args)
        {
            TicketStorage storage = new TicketStorage("tickets.json");
            TicketManager manager = new TicketManager();
            manager.LoadTickets(storage.Load());

            while (true)
            {
                Console.Clear();

                Console.WriteLine("      SUPPORT TICKET SYSTEM");
                Console.WriteLine("====================================");
                Console.WriteLine("1. Add Ticket");
                Console.WriteLine("2. List All Tickets");
                Console.WriteLine("3. Search Tickets");
                Console.WriteLine("4. Sort Tickets");
                Console.WriteLine("5. Get Next Ticket to Process");
                Console.WriteLine("6. Update Ticket");
                Console.WriteLine("7. Close Ticket");
                Console.WriteLine("8. Remove Ticket");
                Console.WriteLine("9. Exit");
                Console.WriteLine();

                Console.Write("Choose an option: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1": AddTicket(manager); break;
                    case "2": ListTickets(manager); break;
                    case "3": SearchTickets(manager); break;
                    case "4": SortTickets(manager); break;
                    case "5": GetNextTicketToProcess(manager); break;
                    case "6": UpdateTicket(manager); break;
                    case "7": CloseTicket(manager); break;
                    case "8": RemoveTicket(manager); break;

                    case "9":
                        storage.Save(manager.GetAllTickets());
                        Console.WriteLine("Thank you for using Support Ticket System.");
                        return;

                    default:
                        Console.WriteLine("Invalid choice!");
                        break;
                }

                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
            }
        }

        static bool TryReadId(out int id)
        {
            Console.Write("Enter Ticket ID: ");
            string input = Console.ReadLine();

            if (!int.TryParse(input, out id))
            {
                Console.WriteLine("Invalid Ticket ID. It must be a number.");
                return false;
            }

            return true;
        }

        static void AddTicket(TicketManager manager)
        {
            if (!TryReadId(out int id))
            {
                return;
            }

            Console.Write("Enter Title: ");
            string title = Console.ReadLine();

            Console.Write("Enter Description: ");
            string description = Console.ReadLine();

            Console.Write("Enter Customer Name: ");
            string customer = Console.ReadLine();

            Console.Write("Enter Priority (Low / Medium / High): ");
            string priorityInput = Console.ReadLine();

            if (!TicketValidator.TryParsePriority(priorityInput, out Priority priority))
            {
                Console.WriteLine("Invalid priority. Must be Low, Medium, or High.");
                return;
            }

            try
            {
                Ticket ticket = new Ticket(id, title, description, customer, priority, Status.Open);
                bool added = manager.AddTicket(ticket);
                Console.WriteLine(added ? "\nTicket added successfully." : "\nTicket ID already exists.");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"\nCould not create ticket: {ex.Message}");
            }
        }

        static void ListTickets(TicketManager manager)
        {
            PrintTicketList(new List<Ticket>(manager.GetAllTickets()));
        }

        static void PrintTicketList(List<Ticket> ticketsToShow)
        {
            if (ticketsToShow.Count == 0)
            {
                Console.WriteLine("No tickets found.");
                return;
            }

            Console.WriteLine($"{"ID",-5}{"Title",-27}{"Priority",-10}{"Status",-14}{"Created",-18}");
            Console.WriteLine(new string('-', 74));

            foreach (Ticket t in ticketsToShow)
            {
                Console.WriteLine(t.ToSummaryLine());
            }
        }

        static void SearchTickets(TicketManager manager)
        {
            Console.WriteLine("Search by: 1) ID   2) Title   3) Status");
            Console.Write("Choose an option: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    if (!TryReadId(out int id)) return;
                    PrintTicketList(manager.SearchById(id));
                    break;

                case "2":
                    Console.Write("Enter part of the title: ");
                    string keyword = Console.ReadLine();
                    PrintTicketList(manager.SearchByTitle(keyword));
                    break;

                case "3":
                    Console.Write("Enter status (Open / In Progress / Closed): ");
                    string statusInput = Console.ReadLine();

                    if (!TicketValidator.TryParseStatus(statusInput, out Status status))
                    {
                        Console.WriteLine("Invalid status.");
                        return;
                    }

                    PrintTicketList(manager.SearchByStatus(status));
                    break;

                default:
                    Console.WriteLine("Invalid choice.");
                    break;
            }
        }

        static void SortTickets(TicketManager manager)
        {
            Console.WriteLine("Sort by: 1) Creation Date   2) Priority   3) Title");
            Console.Write("Choose an option: ");
            string choice = Console.ReadLine();

            List<Ticket> sorted;

            switch (choice)
            {
                case "1": sorted = manager.SortByDate(); break;
                case "2": sorted = manager.SortByPriority(); break;
                case "3": sorted = manager.SortByTitle(); break;
                default:
                    Console.WriteLine("Invalid choice.");
                    return;
            }

            PrintTicketList(sorted);
        }

        static void GetNextTicketToProcess(TicketManager manager)
        {
            Ticket next = manager.GetNextTicketToProcess();

            if (next != null)
            {
                Console.WriteLine("Next ticket to process:");
                next.DisplayTicket();
            }
            else
            {
                Console.WriteLine("No tickets are pending processing.");
            }
        }

        static void UpdateTicket(TicketManager manager)
        {
            if (!TryReadId(out int id))
            {
                return;
            }

            Ticket existing = manager.FindTicketById(id);

            if (existing == null)
            {
                Console.WriteLine("Ticket not found.");
                return;
            }

            Console.WriteLine("Leave a field blank to keep its current value.");

            Console.Write($"Title [{existing.Title}]: ");
            string title = Console.ReadLine();

            Console.Write($"Description [{existing.Description}]: ");
            string description = Console.ReadLine();

            Console.Write($"Priority [{existing.Priority}] (Low/Medium/High): ");
            string priorityInput = Console.ReadLine();

            Console.Write($"Status [{existing.Status}] (Open/In Progress/Closed): ");
            string statusInput = Console.ReadLine();

            Priority? newPriority = null;
            if (!string.IsNullOrWhiteSpace(priorityInput))
            {
                if (!TicketValidator.TryParsePriority(priorityInput, out Priority parsedPriority))
                {
                    Console.WriteLine("Invalid priority. Update cancelled.");
                    return;
                }
                newPriority = parsedPriority;
            }

            Status? newStatus = null;
            if (!string.IsNullOrWhiteSpace(statusInput))
            {
                if (!TicketValidator.TryParseStatus(statusInput, out Status parsedStatus))
                {
                    Console.WriteLine("Invalid status. Update cancelled.");
                    return;
                }
                newStatus = parsedStatus;
            }

            try
            {
                bool updated = manager.UpdateTicket(
                    id,
                    string.IsNullOrWhiteSpace(title) ? null : title,
                    string.IsNullOrWhiteSpace(description) ? null : description,
                    newPriority,
                    newStatus
                );

                Console.WriteLine(updated ? "Ticket updated successfully." : "Ticket not found.");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Could not update ticket: {ex.Message}");
            }
        }

        static void CloseTicket(TicketManager manager)
        {
            if (!TryReadId(out int id))
            {
                return;
            }

            Console.WriteLine(manager.CloseTicket(id) ? "Ticket closed successfully." : "Ticket not found.");
        }

        static void RemoveTicket(TicketManager manager)
        {
            if (!TryReadId(out int id))
            {
                return;
            }

            Console.WriteLine(manager.RemoveTicket(id) ? "Ticket removed successfully." : "Ticket not found.");
        }
    }
}
