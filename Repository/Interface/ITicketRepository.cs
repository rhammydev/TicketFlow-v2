
using TicketFlow.Model;

namespace TicketFlow_v2.Repository.Interface;

public interface ITicketRepository
{
    Task<IEnumerable<Ticket>> CreateTicketAsync(Ticket request);
    Task<Ticket> TransferTicketAsync(Ticket request);
    Task<Ticket> CancelTicketAsync(Ticket request);
}