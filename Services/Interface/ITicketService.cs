using TicketFlow_v2.DTOs.Request;
using TicketFlow_v2.DTOs.Response;

namespace TicketFlow_v2.Services.Interface;

public interface ITicketService
{
    Task<ApiResponse<IEnumerable<TicketResponse>>> CreateTicketAsync(CreateTicketRequest request);
    Task<ApiResponse<TicketResponse>> TransferTicketAsync(TransferTicketRequest request);
    Task<ApiResponse<TicketResponse>> CancelTicketAsync(CancelTicketRequest request);
}