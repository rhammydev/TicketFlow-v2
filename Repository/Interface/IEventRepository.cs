using TicketFlow.DTOs.Request;
using TicketFlow.DTOs.Response;

namespace TicketFlow_v2.Repository.Interface;

public interface IEventInterface
{
    Task<ApiResponse<EventResponse>> CreateEventAsync(CreateEventRequest request);
    Task<ApiResponse<IEnumerable<EventResponse>>> GetAllEventsAsync();
    Task<ApiResponse<IEnumerable<EventResponse>>> GetAvailableEventsAsync();
    Task<ApiResponse<EventUsersResponse>> GetEventUsersAsync(Guid eventId);
    Task<ApiResponse<IEnumerable<BookingLogResponse>>> GetAuditLogAsync(Guid eventId);
}