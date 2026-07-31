using TicketFlow_v2.Models;

namespace TicketFlow_v2.DTOs.Request;

public class CreateUserRequest
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public UserRole Role { get; set; }
}