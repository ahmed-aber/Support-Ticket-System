# Mini Support Ticket System

A robust, Object-Oriented Console Application built in C# for managing, searching, sorting, and processing customer support tickets according to priority.

## 👥 Team Members
- Salah-Eldeen
- Ahmed Aber

---

## 🚀 Features & Core Capabilities
1. **Ticket Creation & Validation:**
   - Auto-validates titles, non-empty fields, priority (Low, Medium, High), and status (Open, In Progress, Closed).
   - Prevents duplicate Ticket IDs.

2. **Smart Priority Processing:**
   - Processes tickets dynamically using multiple queues based on priority level (**High ➔ Medium ➔ Low**).
   - Implements First-In, First-Out (FIFO) processing within the same priority tier.

3. **Searching & Sorting:**
   - Search by exact **Ticket ID**, **Title (substring)**, or **Status**.
   - Sort tickets dynamically by **Creation Date**, **Priority**, or **Title**.

4. **Data Persistence:**
   - System state is preserved in JSON format (`tickets.json`).
   - Gracefully handles corrupted files, missing data, and invalid disk inputs on boot.

5. **Ticket Lifetime Management:**
   - Supports editing non-immutable ticket fields (Title, Description, Priority, Status).
   - Supports closing tickets without losing their historical searchability.
   - Complete removal capability.

---

## 🛠️ How to Run
1. Open the solution file `SupportTicketSystem.sln` using Visual Studio 2019/2022.
2. Build the solution (**Ctrl + Shift + B**) to restore NuGet packages (like `Newtonsoft.Json`).
3. Press **F5** or click **Start** to run the console application.

---

## 📌 Technical Assumptions
- **Ticket ID:** Provided manually by the operator and must be a unique integer.
- **Priority Re-assignment:** When a ticket's priority is updated, it is re-enqueued into the corresponding queue. Outdated/closed entries are skipped dynamically during processing.
- **Persistence Format:** Local JSON file storage (`tickets.json`).