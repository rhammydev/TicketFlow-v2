namespace TicketFlow_v2.Utilities;

public static class SmsUtils
{
    public static string GetBookingConfirmationSms(
        string attendeeName,
        string eventName,
        DateTime eventDate,
        int quantity)
    {
        return $"Hi {attendeeName}! Your booking is confirmed. " +
               $"Event: {eventName} | Date: {eventDate:dd MMM yyyy, h:mm tt} | " +
               $"Tickets: {quantity}. Show this SMS at the gate. Enjoy the event! – Event Bridge";
    }

    public static string GetTicketTransferSms(
        string recipientName,
        string senderName,
        string eventName,
        DateTime eventDate,
        Guid ticketId)
    {
        return $"Hi {recipientName}! {senderName} has transferred a ticket to you. " +
               $"Event: {eventName} | Date: {eventDate:dd MMM yyyy, h:mm tt} | " +
               $"Ticket ID: {ticketId.ToString()[..8].ToUpper()}. Check your email for full details. – Event Bridge";
    }

    public static string GetTicketCancellationSms(
        string fullName,
        string eventName,
        DateTime eventDate,
        Guid ticketId)
    {
        return $"Hi {fullName}, your ticket has been cancelled. " +
               $"Event: {eventName} | Date: {eventDate:dd MMM yyyy, h:mm tt} | " +
               $"Ticket ID: {ticketId.ToString()[..8].ToUpper()}. Your seat has been released. – Event Bridge";
    }

    public static string GetWelcomeSms(string fullName, string role)
    {
        return $"Welcome to Event Bridge, {fullName}! Your account is ready. " +
               $"You're registered as a {role}. " +
               (role.Equals("Organizer", StringComparison.OrdinalIgnoreCase)
                   ? "Start creating events on the platform."
                   : "Start exploring and booking events today.") +
               " – Event Bridge";
    }
}
