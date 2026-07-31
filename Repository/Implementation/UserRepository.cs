using Microsoft.EntityFrameworkCore;
using TicketFlow_v2.Repository.Interface;
using TicketFlow_v2.Data;
using TicketFlow_v2.Models;

namespace TicketFlow_v2.Repository.Implementation;

public class UserRepository(TicketDbContext dbContext) : IUserRepository
{
    private readonly TicketDbContext _dbContext = dbContext;
    
    public async Task<User> CreateUserAsync(User request)
    {
        _dbContext.Users.Add(request);
        await _dbContext.SaveChangesAsync();
        return request;
    }

    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        return await _dbContext.Users.ToListAsync();
    }

    public async Task<User?> GetUserAsync(Guid userId)
    {
        return await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == userId);
    }
    
    public async Task<User?> GetUserByEmailAsync(string email)
    {
        return await _dbContext.Users.FirstOrDefaultAsync(x => x.Email.ToLower() == email.ToLower());
    }

    public async Task<IEnumerable<Ticket>> GetUserTicketsAsync(Guid userId)
    {
        return await _dbContext.Tickets
            .Where(t => t.OwnerId == userId && t.Status == TicketStatus.ACTIVE)
            .ToListAsync();
    }
}
