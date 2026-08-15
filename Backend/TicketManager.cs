
namespace TicketDashboard;

public class TicketManager
{
    // Lower rank = more urgent. Used for priority-aware sorting.
    private static readonly Dictionary<string, int> PriorityRank = new()
    {
        ["Critical"] = 0,
        ["High"] = 1,
        ["Medium"] = 2,
        ["Low"] = 3,
    };

    // Most urgent first. Escalating a ticket moves it one step toward index 0.
    private static readonly string[] PriorityOrder = { "Critical", "High", "Medium", "Low" };

    // How long a ticket can sit unresolved at each priority before it's
    // considered in SLA breach.
    private static readonly Dictionary<string, TimeSpan> SlaThresholds = new()
    {
        ["Critical"] = TimeSpan.FromHours(4),
        ["High"] = TimeSpan.FromHours(48),
        ["Medium"] = TimeSpan.FromDays(5),
        ["Low"] = TimeSpan.FromDays(10),
    };

    private readonly List<Ticket> _tickets = new()
    {
        new Ticket
        {
            Id = 1, Title = "Email server down", Status = "Open", PriorityLevel = "Critical",
            CreatedDate = new DateTime(2026, 8, 10), AssignedTo = "Maya Patel",
            Tags = new() { "email", "outage" },
            Comments = new() { new Comment { Author = "Maya Patel", Text = "Escalated to infra team.", Timestamp = new DateTime(2026, 8, 10) } },
        },
        new Ticket
        {
            Id = 2, Title = "Printer offline on 3rd floor", Status = "Open", PriorityLevel = "Low",
            CreatedDate = new DateTime(2026, 8, 5), AssignedTo = null,
            Tags = new() { "hardware", "printer" },
            Comments = new(),
        },
        new Ticket
        {
            Id = 3, Title = "VPN keeps disconnecting", Status = "In Progress", PriorityLevel = "High",
            CreatedDate = new DateTime(2026, 8, 9), AssignedTo = "Jordan Lee",
            Tags = new() { "network", "vpn" },
            Comments = new() { new Comment { Author = "Jordan Lee", Text = "Reproduced on Windows clients only.", Timestamp = new DateTime(2026, 8, 9) } },
        },
        new Ticket
        {
            Id = 4, Title = "New hire laptop setup", Status = "Closed", PriorityLevel = "Medium",
            CreatedDate = new DateTime(2026, 7, 28), ClosedDate = new DateTime(2026, 7, 30), AssignedTo = "Maya Patel",
            Tags = new() { "onboarding", "hardware" },
            Comments = new() { new Comment { Author = "Maya Patel", Text = "Laptop imaged and delivered.", Timestamp = new DateTime(2026, 7, 30) } },
        },
        new Ticket
        {
            Id = 5, Title = "Database replication lag", Status = "Open", PriorityLevel = "Critical",
            CreatedDate = new DateTime(2026, 8, 12), AssignedTo = "Jordan Lee",
            Tags = new() { "database", "performance" },
            Comments = new(),
        },
        new Ticket
        {
            Id = 6, Title = "Password reset request", Status = "Closed", PriorityLevel = "Low",
            CreatedDate = new DateTime(2026, 7, 30), ClosedDate = new DateTime(2026, 7, 30), AssignedTo = null,
            Tags = new() { "account" },
            Comments = new() { new Comment { Author = "Helpdesk Bot", Text = "Auto-resolved via self-service portal.", Timestamp = new DateTime(2026, 7, 30) } },
        },
        new Ticket
        {
            Id = 7, Title = "Office Wi-Fi intermittent", Status = "In Progress", PriorityLevel = "Medium",
            CreatedDate = new DateTime(2026, 8, 6), AssignedTo = null,
            Tags = new() { "network", "wifi" },
            Comments = new() { new Comment { Author = "Sam Osei", Text = "Checking access point firmware.", Timestamp = new DateTime(2026, 8, 7) } },
        },
        new Ticket
        {
            Id = 8, Title = "Payroll app throwing 500s", Status = "Open", PriorityLevel = "High",
            CreatedDate = new DateTime(2026, 8, 11), AssignedTo = "Sam Osei",
            Tags = new() { "payroll", "bug" },
            Comments = new() { new Comment { Author = "Sam Osei", Text = "Stack trace points to a null reference in the payroll service.", Timestamp = new DateTime(2026, 8, 11) } },
        },
    };

    // Returns all tickets.
    // TODO: This currently hands back a direct reference to the internal
    // list, so anything a caller does to the returned list (Add, Remove,
    // Clear, Sort...) mutates TicketManager's internal state too. Fix this
    // so GetAllTickets() returns a defensive copy instead.
    public List<Ticket> GetAllTickets()
    {
        
        return new List<Ticket> =(_tickets);
    }

    // Returns only tickets whose PriorityLevel is "Critical" or "High".
     public List<Ticket> GetHighPriorityTickets()
     {
         var result = new List<Ticket>();
        // TODO: Loop through _tickets and add any ticket with PriorityLevel
     foreach (var ticket in _tickets)
    {
        if (ticket.PriorityLevel == "Critical" ||
            ticket.PriorityLevel == "High")
        {
            result.Add(ticket);
        }
    }
        // "Critical" or "High" to result.
         return result;
     }

    // Counts tickets per status into a Dictionary<string, int>.
    // TODO: Implement this using LINQ (GroupBy + ToDictionary), not a manual
    // foreach loop.
    public Dictionary<string, int> GetTicketCountsByStatus()
    {
        var counts = new Dictionary<string, int>();
        function getTicketCountsByStatus(tickets) {
     return tickets.reduce((counts, ticket) => {
        counts[ticket.status] = (counts[ticket.status] || 0) + 1;
         return counts
          .GroupBy(t => t.Status)
        .ToDictionary(g => g.Key, g => g.Count());;
    }, {});
    }
    }
 

    // Returns tickets sorted newest to oldest by CreatedDate.
    public List<Ticket> SortTicketsByDate()
    {
        var sorted = new List<Ticket>(_tickets);
        // TODO: Sort `sorted` by CreatedDate, newest first.
        sorted.Sort((a, b) => b.CreatedDate.CompareTo(a.CreatedDate));

        return sorted;
    }
     //double check this !!
    // public Dictionary<string, int> GetTicketCountsByStatus()
    // {
    //     return _tickets
    //     .GroupBy(t => t.Status)
    //     .ToDictionary(g => g.Key, g => g.Count());
    //  }

.
    public List<Ticket> SortTicketsByPriorityThenDate()
    {
        var sorted = new List<Ticket>(_tickets);
        // TODO: Sort `sorted` by PriorityRank[t.PriorityLevel] ascending,
     function sortTicketsByPriorityThenDate(tickets) {
     const priorityRank = {
         Critical: 0,
        High: 1,
         Medium: 2,
         Low: 3
    };
        return [...tickets].sort((a, b) => {
       const priorityComparison =
             priorityRank[a.priorityLevel] - priorityRank[b.priorityLevel];

         if (priorityComparison !== 0) {
             return priorityComparison;
         }

        return new Date(b.createdDate) - new Date(a.createdDate);
    });
 }
        // then by CreatedDate descending within the same priority.
        return sorted;
    }

    // Returns the average number of days between CreatedDate and ClosedDate
    // for tickets that have been closed. Tickets without a ClosedDate must
    // be excluded. Return 0 if there are no closed tickets (don't divide by
    // zero!).
    public double GetAverageResolutionDays()
    {
        // TODO: Implement using LINQ.
        / function getAverageResolutionDays(tickets) {
     const closedTickets = tickets.filter(ticket => ticket.closedDate);

     if (closedTickets.length === 0) {
        return 0;
     }

    const totalDays = closedTickets.reduce((total, ticket) => {
        const created = new Date(ticket.createdDate);
        const closed = new Date(ticket.closedDate);

        return total + (closed - created) / (1000 * 60 * 60 * 24);     }, 0);

    return totalDays / closedTickets.length;
// }

        return 0;
    }

    // Returns tickets assigned to the given person (case-insensitive match
    // on AssignedTo). Passing null or an empty string should return the
    // tickets that are currently unassigned.
    public List<Ticket> GetTicketsByAssignee(string? assignee)
    {
        // TODO: Implement. Remember Assigned To can itself be null - don't
        // function getTicketsByAssignee(tickets, assignee) {
     if (!assignee || assignee.trim() === "") {
         return tickets.filter(ticket => ticket.assignedTo == null);     }

     return tickets.filter(ticket =>
        ticket.assignedTo != null &&
         ticket.assignedTo.toLowerCase() === assignee.toLowerCase()
     );}  // let a null AssignedTo blow up your comparison.
        return new List<Ticket>();
    }

    // Returns tickets where the keyword (case-insensitive) appears in the
    // Title, in any of the ticket's Tags, or in the text of any Comment.
    public List<Ticket> SearchTickets(string keyword)
    {
        // TODO: Implement using LINQ. You'll need Any() to look inside the

        // Tags and Comments collections on each ticket.
        return new List<Ticket>();
    }
function searchTickets(tickets, keyword) {
     if (!keyword || keyword.trim() === "") {
        return [];
    }

     const searchTerm = keyword.toLowerCase();

    //  return tickets.filter(ticket =>
    //      ticket.title.toLowerCase().includes(searchTerm) ||
    //     ticket.tags.some(tag =>
    //        tag.toLowerCase().includes(searchTerm)
    //      ) ||
    //     ticket.comments.some(comment =>
    //          comment.text.toLowerCase().includes(searchTerm)
    //      )
    //  );
    return tickets
    .Where(tickets => tickets.Contains(keyword, StringComparison)|| tickets.Tags.Any (tag => tag.Contains(keyword, StringComparison.OrdinalIgnoreCase)) ||
            t.Comments.Any(comment =>
                comment.Text.Contains(
                    keyword,
                    StringComparison.OrdinalIgnoreCase)))
        .ToList();
} ()
 }


    
    public List<Ticket> GetUnresolvedTickets()
    {
        var result = new List<Ticket>();
        foreach (var ticket in _tickets)
        {
            if (ticket.Status != "Closed")
            {
                result.Add(ticket);
            }
        }
        result.Sort((a, b) => a.CreatedDate.CompareTo(b.CreatedDate));
        return result;
    }

    //retrying pull 


S

public List<Ticket> GetSlaBreaches(DateTime asOf)
{
    return _tickets
        .Where(t =>
            (t.Status == "Open" || t.Status == "In Progress") &&
            SlaThresholds.ContainsKey(t.PriorityLevel) &&
            asOf - t.CreatedDate > SlaThresholds[t.PriorityLevel])
        .ToList();
}
   
    
    public List<Ticket> GetEscalatedTickets(DateTime asOf)
    {
        return new List<Ticket>();
    }





