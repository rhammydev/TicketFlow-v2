using TicketFlow_v2.Repository.Interface;
using TicketFlow.Data;
using TicketFlow.Model;

namespace TicketFlow_v2.Repository.Implementation;

public class TicketRepository(TicketDbContext dbContext) : ITicketRepository
{
    private readonly TicketDbContext _ticketDbContext = dbContext;
    
    public async Task<IEnumerable<Ticket>> CreateTicketAsync(Ticket request)
    {
        _ticketDbContext.Tickets.Add(request);
        await _ticketDbContext.SaveChangesAsync();
        return [request];
    }

    public async Task<Ticket> TransferTicketAsync(Ticket request)
    {
        _ticketDbContext.Tickets.Update(request);
        await _ticketDbContext.SaveChangesAsync();
        return request;
    }

    public async Task<Ticket> CancelTicketAsync(Ticket request)
    {
        _ticketDbContext.Tickets.Update(request);
        await _ticketDbContext.SaveChangesAsync();
        return request;
    }
}