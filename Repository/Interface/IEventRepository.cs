using TicketFlow.Model;

namespace TicketFlow_v2.Repository.Interface;

public interface IEventRepository
{
    Task<Event> CreateEventAsync(Event request);
    Task<IEnumerable<Event>> GetAllEventsAsync();
    Task<IEnumerable<Event>> GetAvailableEventsAsync();
    Task<Event?> GetEventUsersAsync(Guid eventId);
    Task<IEnumerable<BookingLog?>> GetAuditLogAsync(Guid eventId);
}