namespace TicketDashboard;

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
    private static readonly string[] PriorityOrder =
    {
        "Critical",
        "High",
        "Medium",
        "Low"
    };

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
            Id = 1,
            Title = "Email server down",
            Status = "Open",
            PriorityLevel = "Critical",
            CreatedDate = new DateTime(2026, 8, 10),
            AssignedTo = "Maya Patel",
            Tags = new() { "email", "outage" },
            Comments = new()
            {
                new Comment
                {
                    Author = "Maya Patel",
                    Text = "Escalated to infra team.",
                    Timestamp = new DateTime(2026, 8, 10)
                }
            },
        },

        new Ticket
        {
            Id = 2,
            Title = "Printer offline on 3rd floor",
            Status = "Open",
            PriorityLevel = "Low",
            CreatedDate = new DateTime(2026, 8, 5),
            AssignedTo = null,
            Tags = new() { "hardware", "printer" },
            Comments = new(),
        },

        new Ticket
        {
            Id = 3,
            Title = "VPN keeps disconnecting",
            Status = "In Progress",
            PriorityLevel = "High",
            CreatedDate = new DateTime(2026, 8, 9),
            AssignedTo = "Jordan Lee",
            Tags = new() { "network", "vpn" },
            Comments = new()
            {
                new Comment
                {
                    Author = "Jordan Lee",
                    Text = "Reproduced on Windows clients only.",
                    Timestamp = new DateTime(2026, 8, 9)
                }
            },
        },

        new Ticket
        {
            Id = 4,
            Title = "New hire laptop setup",
            Status = "Closed",
            PriorityLevel = "Medium",
            CreatedDate = new DateTime(2026, 7, 28),
            ClosedDate = new DateTime(2026, 7, 30),
            AssignedTo = "Maya Patel",
            Tags = new() { "onboarding", "hardware" },
            Comments = new()
            {
                new Comment
                {
                    Author = "Maya Patel",
                    Text = "Laptop imaged and delivered.",
                    Timestamp = new DateTime(2026, 7, 30)
                }
            },
        },

        new Ticket
        {
            Id = 5,
            Title = "Database replication lag",
            Status = "Open",
            PriorityLevel = "Critical",
            CreatedDate = new DateTime(2026, 8, 12),
            AssignedTo = "Jordan Lee",
            Tags = new() { "database", "performance" },
            Comments = new(),
        },

        new Ticket
        {
            Id = 6,
            Title = "Password reset request",
            Status = "Closed",
            PriorityLevel = "Low",
            CreatedDate = new DateTime(2026, 7, 30),
            ClosedDate = new DateTime(2026, 7, 30),
            AssignedTo = null,
            Tags = new() { "account" },
            Comments = new()
            {
                new Comment
                {
                    Author = "Helpdesk Bot",
                    Text = "Auto-resolved via self-service portal.",
                    Timestamp = new DateTime(2026, 7, 30)
                }
            },
        },

        new Ticket
        {
            Id = 7,
            Title = "Office Wi-Fi intermittent",
            Status = "In Progress",
            PriorityLevel = "Medium",
            CreatedDate = new DateTime(2026, 8, 6),
            AssignedTo = null,
            Tags = new() { "network", "wifi" },
            Comments = new()
            {
                new Comment
                {
                    Author = "Sam Osei",
                    Text = "Checking access point firmware.",
                    Timestamp = new DateTime(2026, 8, 7)
                }
            },
        },

        new Ticket
        {
            Id = 8,
            Title = "Payroll app throwing 500s",
            Status = "Open",
            PriorityLevel = "High",
            CreatedDate = new DateTime(2026, 8, 11),
            AssignedTo = "Sam Osei",
            Tags = new() { "payroll", "bug" },
            Comments = new()
            {
                new Comment
                {
                    Author = "Sam Osei",
                    Text = "Stack trace points to a null reference in the payroll service.",
                    Timestamp = new DateTime(2026, 8, 11)
                }
            },
        },
    };

    // Returns all tickets.
    public List<Ticket> GetAllTickets()
    {
        return new List<Ticket>(_tickets);
    }

    // Returns only tickets whose PriorityLevel is "Critical" or "High".
    public List<Ticket> GetHighPriorityTickets()
    {
        var result = new List<Ticket>();

        foreach (var ticket in _tickets)
        {
            if (ticket.PriorityLevel == "Critical" ||
                ticket.PriorityLevel == "High")
            {
                result.Add(ticket);
            }
        }

        return result;
    }

    // Counts tickets per status using LINQ.
    public Dictionary<string, int> GetTicketCountsByStatus()
    {
        return _tickets
            .GroupBy(ticket => ticket.Status)
            .ToDictionary(
                group => group.Key,
                group => group.Count());
    }

    // Returns tickets sorted newest to oldest by CreatedDate.
    public List<Ticket> SortTicketsByDate()
    {
        var sorted = new List<Ticket>(_tickets);

        sorted.Sort((a, b) =>
            b.CreatedDate.CompareTo(a.CreatedDate));

        return sorted;
    }

    // Returns tickets ordered by priority, then newest date.
    public List<Ticket> SortTicketsByPriorityThenDate()
    {
        var sorted = new List<Ticket>(_tickets);

        sorted.Sort((a, b) =>
        {
            int priorityComparison =
                PriorityRank[a.PriorityLevel]
                .CompareTo(PriorityRank[b.PriorityLevel]);

            if (priorityComparison != 0)
            {
                return priorityComparison;
            }

            return b.CreatedDate.CompareTo(a.CreatedDate);
        });

        return sorted;
    }

    // Returns average resolution time in days.
    public double GetAverageResolutionDays()
    {
        var closedTickets = _tickets
            .Where(ticket => ticket.ClosedDate.HasValue)
            .ToList();

        if (closedTickets.Count == 0)
        {
            return 0;
        }

        return closedTickets
            .Average(ticket =>
                (ticket.ClosedDate!.Value - ticket.CreatedDate).TotalDays);
    }

    // Returns tickets assigned to the given person.
    // null or empty string returns unassigned tickets.
    public List<Ticket> GetTicketsByAssignee(string? assignee)
    {
        if (string.IsNullOrWhiteSpace(assignee))
        {
            return _tickets
                .Where(ticket => ticket.AssignedTo == null)
                .ToList();
        }

        return _tickets
            .Where(ticket =>
                ticket.AssignedTo != null &&
                string.Equals(
                    ticket.AssignedTo,
                    assignee,
                    StringComparison.OrdinalIgnoreCase))
            .ToList();
    }

    // Searches title, tags and comments.
    public List<Ticket> SearchTickets(string keyword)
    {
        if (string.IsNullOrWhiteSpace(keyword))
        {
            return new List<Ticket>();
        }

        return _tickets
            .Where(ticket =>
                ticket.Title.Contains(
                    keyword,
                    StringComparison.OrdinalIgnoreCase)

                || ticket.Tags.Any(tag =>
                    tag.Contains(
                        keyword,
                        StringComparison.OrdinalIgnoreCase))

                || ticket.Comments.Any(comment =>
                    comment.Text.Contains(
                        keyword,
                        StringComparison.OrdinalIgnoreCase)))
            .ToList();
    }

    // Returns unresolved tickets, oldest first.
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

        result.Sort((a, b) =>
            a.CreatedDate.CompareTo(b.CreatedDate));

        return result;
    }

    // Returns unresolved tickets that have exceeded their SLA threshold.
    public List<Ticket> GetSlaBreaches(DateTime asOf)
    {
        return _tickets
            .Where(ticket =>
                ticket.Status != "Closed" &&
                SlaThresholds.ContainsKey(ticket.PriorityLevel) &&
                asOf - ticket.CreatedDate >
                    SlaThresholds[ticket.PriorityLevel])
            .ToList();
    }

    // Returns NEW ticket copies with priority escalated one level.
    public List<Ticket> GetEscalatedTickets(DateTime asOf)
    {
        var breachedTickets = GetSlaBreaches(asOf);
        var result = new List<Ticket>();

        foreach (var ticket in breachedTickets)
        {
            int currentIndex =
                Array.IndexOf(PriorityOrder, ticket.PriorityLevel);

            string newPriority = ticket.PriorityLevel;

            if (currentIndex > 0)
            {
                newPriority = PriorityOrder[currentIndex - 1];
            }

            var escalatedTicket = new Ticket
            {
                Id = ticket.Id,
                Title = ticket.Title,
                Status = ticket.Status,
                PriorityLevel = newPriority,
                CreatedDate = ticket.CreatedDate,
                ClosedDate = ticket.ClosedDate,
                AssignedTo = ticket.AssignedTo,
                Tags = new List<string>(ticket.Tags),

                Comments = ticket.Comments
                    .Select(comment => new Comment
                    {
                        Author = comment.Author,
                        Text = comment.Text,
                        Timestamp = comment.Timestamp
                    })
                    .ToList()
            };

            result.Add(escalatedTicket);
        }

        return result;
    }
}

