namespace TicketFlow.DTOs.Request;

public class TransferTicketRequest
{
    public Guid TicketId { get; set; }
    public Guid FromUserId  { get; set; }
    public string ToUserEmail { get; set; }
}