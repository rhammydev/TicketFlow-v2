using Microsoft.EntityFrameworkCore;
using TicketFlow_v2.Repository.Interface;
using TicketFlow.Data;
using TicketFlow.DTOs.Request;
using TicketFlow.DTOs.Response;
using TicketFlow.Model;

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

    public async Task<User?> GetUserTicketsAsync(Guid userId)
    {
        return await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == userId);
    }
}