using Microsoft.EntityFrameworkCore;
using TicketFlow_v2.Repository.Interface;
using TicketFlow_v2.Data;
using TicketFlow_v2.Models;

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

    public async Task<Event> UpdateEventAsync(Event request)
    {
        _ticketDbContext.Events.Update(request);
        await _ticketDbContext.SaveChangesAsync();
        return request;
    }

    public async Task<IEnumerable<Event>> GetAllEventsAsync()
    {
        return await _ticketDbContext.Events.ToListAsync();
    }

    public async Task<IEnumerable<Event>> GetAvailableEventsAsync()
    {
        return await _ticketDbContext.Events
            .Where(e => e.Date > DateTime.UtcNow && e.AvailableSeats > 0)
            .OrderBy(e => e.CreatedAt)
            .ToListAsync();
    }

    public async Task<Event?> GetEventUsersAsync(Guid eventId)
    {
        return await _ticketDbContext.Events.FirstOrDefaultAsync(e => e.Id == eventId);
    }

    public async Task<IEnumerable<User>> GetUsersByEventAsync(Guid eventId)
    {
        return await _ticketDbContext.Users
            .Where(u => _ticketDbContext.Tickets.Any(t =>
                t.EventId == eventId &&
                t.OwnerId == u.Id &&
                t.Status == TicketStatus.ACTIVE))
            .ToListAsync();
    }

    public async Task<IEnumerable<BookingLog>> GetAllAuditLogsAsync()
    {
        return await _ticketDbContext.BookingLogs
            .OrderBy(l => l.TimeStamp)
            .ToListAsync();
    }

    public async Task<IEnumerable<BookingLog>> GetAuditLogAsync(Guid eventId)
    {
        return await _ticketDbContext.BookingLogs
            .Where(l => l.EventId == eventId)
            .OrderBy(l => l.TimeStamp)
            .ToListAsync();
    }
}
