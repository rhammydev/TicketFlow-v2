using TicketFlow_v2.DTOs.Request;
using TicketFlow_v2.DTOs.Response;

namespace TicketFlow_v2.Services.Interface;

public interface IUserService
{
    Task<ApiResponse<NewUserResponse>> CreateUserAsync(CreateUserRequest request);
    Task<ApiResponse<IEnumerable<UserResponse>>> GetAllUsersAsync();
    Task<ApiResponse<UserResponse>> GetUserAsync(Guid userId);
    Task<ApiResponse<UserResponse>> GetUserByEmailAsync(string email);
    
    Task<ApiResponse<UserTicketsResponse>> GetUserTicketsAsync(Guid userId);
}