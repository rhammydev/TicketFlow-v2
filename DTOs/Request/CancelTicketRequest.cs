namespace TicketFlow_v2.DTOs.Request;

public class CancelTicketRequest
{
    public Guid TicketId { get; set; }
    public Guid UserId  { get; set; }
}