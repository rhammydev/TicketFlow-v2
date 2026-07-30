namespace TicketFlow.Model;

public class Event
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public DateTime Date { get; set; }
    public int TotalCapacity { get; set; }
    public int AvailableSeats { get; set; }
    public decimal TicketPrice { get; set; }
    public DateTime CreatedAt { get; set; }
    public Guid OrganizerId { get; set; }
}