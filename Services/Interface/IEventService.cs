using TicketFlow_v2.DTOs.Request;
using TicketFlow_v2.DTOs.Response;

namespace TicketFlow_v2.Services.Interface;

public interface IEventService
{
    Task<ApiResponse<EventResponse>> CreateEventAsync(CreateEventRequest request);
    Task<ApiResponse<IEnumerable<EventResponse>>> GetAllEventsAsync();
    Task<ApiResponse<IEnumerable<EventResponse>>> GetAvailableEventsAsync();
    Task<ApiResponse<IEnumerable<BookingLogResponse>>> GetAllAuditLogsAsync();
    Task<ApiResponse<IEnumerable<BookingLogResponse>>> GetAuditLogAsync(Guid eventId);
    Task<ApiResponse<EventUsersResponse>> GetEventUsersAsync(Guid eventId);
}
