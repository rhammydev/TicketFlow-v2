using TicketFlow.DTOs.Request;
using TicketFlow.DTOs.Response;

namespace TicketFlow_v2.Repository.Interface;

public interface IUserInterface
{
    Task<ApiResponse<NewUserResponse>> CreateUserAsync(CreateUserRequest request);
    Task<ApiResponse<IEnumerable<UserResponse>>> GetAllUsersAsync();
    Task<ApiResponse<UserResponse>> GetUserAsync(Guid userId);
    Task<ApiResponse<UserTicketsResponse>> GetUserTicketsAsync(Guid userId);
}