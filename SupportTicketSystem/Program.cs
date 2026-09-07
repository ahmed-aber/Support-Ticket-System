using System;

namespace SupportTicketSystem
{
    internal class Program
    {
        static void Main(string[] args)
        {
            TicketManager manager = new TicketManager();

            while (true)
            {
                Console.Clear();
           
                Console.WriteLine("      SUPPORT TICKET SYSTEM");
                Console.WriteLine("====================================");
                Console.WriteLine("1. Add Ticket");
                Console.WriteLine("2. Find Ticket");
                Console.WriteLine("3. Remove Ticket");
                Console.WriteLine("4. Close Ticket");
                Console.WriteLine("5. Exit");
                Console.WriteLine();

                Console.Write("Choose an option: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddTicket(manager);
                        break;

                    case "2":
                        FindTicket(manager);
                        break;

                    case "3":
                        RemoveTicket(manager);
                        break;

                    case "4":
                        CloseTicket(manager);
                        break;

                    case "5":
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

        // دي خارج Main ولكن داخل Program
        static void AddTicket(TicketManager manager)
        {
            Console.Write("Enter Ticket ID: ");
            int id = int.Parse(Console.ReadLine());

            Console.Write("Enter Title: ");
            string title = Console.ReadLine();

            Console.Write("Enter Description: ");
            string description = Console.ReadLine();

            Console.Write("Enter Customer Name: ");
            string customer = Console.ReadLine();

            Console.Write("Enter Priority (Low / Medium / High): ");
            string priority = Console.ReadLine();

            Ticket ticket = new Ticket(
                id,
                title,
                description,
                customer,
                priority,
                "Open"
            );

            bool added = manager.AddTicket(id, ticket);

            if (added)
            {
                Console.WriteLine("\nTicket added successfully.");
            }
            else
            {
                Console.WriteLine("\nTicket ID already exists.");
            }
        }




        static void FindTicket(TicketManager manager)
        {
            Console.Write("Enter Ticket ID: ");
            int id = int.Parse(Console.ReadLine());

            Ticket ticket = manager.FindTicketById(id);

            if (ticket != null)
            {
                ticket.DisplayTicket();
            }
            else
            {
                Console.WriteLine("Ticket not found.");
            }
        }





        static void RemoveTicket(TicketManager manager)
        {
            Console.Write("Enter Ticket ID: ");
            int id = int.Parse(Console.ReadLine());

            if (manager.RemoveTicket(id))
            {
                Console.WriteLine("Ticket removed successfully.");
            }
            else
            {
                Console.WriteLine("Ticket not found."); 
            }
        }




        static void CloseTicket(TicketManager manager)
        {
            Console.Write("Enter Ticket ID: ");
            int id = int.Parse(Console.ReadLine());

            if (manager.CloseTicket(id))
            {
                Console.WriteLine("Ticket closed successfully.");
            }
            else
            {
                Console.WriteLine("Ticket not found.");
            }
        }
    }
}