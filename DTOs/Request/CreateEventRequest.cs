namespace TicketFlow.DTOs.Request;

public class CreateEventRequest
{
    public Guid OrganizerId { get; set; }
    public string Name { get; set; }
    public DateTime Date { get; set; }
    public int TotalCapacity { get; set; }
    public decimal TicketPrice { get; set; }
}