using TicketFlow_v2.Models;

namespace TicketFlow_v2.Repository.Interface;

public interface IUserRepository
{
    Task<User> CreateUserAsync(User request);
    Task<IEnumerable<User>> GetAllUsersAsync();
    Task<User?> GetUserAsync(Guid userId);
    Task<User?> GetUserByEmailAsync(string email);

    Task<IEnumerable<Ticket>> GetUserTicketsAsync(Guid userId);
}
