using TicketFlow.Model;

namespace TicketFlow_v2.Repository.Interface;

public interface IUserRepository
{
    Task<User> CreateUserAsync(User request);
    Task<IEnumerable<User>> GetAllUsersAsync();
    Task<User?> GetUserAsync(Guid userId);
    Task<User?> GetUserTicketsAsync(Guid userId);
}