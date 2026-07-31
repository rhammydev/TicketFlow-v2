
using TicketFlow_v2.Models;

namespace TicketFlow_v2.Repository.Interface;

public interface ITicketRepository
{
    Task<IEnumerable<Ticket>> CreateTicketsAsync(IEnumerable<Ticket> tickets, IEnumerable<BookingLog> bookingLogs, Event eventDetails);
    Task<Ticket?> GetTicketAsync(Guid ticketId);
    Task<Ticket> UpdateTicketAsync(Ticket request);
    Task<Ticket> UpdateTicketWithLogAsync(Ticket ticket, BookingLog bookingLog);
    Task<Ticket> UpdateTicketEventAndLogAsync(Ticket ticket, Event eventDetails, BookingLog bookingLog);
    Task AddBookingLogAsync(BookingLog bookingLog);
}
