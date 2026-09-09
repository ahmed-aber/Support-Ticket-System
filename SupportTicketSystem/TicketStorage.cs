using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace SupportTicketSystem
{
    // Responsible ONLY for reading/writing tickets to disk as JSON.
    public class TicketStorage
    {
        private readonly string filePath;

        public TicketStorage(string filePath)
        {
            this.filePath = filePath;
        }

        public void Save(IEnumerable<Ticket> tickets)
        {
            try
            {
                string json = JsonConvert.SerializeObject(tickets, Formatting.Indented);
                File.WriteAllText(filePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Warning: could not save tickets ({ex.Message}).");
            }
        }

        public List<Ticket> Load()
        {
            if (!File.Exists(filePath))
            {
                // First run, or the file was deleted -> start with an empty system.
                return new List<Ticket>();
            }

            try
            {
                string json = File.ReadAllText(filePath);

                if (string.IsNullOrWhiteSpace(json))
                {
                    return new List<Ticket>();
                }

                List<Ticket> loaded = JsonConvert.DeserializeObject<List<Ticket>>(json);
                return loaded ?? new List<Ticket>();
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Warning: stored data is corrupted and was ignored ({ex.Message}).");
                return new List<Ticket>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Warning: could not load tickets ({ex.Message}).");
                return new List<Ticket>();
            }
        }
    }
}
