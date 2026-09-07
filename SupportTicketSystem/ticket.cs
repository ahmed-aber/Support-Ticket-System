using System;

namespace SupportTicketSystem
{
    public class Ticket
    {
        private int id;
        private string title;
        private string description;
        private string customerName;
        private string priority;
        private string status;
        private DateTime creationDate;

        public Ticket(
            int id,
            string title,
            string description,
            string customerName,
            string priority,
            string status  )
        {
            this.id = id;

            if (ValidateTitle(title))
            {
                this.title = title;
            }
        
            this.description = description;
            this.customerName = customerName;
            this.priority = priority;
            this.status = status;
            this.creationDate = DateTime.Now;
        }

        private bool ValidateTitle(string title)
        {
            return !string.IsNullOrWhiteSpace(title);
        }


        private bool ValidatePriority(string priority)
        {
            return priority == "Low" ||
                   priority == "Medium" ||
                   priority == "High";
        }




        private bool ValidateStatus(string status)
        {
            return status == "Open" ||
                   status == "In Progress" ||
                   status == "Closed";
        }
                


        public void CloseTicket()
        {
            status = "Closed";
        }



        public void DisplayTicket()
        {
            Console.WriteLine("Ticket data");
            Console.WriteLine($"ID          : {id}");
            Console.WriteLine($"Title       : {title}");
            Console.WriteLine($"Description : {description}");
            Console.WriteLine($"Customer    : {customerName}");
            Console.WriteLine($"Priority    : {priority}");
            Console.WriteLine($"Status      : {status}");
            Console.WriteLine($"Created Date: {creationDate}");
        }


    }
     
}