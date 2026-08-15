namespace TicketDashboard;

public class Comment
{
    public string Author { get; set; } = string.Empty;
    public string Text { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
}
//jjjjjj
public class Ticket
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;        // "Open", "In Progress", "Closed"
    public string PriorityLevel { get; set; } = string.Empty; // "Low", "Medium", "High", "Critical"
    public DateTime CreatedDate { get; set; }
    public DateTime? ClosedDate { get; set; }                 // set only when Status == "Closed"
    public string AssignedTo { get; set; }  = string.Empty;                 // null means unassigned
    public List<string> Tags { get; set; } = new();
    public List<Comment> Comments { get; set; } = new();
// "Low", "Medium", "High", "Critical"
    
}
