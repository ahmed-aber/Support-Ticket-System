using Newtonsoft.Json;
using System;

namespace SupportTicketSystem
{
    public enum Priority
    {
        Low,
        Medium,
        High
    }

    public enum Status
    {
        Open,
        InProgress,
        Closed
    }

    public class Ticket
    {
        public int Id { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public string CustomerName { get; private set; }
        public Priority Priority { get; private set; }
        public Status Status { get; private set; }
        public DateTime CreationDate { get; private set; }

        public Ticket(
            int id,
            string title,
            string description,
            string customerName,
            Priority priority,
            Status status)
        {
            if (!TicketValidator.ValidateTitle(title))
            {
                throw new ArgumentException("Title cannot be empty.");
            }

            Id = id;
            Title = title;
            Description = description;
            CustomerName = customerName;
            Priority = priority;
            Status = status;
            CreationDate = DateTime.Now;
        }

        // Used only by TicketStorage when loading tickets back from disk,
        // so the original CreationDate is preserved instead of being reset to now.
        [JsonConstructor]
        public Ticket(
            int id,
            string title,
            string description,
            string customerName,
            Priority priority,
            Status status,
            DateTime creationDate)
        {
            Id = id;
            Title = title;
            Description = description;
            CustomerName = customerName;
            Priority = priority;
            Status = status;
            CreationDate = creationDate;
        }

        public void UpdateTitle(string title)
        {
            if (!TicketValidator.ValidateTitle(title))
            {
                throw new ArgumentException("Title cannot be empty.");
            }
            Title = title;
        }

        public void UpdateDescription(string description)
        {
            Description = description;
        }

        public void UpdatePriority(Priority priority)
        {
            Priority = priority;
        }

        public void UpdateStatus(Status status)
        {
            Status = status;
        }

        public void CloseTicket()
        {
            Status = Status.Closed;
        }

        public void DisplayTicket()
        {
            Console.WriteLine("----------------------------------");
            Console.WriteLine($"ID          : {Id}");
            Console.WriteLine($"Title       : {Title}");
            Console.WriteLine($"Description : {Description}");
            Console.WriteLine($"Customer    : {CustomerName}");
            Console.WriteLine($"Priority    : {Priority}");
            Console.WriteLine($"Status      : {Status}");
            Console.WriteLine($"Created     : {CreationDate}");
            Console.WriteLine("----------------------------------");
        }

        // One-line summary row used by List/Search/Sort output.
        public string ToSummaryLine()
        {
            return $"#{Id,-4} {Title,-25} {Priority,-8} {Status,-12} {CreationDate:yyyy-MM-dd HH:mm}";
        }
    }
}
