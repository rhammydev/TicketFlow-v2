namespace TicketFlow_v2.Services.Interface;

public interface IEmailService
{
    Task SendWelcomeEmailAsync(string toEmail, string fullName, string role);

    Task SendBookingConfirmationAsync(
        string toEmail,
        string attendeeName,
        string eventName,
        DateTime eventDate,
        int quantity,
        decimal ticketPrice);
    
    Task SendTicketTransferNotificationAsync(
        string toEmail,
        string recipientName,
        string senderName,
        string eventName,
        DateTime eventDate,
        Guid ticketId);

    Task SendTicketCancellationNotificationAsync(
        string toEmail,
        string fullName,
        string eventName,
        DateTime eventDate,
        Guid ticketId);
}
