using System;
using System.Collections.Generic;

namespace SupportTicketSystem
{
    // Responsible ONLY for owning the ticket collection and coordinating the
    // smaller single-purpose classes (validator, searcher, sorter, processor).
    public class TicketManager
    {
        private Dictionary<int, Ticket> tickets;
        private TicketProcessor processor;

        public TicketManager()
        {
            tickets = new Dictionary<int, Ticket>();
            processor = new TicketProcessor();
        }

        public bool AddTicket(Ticket ticket)
        {
            if (tickets.ContainsKey(ticket.Id))
            {
                return false;
            }

            tickets.Add(ticket.Id, ticket);
            processor.Enqueue(ticket);
            return true;
        }

        public Ticket FindTicketById(int id)
        {
            return tickets.ContainsKey(id) ? tickets[id] : null;
        }

        public bool RemoveTicket(int id)
        {
            if (!tickets.ContainsKey(id))
            {
                return false;
            }

            tickets.Remove(id);
            return true;
        }

        public bool CloseTicket(int id)
        {
            Ticket ticket = FindTicketById(id);

            if (ticket == null)
            {
                return false;
            }

            ticket.CloseTicket();
            return true;
        }

        // Any parameter left null keeps that field unchanged.
        public bool UpdateTicket(int id, string title, string description, Priority? priority, Status? status)
        {
            Ticket ticket = FindTicketById(id);

            if (ticket == null)
            {
                return false;
            }

            if (title != null)
            {
                ticket.UpdateTitle(title);
            }

            if (description != null)
            {
                ticket.UpdateDescription(description);
            }

            if (status.HasValue)
            {
                ticket.UpdateStatus(status.Value);
            }

            if (priority.HasValue && priority.Value != ticket.Priority)
            {
                ticket.UpdatePriority(priority.Value);

                // Known simplification: the ticket's earlier entry in its old
                // priority queue is not removed, only a new one is added under
                // the new priority. See DESIGN.md for why this trade-off is
                // acceptable at this project's scale.
                processor.Enqueue(ticket);
            }

            return true;
        }

        public IEnumerable<Ticket> GetAllTickets()
        {
            return tickets.Values;
        }

        public List<Ticket> SearchById(int id)
        {
            List<Ticket> result = new List<Ticket>();
            Ticket ticket = FindTicketById(id);

            if (ticket != null)
            {
                result.Add(ticket);
            }

            return result;
        }

        public List<Ticket> SearchByTitle(string keyword)
        {
            return TicketSearcher.SearchByTitleContains(tickets.Values, keyword);
        }

        public List<Ticket> SearchByStatus(Status status)
        {
            return TicketSearcher.SearchByStatus(tickets.Values, status);
        }

        public List<Ticket> SortByDate()
        {
            return TicketSorter.SortByCreationDate(tickets.Values);
        }

        public List<Ticket> SortByPriority()
        {
            return TicketSorter.SortByPriority(tickets.Values);
        }

        public List<Ticket> SortByTitle()
        {
            return TicketSorter.SortByTitle(tickets.Values);
        }

        // Picks the next ticket to work on: High priority first, then Medium,
        // then Low, FIFO within the same priority. Skips tickets that were
        // removed or already closed after being queued, instead of handing
        // back a stale ticket.
        public Ticket GetNextTicketToProcess()
        {
            Ticket candidate;

            while ((candidate = processor.DequeueNextCandidate()) != null)
            {
                bool stillExists = tickets.ContainsKey(candidate.Id);

                if (stillExists && candidate.Status != Status.Closed)
                {
                    return candidate;
                }
            }

            return null;
        }

        // Used at startup to repopulate the manager (and processing queues)
        // from tickets loaded off disk. Closed tickets are restored for
        // viewing but are not re-queued for processing.
        public void LoadTickets(IEnumerable<Ticket> loadedTickets)
        {
            foreach (Ticket ticket in loadedTickets)
            {
                tickets[ticket.Id] = ticket;

                if (ticket.Status != Status.Closed)
                {
                    processor.Enqueue(ticket);
                }
            }
        }
    }
}
