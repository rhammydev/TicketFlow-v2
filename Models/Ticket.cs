namespace TicketFlow.Model;

public class Ticket
{
    public Guid Id { get; set; }
    public Guid EventId { get; set; }
    public Guid OwnerId { get; set; }
    public TicketStatus Status { get; set; }
    public int Quantity { get; set; }
    public DateTime CreatedAt { get; set; }
}