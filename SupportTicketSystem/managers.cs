using System;
using System.Collections.Generic;
    

namespace SupportTicketSystem
{
    public class TicketManager
    {
        private Dictionary<int, Ticket> tickets;

        public TicketManager()
        {
            tickets = new Dictionary<int, Ticket>();
        }


        //add ticket
        public bool AddTicket(int id, Ticket ticket)
        {
            if (tickets.ContainsKey(id))
            {
                return false;
            }

            tickets.Add(id, ticket);
            return true;
        }


        //Find ticket by id
        public Ticket FindTicketById(int id)
        {
            if (tickets.ContainsKey(id))
            {
                return tickets[id];
            }

            return null;
        }


        //Remove ticket
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

    }
}

