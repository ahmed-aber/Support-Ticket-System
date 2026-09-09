# Architectural & Design Documentation (`DESIGN.md`)

## 🏗️ 1. Architecture & OOP Design

The system strictly follows the **Single Responsibility Principle (SRP)** and Object-Oriented Principles:

- **`Ticket`**: Represents the domain entity and handles its own internal state encapsulation.
- **`TicketManager`**: Acts as the system coordinator / aggregate root holding the master collection of tickets.
- **`TicketProcessor`**: Encapsulates processing order algorithms using priority queues.
- **`TicketSearcher` / `TicketSorter`**: Pure utility modules for query and ordering operations.
- **`TicketStorage`**: Manages JSON data read/write persistence.
- **`TicketValidator`**: Validates user inputs before hitting core domain logic.

---

## 📊 2. Data Structures Used & Justifications

| Concern / Module | Selected Data Structure | Justification / Reason |
| :--- | :--- | :--- |
| **In-Memory Master Storage** | `Dictionary<int, Ticket>` | Enables **O(1)** direct lookup, updates, and removal by Ticket ID. |
| **Priority Processing Order** | Three Separate `Queue<Ticket>` | Implements FIFO per priority level (High, Medium, Low) to achieve **O(1)** enqueuing and dequeuing without high priority preemption complexity. |
| **Sorting Operations** | `List<Ticket>` | Works with C#'s underlying Introspective Sort (Quicksort/Heapsort hybrid) providing reliable average **O(N log N)** time. |

---

## ⏱️ 3. Algorithmic Complexity (Big-O Analysis)

| Operation | Time Complexity | Space Complexity | Reason / Explanation |
| :--- | :--- | :--- | :--- |
| **Add Ticket** | **O(1)** | O(1) | Inserting into a `Dictionary` and `Queue` is constant time. |
| **Find Ticket by ID** | **O(1)** | O(1) | Direct key hash-lookup in `Dictionary`. |
| **Search by Title** | **O(N)** | O(K) | Full-table linear scan across all tickets for substring match. |
| **Search by Status** | **O(N)** | O(K) | Linear scan filtering entries matching status enum. |
| **Get Next Ticket** | **O(1)** Amortized | O(1) | Pulling from the highest priority queue is O(1). If stale/closed tickets are encountered, it skips them in amortized O(1). |
| **Update Ticket** | **O(1)** | O(1) | Direct lookup via Dictionary + updating fields / re-queuing. |
| **Remove Ticket** | **O(1)** | O(1) | Direct deletion from Dictionary by ID key. |
| **Sort Tickets** | **O(N log N)** | O(N) | Hybrid Introsort algorithm on temporary array lists. |

---

## 🔄 4. Priority Queue & Lazy Deletion Trade-offs
Instead of using a heavy binary heap or spending **O(N)** to remove updated/deleted items from inside a Queue, our system uses **Lazy Deletion / Stale-Check Strategy**:
- When a ticket is closed or removed, it is erased immediately from the main `Dictionary<int, Ticket>`.
- When `GetNextTicketToProcess()` pops a candidate from the queues, it checks if it still exists in the Dictionary and is active. If not, it skips it instantly and takes the next one.
- **Advantage:** Keeps enqueue and dequeue operations at **O(1)** speed without costly inner-queue cleanups.