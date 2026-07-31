using TicketFlow_v2.Models;

namespace TicketFlow_v2.Repository.Interface;

public interface IEventRepository
{
    Task<Event> CreateEventAsync(Event request);
    Task<Event> UpdateEventAsync(Event request);
    Task<IEnumerable<Event>> GetAllEventsAsync();
    Task<IEnumerable<Event>> GetAvailableEventsAsync();
    Task<Event?> GetEventUsersAsync(Guid eventId);
    Task<IEnumerable<User>> GetUsersByEventAsync(Guid eventId);
    Task<IEnumerable<BookingLog>> GetAllAuditLogsAsync();
    Task<IEnumerable<BookingLog>> GetAuditLogAsync(Guid eventId);
}
