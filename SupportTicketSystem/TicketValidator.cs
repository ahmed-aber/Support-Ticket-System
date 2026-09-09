using System;

namespace SupportTicketSystem
{
    // Responsible ONLY for turning raw user/stored input into valid domain values.
    public static class TicketValidator
    {
        public static bool ValidateTitle(string title)
        {
            return !string.IsNullOrWhiteSpace(title);
        }

        public static bool TryParsePriority(string input, out Priority priority)
        {
            return Enum.TryParse(input, true, out priority)
                && Enum.IsDefined(typeof(Priority), priority);
        }

        public static bool TryParseStatus(string input, out Status status)
        {
            // Accept "In Progress" (with a space) as well as "InProgress"
            string normalized = input?.Trim().Replace(" ", "");

            return Enum.TryParse(normalized, true, out status)
                && Enum.IsDefined(typeof(Status), status);
        }
    }
}
