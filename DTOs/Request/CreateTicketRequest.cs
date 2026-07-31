namespace TicketFlow_v2.DTOs.Request;

public class CreateTicketRequest
{
    public Guid EventId { get; set; }
    public Guid UserId { get; set; }
    public int Quantity { get; set; }
}