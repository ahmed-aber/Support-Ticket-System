using System;
using System.Collections.Generic;
using System.Linq;

namespace SupportTicketSystem
{
    // Responsible ONLY for finding tickets that match a search criterion.
    public static class TicketSearcher
    {
        // O(n): a substring can appear anywhere in the title, so every ticket
        // must be checked. No ordering property lets us do better without a
        // heavier structure (e.g. a suffix index), which isn't worth it here.
        public static List<Ticket> SearchByTitleContains(IEnumerable<Ticket> tickets, string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
            {
                return new List<Ticket>();
            }

            return tickets
                .Where(t => t.Title.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0)
                .ToList();
        }

        // O(n): full scan. Could become O(1) amortized with a secondary
        // Dictionary<Status, List<Ticket>> index if status search became a hot path.
        public static List<Ticket> SearchByStatus(IEnumerable<Ticket> tickets, Status status)
        {
            return tickets.Where(t => t.Status == status).ToList();
        }
    }
}
