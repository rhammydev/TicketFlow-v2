using System.Text.Json.Serialization;

namespace TicketFlow.Model;

public enum UserRole
{
    ORGANIZER,
    ATTENDEE
}

public enum TicketStatus
{
    ACTIVE,
    CANCELLED
}

public enum ActionType
{
    PURCHASE,
    TRANSFER,
    CANCEL
}
