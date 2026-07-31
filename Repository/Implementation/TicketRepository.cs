using TicketFlow_v2.Repository.Interface;
using TicketFlow_v2.Data;
using TicketFlow_v2.Models;
using Microsoft.EntityFrameworkCore;

namespace TicketFlow_v2.Repository.Implementation;

public class TicketRepository(TicketDbContext dbContext) : ITicketRepository
{
    private readonly TicketDbContext _ticketDbContext = dbContext;
    
    public async Task<IEnumerable<Ticket>> CreateTicketsAsync(IEnumerable<Ticket> tickets, IEnumerable<BookingLog> bookingLogs, Event eventDetails)
    {
        var ticketList = tickets.ToList();
        _ticketDbContext.Events.Update(eventDetails);
        _ticketDbContext.Tickets.AddRange(ticketList);
        _ticketDbContext.BookingLogs.AddRange(bookingLogs);
        await _ticketDbContext.SaveChangesAsync();
        return ticketList;
    }

    public async Task<Ticket?> GetTicketAsync(Guid ticketId)
    {
        return await _ticketDbContext.Tickets.FirstOrDefaultAsync(t => t.Id == ticketId);
    }

    public async Task<Ticket> UpdateTicketAsync(Ticket request)
    {
        _ticketDbContext.Tickets.Update(request);
        await _ticketDbContext.SaveChangesAsync();
        return request;
    }

    public async Task<Ticket> UpdateTicketWithLogAsync(Ticket ticket, BookingLog bookingLog)
    {
        _ticketDbContext.Tickets.Update(ticket);
        _ticketDbContext.BookingLogs.Add(bookingLog);
        await _ticketDbContext.SaveChangesAsync();
        return ticket;
    }

    public async Task<Ticket> UpdateTicketEventAndLogAsync(Ticket ticket, Event eventDetails, BookingLog bookingLog)
    {
        _ticketDbContext.Tickets.Update(ticket);
        _ticketDbContext.Events.Update(eventDetails);
        _ticketDbContext.BookingLogs.Add(bookingLog);
        await _ticketDbContext.SaveChangesAsync();
        return ticket;
    }

    public async Task AddBookingLogAsync(BookingLog bookingLog)
    {
        _ticketDbContext.BookingLogs.Add(bookingLog);
        await _ticketDbContext.SaveChangesAsync();
    }
}
