using System.Collections.Generic;

namespace SupportTicketSystem
{
    // Responsible ONLY for deciding processing order: High -> Medium -> Low,
    // FIFO within each priority level.
    public class TicketProcessor
    {
        private Queue<Ticket> highPriorityQueue;
        private Queue<Ticket> mediumPriorityQueue;
        private Queue<Ticket> lowPriorityQueue;

        public TicketProcessor()
        {
            highPriorityQueue = new Queue<Ticket>();
            mediumPriorityQueue = new Queue<Ticket>();
            lowPriorityQueue = new Queue<Ticket>();
        }

        public void Enqueue(Ticket ticket)
        {
            switch (ticket.Priority)
            {
                case Priority.High:
                    highPriorityQueue.Enqueue(ticket);
                    break;

                case Priority.Medium:
                    mediumPriorityQueue.Enqueue(ticket);
                    break;

                case Priority.Low:
                    lowPriorityQueue.Enqueue(ticket);
                    break;
            }
        }

        // Returns the raw next candidate without checking if it's still valid.
        // TicketManager is responsible for skipping stale (removed/closed) entries.
        public Ticket DequeueNextCandidate()
        {
            if (highPriorityQueue.Count > 0)
            {
                return highPriorityQueue.Dequeue();
            }

            if (mediumPriorityQueue.Count > 0)
            {
                return mediumPriorityQueue.Dequeue();
            }

            if (lowPriorityQueue.Count > 0)
            {
                return lowPriorityQueue.Dequeue();
            }

            return null;
        }
    }
}
