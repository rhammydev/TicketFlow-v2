using TicketFlow.Model;

namespace TicketFlow.DTOs.Response;

public class TicketResponse
{
    public Guid Id { get; set; }
    public Guid EventId { get; set; }
    public Guid OwnerId { get; set; }
    public TicketStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
}