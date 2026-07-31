using TicketFlow_v2.Models;

namespace TicketFlow_v2.DTOs.Response;

public class UserTicketsResponse
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public UserRole Role { get; set; }
    public IEnumerable<TicketResponse> Tickets { get; set; }
}