namespace TicketFlow_v2.Services.Interface;

public interface ISmsService
{
    public Task SendSmsAsync(string recipientPhoneNumber, string message);
}