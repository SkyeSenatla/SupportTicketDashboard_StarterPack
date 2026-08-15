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
    private List<Ticket> GetAllTickets()
    {   
         
        return _tickets;
    }

    // Returns only tickets whose PriorityLevel is "Critical" or "High".
    public List<Ticket> GetHighPriorityTickets()
    {
        var result = new List<Ticket>();
        // TODO: Loop through _tickets and add any ticket with PriorityLevel
        // "Critical" or "High" to result.
        if(PriorityLevel  = "Crital" || "High")
        {

             return result;
        }
       
    }

    // Counts tickets per status into a Dictionary<string, int>.
    // TODO: Implement this using LINQ (GroupBy + ToDictionary), not a manual
    // foreach loop.
    public Dictionary<string, int> GetTicketCountsByStatus()
    {
        var counts = new Dictionary<string, int>();
        return counts;
        GetAllTickets.
        GetTicketCountsByStatus => counts.s {
            Open ;
            Medium;
            Low;
        }
    }

    // Returns tickets sorted newest to oldest by CreatedDate.
    public List<Ticket> SortTicketsByDate()
    {
        var sorted = new List<Ticket>(_tickets);
        // TODO: Sort `sorted` by CreatedDate, newest first.
        if(sorted  > CreatedDate)
        {
             return sorted;
        }
       
    }

    // Returns tickets ordered by urgency first (Critical, then High, then
    // Medium, then Low), and within the same priority, newest CreatedDate
    // first. Use the PriorityRank map above - sorting PriorityLevel as a
    // plain string will NOT give you the right order.
    public List<Ticket> SortTicketsByPriorityThenDate()
    {
        var sorted = new List<Ticket>(_tickets);
        // TODO: Sort `sorted` by PriorityRank[t.PriorityLevel] ascending,
        // then by CreatedDate descending within the same priority.
        
         
        result.Sort((PriorityOrder) => a.CreatedDate.CompareTo(b.CreatedDate));
        if(sorted == "Crital")
        {
            return sorted;
        }
        else if (sorted == "High")
        {
            return sorted;
        }
        else if(sorted == "Medium")
        {
            return sorted;
        }
        else if(sorted == "Low")
        (
           return sorted;
        )
        else{
            CreatedDate 
        }
        return result; 
}
      
    }

    // Returns the average number of days between CreatedDate and ClosedDate
    // for tickets that have been closed. Tickets without a ClosedDate must
    // be excluded. Return 0 if there are no closed tickets (don't divide by
    // zero!).
    public double GetAverageResolutionDays()
    {
        // TODO: Implement using LINQ.
        return 0;
    }

    // Returns tickets assigned to the given person (case-insensitive match
    // on AssignedTo). Passing null or an empty string should return the
    // tickets that are currently unassigned.
    public List<Ticket> GetTicketsByAssignee(string? assignee)
    {
        // TODO: Implement. Remember AssignedTo can itself be null - don't
        // let a null AssignedTo blow up your comparison.
        assignee = AssignedTo
        return new List<Ticket>();
    }

    // Returns tickets where the keyword (case-insensitive) appears in the
    // Title, in any of the ticket's Tags, or in the text of any Comment.
    public List<Ticket> SearchTickets(string keyword)
    {
        // TODO: Implement using LINQ. You'll need Any() to look inside the
        // Tags and Comments collections on each ticket.

        SearchTickets=> s.keyword = Title,Tags , Comments         
        return new List<Ticket>();
    }

    // Returns every ticket that has NOT been closed yet (Status is "Open" or
    // "In Progress"), sorted oldest-created first so the longest-waiting
    // tickets surface at the top.
    //
    // NOTE: This method is already fully written - but it has a bug. Run the
    // project, compare the output against the description above, and find
    // and fix the mistake. Do not rewrite the method from scratch; there is
    // one small thing wrong with it.
    public List<Ticket> GetUnresolvedTickets()
    {
        var result = new List<Ticket>();
        foreach (var ticket in _tickets)
        {
            if (ticket.Status != "closed")
            {
                result.Add(ticket);
            }
        }
        result.Sort((a, b) => a.CreatedDate.CompareTo(b.CreatedDate));
        return result;
    }

    // Returns unresolved tickets (Status is "Open" or "In Progress") whose
    // age (asOf - CreatedDate) exceeds their priority's SLA threshold (see
    // SlaThresholds above). Closed tickets never breach, no matter how old.
    // TODO: Implement.
    public List<Ticket> GetSlaBreaches(DateTime asOf)
    {
        return new List<Ticket>();
    }

    // Returns a NEW list of ticket copies for every SLA-breaching ticket
    // (see GetSlaBreaches), with each copy's PriorityLevel bumped one step
    // more urgent using PriorityOrder above (Critical stays Critical). The
    // original tickets must be unaffected - do not mutate _tickets, and
    // don't just hand back the same Ticket objects with PriorityLevel
    // changed in place.
    // TODO: Implement.
    public List<Ticket> GetEscalatedTickets(DateTime asOf)
    {
        return new List<Ticket>();
    }
}
