namespace TicketFlow.Model;

public class BookingLog
{
    public Guid Id { get; set; }
    public Guid TicketId { get; set; }
    public Guid EventId { get; set; }
    public Guid UserId { get; set; }
    public ActionType Action { get; set; }
    public DateTime TimeStamp { get; set; }
    public string? Note { get; set; }
}