using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using TicketFlow_v2.Models;
using TicketFlow_v2.Services.Interface;
using TicketFlow_v2.Utilities;

namespace TicketFlow_v2.Services.Implementation;

public class EmailService(IOptions<SmtpMail> smtpMail, ILogger<EmailService> logger) : IEmailService
{
    private readonly SmtpMail _smtpMail = smtpMail.Value;
    private readonly ILogger<EmailService> _logger = logger;

    public async Task SendWelcomeEmailAsync(string toEmail, string fullName, string role)
    {
        var message = CreateBaseMessage(toEmail, fullName, $"Welcome to Event Bridge, {fullName}!");
        message.Body = new TextPart(TextFormat.Html)
        {
            Text = MailUtils.GetWelcomeEmailHtml(fullName, toEmail, role)
        };

        await SendMimeMessageAsync(message);
    }

    public async Task SendBookingConfirmationAsync(
        string toEmail,
        string attendeeName,
        string eventName,
        DateTime eventDate,
        int quantity,
        decimal ticketPrice)
    {
        var message = CreateBaseMessage(toEmail, attendeeName, $"Your tickets are confirmed - {eventName}");
        message.Body = new TextPart(TextFormat.Html)
        {
            Text = MailUtils.GetBookingConfirmationHtml(attendeeName, eventName, eventDate, quantity, ticketPrice)
        };

        await SendMimeMessageAsync(message);
    }

    public async Task SendTicketTransferNotificationAsync(
        string toEmail,
        string recipientName,
        string senderName,
        string eventName,
        DateTime eventDate,
        Guid ticketId)
    {
        var message = CreateBaseMessage(toEmail, recipientName, $"{senderName} sent you a ticket to {eventName}");
        message.Body = new TextPart(TextFormat.Html)
        {
            Text = MailUtils.GetTicketTransferHtml(recipientName, senderName, eventName, eventDate, ticketId)
        };

        await SendMimeMessageAsync(message);
    }

    public async Task SendTicketCancellationNotificationAsync(
        string toEmail,
        string fullName,
        string eventName,
        DateTime eventDate,
        Guid ticketId)
    {
        var message = CreateBaseMessage(toEmail, fullName, $"Your ticket was cancelled - {eventName}");
        message.Body = new TextPart(TextFormat.Html)
        {
            Text = MailUtils.GetTicketCancellationHtml(fullName, eventName, eventDate, ticketId)
        };

        await SendMimeMessageAsync(message);
    }

    private MimeMessage CreateBaseMessage(string toEmail, string toName, string subject)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(_smtpMail.SenderName, _smtpMail.SenderEmail));
        message.To.Add(new MailboxAddress(toName, toEmail));
        message.Subject = subject;
        return message;
    }

    private async Task SendMimeMessageAsync(MimeMessage message)
    {
        if (string.IsNullOrWhiteSpace(_smtpMail.Server) ||
            string.IsNullOrWhiteSpace(_smtpMail.Username) ||
            string.IsNullOrWhiteSpace(_smtpMail.Password))
        {
            _logger.LogInformation("SMTP settings are incomplete. Skipping email to {Recipient}.", message.To);
            return;
        }

        using var client = new SmtpClient();
        try
        {
            await client.ConnectAsync(_smtpMail.Server, _smtpMail.Port, false);
            await client.AuthenticateAsync(_smtpMail.Username, _smtpMail.Password);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send email to {Recipient}.", message.To);
        }
    }
}
