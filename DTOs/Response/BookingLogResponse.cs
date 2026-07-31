using TicketFlow_v2.Models;

namespace TicketFlow_v2.DTOs.Response;

public class BookingLogResponse
{
    public Guid Id { get; set; }
    public Guid TicketId { get; set; }
    public Guid EventId { get; set; }
    public Guid UserId { get; set; }
    public ActionType Action { get; set; }
    public string? Note { get; set; }
    public DateTime Timestamp { get; set; }
}
