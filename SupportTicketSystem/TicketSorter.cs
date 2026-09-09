using System;
using System.Collections.Generic;

namespace SupportTicketSystem
{
    // Responsible ONLY for producing sorted views of a ticket collection.
    // List<T>.Sort is an in-place introspective sort (quicksort/heapsort/
    // insertion-sort hybrid): average O(n log n) time, O(log n) extra space.
    public static class TicketSorter
    {
        public static List<Ticket> SortByCreationDate(IEnumerable<Ticket> tickets)
        {
            List<Ticket> list = new List<Ticket>(tickets);
            list.Sort((a, b) => a.CreationDate.CompareTo(b.CreationDate));
            return list;
        }

        public static List<Ticket> SortByPriority(IEnumerable<Ticket> tickets)
        {
            List<Ticket> list = new List<Ticket>(tickets);
            // High should sort first, so compare in reverse enum order.
            list.Sort((a, b) => b.Priority.CompareTo(a.Priority));
            return list;
        }

        public static List<Ticket> SortByTitle(IEnumerable<Ticket> tickets)
        {
            List<Ticket> list = new List<Ticket>(tickets);
            list.Sort((a, b) => string.Compare(a.Title, b.Title, StringComparison.OrdinalIgnoreCase));
            return list;
        }
    }
}
