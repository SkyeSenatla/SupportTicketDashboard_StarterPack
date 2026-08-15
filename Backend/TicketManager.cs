public List<Ticket> GetAllTickets()
{
    
    return new List<Ticket>(_tickets);
}

public List<Ticket> GetHighPriorityTickets()
{
    return _tickets
        .Where(t => t.PriorityLevel == "Critical" || t.PriorityLevel == "High").ToList();
}

public Dictionary<string, int> GetTicketCountsByStatus()
{
    return _tickets
        .GroupBy(t => t.Status)
        .ToDictionary(g => g.Key, g => g.Count());
}

public List<Ticket> SortTicketsByDate()
{
    var sorted = new List<Ticket>(_tickets);

    
    sorted.Sort((a, b) => b.CreatedDate.CompareTo(a.CreatedDate));

    return sorted;
}

public List<Ticket> SortTicketsByPriorityThenDate()
{
    var sorted = new List<Ticket>(_tickets);

    sorted.Sort((a, b) =>
    {
        int priorityComparison =
            PriorityRank[a.PriorityLevel].CompareTo(PriorityRank[b.PriorityLevel]);

        if (priorityComparison != 0)
        {
            return priorityComparison;
        }

        
        return b.CreatedDate.CompareTo(a.CreatedDate);
    });

    return sorted;
}

public double GetAverageResolutionDays()
{
    var closedTickets = _tickets
        .Where(t => t.ClosedDate.HasValue)
        .ToList();

    if (!closedTickets.Any())
    {
        return 0;
    }

    return closedTickets
        .Average(t => (t.ClosedDate!.Value - t.CreatedDate).TotalDays);
}

public List<Ticket> GetTicketsByAssignee(string? assignee)
{
    
    if (string.IsNullOrWhiteSpace(assignee))
    {
        return _tickets
            .Where(t => t.AssignedTo == null)
            .ToList();
    }

    return _tickets
        .Where(t => t.AssignedTo != null && t.AssignedTo.Equals(assignee,StringComparison.OrdinalIgnoreCase))
        .ToList();
}

public List<Ticket> SearchTickets(string keyword)
{
    if (string.IsNullOrWhiteSpace(keyword))
    {
        return new List<Ticket>();
    }

    return _tickets
        .Where(t =>
            t.Title.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||

            t.Tags.Any(tag =>tag.Contains(keyword, StringComparison.OrdinalIgnoreCase)) ||

            t.Comments.Any(comment =>comment.Text.Contains( keyword,StringComparison.OrdinalIgnoreCase)))
        .ToList();
}

public List<Ticket> GetUnresolvedTickets()
{
    var result = new List<Ticket>();

    foreach (var ticket in _tickets)
    {
       
        if (ticket.Status == "Open" || ticket.Status == "In Progress")
        {
            result.Add(ticket);
        }
    }

    result.Sort((a, b) => a.CreatedDate.CompareTo(b.CreatedDate));

    return result;
}

public List<Ticket> GetSlaBreaches(DateTime asOf)
{
    return _tickets
        .Where(t =>
            (t.Status == "Open" || t.Status == "In Progress") && SlaThresholds.ContainsKey(t.PriorityLevel) &&
            (asOf - t.CreatedDate) > SlaThresholds[t.PriorityLevel])
        .ToList();
}

public List<Ticket> GetEscalatedTickets(DateTime asOf)
{
    var breaches = GetSlaBreaches(asOf);
    var result = new List<Ticket>();

    foreach (var ticket in breaches)
    {
        int currentIndex = Array.IndexOf(
            PriorityOrder,
            ticket.PriorityLevel);

        
        int newIndex = Math.Max(0, currentIndex - 1);

        var escalatedTicket = new Ticket
        {
            Id = ticket.Id,
            Title = ticket.Title,
            Status = ticket.Status,
            PriorityLevel = PriorityOrder[newIndex],
            CreatedDate = ticket.CreatedDate,
            ClosedDate = ticket.ClosedDate,
            AssignedTo = ticket.AssignedTo,

          
            Tags = new List<string>(ticket.Tags),

            Comments = ticket.Comments
                .Select(c => new Comment
                {
                    Author = c.Author,
                    Text = c.Text,
                    Timestamp = c.Timestamp
                }).ToList()
        };

        result.Add(escalatedTicket);
    }

    return result;
}