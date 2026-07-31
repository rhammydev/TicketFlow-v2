using TicketFlow_v2.Services.Interface;

namespace TicketFlow_v2.Services.Implementation;

public class NullSmsService(ILogger<NullSmsService> logger) : ISmsService
{
    private readonly ILogger<NullSmsService> _logger = logger;

    public Task SendSmsAsync(string recipientPhoneNumber, string message)
    {
        _logger.LogInformation("SMS provider is not configured. Skipping SMS to {Recipient}.", recipientPhoneNumber);
        return Task.CompletedTask;
    }
}
