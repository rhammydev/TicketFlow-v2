using System.Text.Json.Serialization;

namespace TicketFlow_v2.Models;

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
