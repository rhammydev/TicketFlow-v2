using Microsoft.EntityFrameworkCore;
using TicketFlow_v2.Repository.Interface;
using TicketFlow.Data;
using TicketFlow.Model;

namespace TicketFlow_v2.Repository.Implementation;

public class EventRepository(TicketDbContext dbContext) : IEventRepository
{
    private readonly TicketDbContext _ticketDbContext = dbContext;
    
    public async Task<Event> CreateEventAsync(Event request)
    { 
        _ticketDbContext.Events.Add(request);
        await _ticketDbContext.SaveChangesAsync();
        return request;
    }

    public async Task<IEnumerable<Event>> GetAllEventsAsync()
    {
        return await _ticketDbContext.Events.ToListAsync();
    }

    public async Task<IEnumerable<Event>> GetAvailableEventsAsync()
    {
        return await _ticketDbContext.Events.ToListAsync();
    }

    public async Task<Event?> GetEventUsersAsync(Guid eventId)
    {
        return await _ticketDbContext.Events.FirstOrDefaultAsync(e => e.Id == eventId);
    }

    public async Task<IEnumerable<BookingLog?>> GetAuditLogAsync(Guid eventId)
    {
        return await _ticketDbContext.BookingLogs.Where(l => l.EventId == eventId).ToListAsync();
    }
}