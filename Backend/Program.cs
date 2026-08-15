using TicketDashboard;

var manager = new TicketManager();

Console.WriteLine("=== All Tickets ===");
foreach (var ticket in manager.GetAllTickets())
{
    Console.WriteLine($"#{ticket.Id} {ticket.Title} (assigned: {ticket.AssignedTo ?? "unassigned"})");
}

Console.WriteLine();
Console.WriteLine("=== High Priority Tickets (Critical / High) ===");
foreach (var ticket in manager.GetHighPriorityTickets())
{
    Console.WriteLine($"[{ticket.PriorityLevel}] #{ticket.Id} {ticket.Title}");
}

Console.WriteLine();
Console.WriteLine("=== Ticket Counts By Status ===");
foreach (var kvp in manager.GetTicketCountsByStatus())
{
    Console.WriteLine($"{kvp.Key}: {kvp.Value}");
}



Console.WriteLine();
Console.WriteLine("=== Tickets Sorted Newest to Oldest ===");
foreach (var ticket in manager.SortTicketsByDate())
{
    Console.WriteLine($"{ticket.CreatedDate:yyyy-MM-dd} - #{ticket.Id} {ticket.Title}");
}

Console.WriteLine();
Console.WriteLine("=== Tickets Sorted By Priority, Then Newest ===");
foreach (var ticket in manager.SortTicketsByPriorityThenDate())
{
    Console.WriteLine($"[{ticket.PriorityLevel}] {ticket.CreatedDate:yyyy-MM-dd} - #{ticket.Id} {ticket.Title}");
}

Console.WriteLine();
Console.WriteLine("=== Average Resolution Time (Days) ===");
Console.WriteLine(manager.GetAverageResolutionDays().ToString("0.##"));

Console.WriteLine();
Console.WriteLine("=== Tickets Assigned To Maya Patel ===");
foreach (var ticket in manager.GetTicketsByAssignee("Maya Patel"))
{
    Console.WriteLine($"#{ticket.Id} {ticket.Title}");
}

Console.WriteLine();
Console.WriteLine("=== Unassigned Tickets ===");
foreach (var ticket in manager.GetTicketsByAssignee(null))
{
    Console.WriteLine($"#{ticket.Id} {ticket.Title}");
}

Console.WriteLine();
Console.WriteLine("=== Search Results For \"network\" ===");
foreach (var ticket in manager.SearchTickets("network"))
{
    Console.WriteLine($"#{ticket.Id} {ticket.Title}");
}

Console.WriteLine();
Console.WriteLine("=== Unresolved Tickets (Oldest First) ===");
foreach (var ticket in manager.GetUnresolvedTickets())
{
    Console.WriteLine($"{ticket.CreatedDate:yyyy-MM-dd} [{ticket.Status}] #{ticket.Id} {ticket.Title}");
}

var asOf = new DateTime(2026, 8, 13, 9, 0, 0);

Console.WriteLine();
Console.WriteLine("=== SLA Breaches (as of 2026-08-13 09:00) ===");
foreach (var ticket in manager.GetSlaBreaches(asOf))
{
    Console.WriteLine($"[{ticket.PriorityLevel}] #{ticket.Id} {ticket.Title}");
}

// Console.WriteLine();
// Console.WriteLine("=== Escalated Tickets (as of 2026-08-13 09:00) ===");
// foreach (var ticket in manager.GetEscalatedTickets(asOf))
// {
//     Console.WriteLine($"#{ticket.Id} {ticket.Title} -> [{ticket.PriorityLevel}]");
// }
