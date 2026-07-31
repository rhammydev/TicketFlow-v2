namespace TicketFlow_v2.Utilities;

public static class MailUtils
{
    // ─── Shared base layout ──────────────────────────────────────────────────

    private static string GetBaseTemplate(string title, string content)
    {
        return $@"<!DOCTYPE html>
<html lang='en'>
<head>
    <meta charset='utf-8' />
    <meta name='viewport' content='width=device-width, initial-scale=1.0'/>
    <title>{title}</title>
    <style>
        * {{ box-sizing: border-box; margin: 0; padding: 0; }}

        body {{
            font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Helvetica, Arial, sans-serif;
            background-color: #F0F2F5;
            color: #1A1A2E;
            -webkit-font-smoothing: antialiased;
        }}

        .wrapper {{
            width: 100%;
            padding: 48px 16px;
            background-color: #F0F2F5;
        }}

        .container {{
            max-width: 560px;
            margin: 0 auto;
            background: #FFFFFF;
            border-radius: 20px;
            overflow: hidden;
            box-shadow: 0 4px 24px rgba(0, 0, 0, 0.08);
        }}

        /* ── Header ── */
        .header {{
            background: linear-gradient(135deg, #FF6B35 0%, #F7931E 100%);
            padding: 36px 40px 28px;
            text-align: center;
        }}
        .header-logo {{
            font-size: 28px;
            font-weight: 800;
            color: #FFFFFF;
            letter-spacing: -0.5px;
            margin-bottom: 4px;
        }}
        .header-logo span {{
            font-size: 20px;
            margin-right: 6px;
        }}
        .header-tagline {{
            font-size: 13px;
            color: rgba(255, 255, 255, 0.80);
            font-weight: 400;
        }}

        /* ── Ticket perforation divider ── */
        .perforation {{
            display: flex;
            align-items: center;
            background: #FFFFFF;
            position: relative;
        }}
        .perf-circle {{
            width: 24px;
            height: 24px;
            border-radius: 50%;
            background: #F0F2F5;
            flex-shrink: 0;
        }}
        .perf-line {{
            flex: 1;
            border-top: 2px dashed #E0E0E0;
        }}

        /* ── Body ── */
        .body {{
            padding: 32px 40px 36px;
        }}
        .heading {{
            font-size: 22px;
            font-weight: 800;
            color: #1A1A2E;
            margin-bottom: 6px;
            line-height: 1.3;
        }}
        .subheading {{
            font-size: 14px;
            color: #6B7280;
            line-height: 1.6;
            margin-bottom: 28px;
        }}

        /* ── Event info card ── */
        .event-card {{
            background: #F8F7F5;
            border-radius: 14px;
            padding: 20px 24px;
            margin-bottom: 24px;
            border: 1px solid #EEECE9;
        }}
        .event-name {{
            font-size: 16px;
            font-weight: 700;
            color: #1A1A2E;
            margin-bottom: 12px;
        }}
        .event-row {{
            display: flex;
            align-items: center;
            gap: 8px;
            font-size: 14px;
            color: #4B5563;
            margin-bottom: 8px;
        }}
        .event-row:last-child {{
            margin-bottom: 0;
        }}
        .event-icon {{
            color: #FF6B35;
            font-size: 15px;
            width: 18px;
            text-align: center;
            flex-shrink: 0;
        }}

        /* ── Stat pills ── */
        .stats-row {{
            display: flex;
            gap: 12px;
            margin-bottom: 24px;
        }}
        .stat-box {{
            flex: 1;
            background: #FFF8F5;
            border: 1px solid #FFE0D0;
            border-radius: 12px;
            padding: 14px 10px;
            text-align: center;
        }}
        .stat-value {{
            display: block;
            font-size: 20px;
            font-weight: 800;
            color: #FF6B35;
            margin-bottom: 3px;
        }}
        .stat-label {{
            font-size: 11px;
            color: #9CA3AF;
            font-weight: 600;
            text-transform: uppercase;
            letter-spacing: 0.4px;
        }}

        /* ── Ticket ID badge ── */
        .ticket-id-box {{
            border: 1.5px dashed #E5E7EB;
            border-radius: 10px;
            padding: 14px 20px;
            margin-bottom: 24px;
            display: flex;
            align-items: center;
            justify-content: space-between;
            background: #FAFAFA;
        }}
        .ticket-id-label {{
            font-size: 11px;
            font-weight: 700;
            text-transform: uppercase;
            letter-spacing: 1px;
            color: #9CA3AF;
        }}
        .ticket-id-value {{
            font-size: 13px;
            font-weight: 700;
            color: #374151;
            font-family: 'Courier New', Courier, monospace;
            letter-spacing: 1.5px;
        }}

        /* ── Notice box ── */
        .notice {{
            background: #FFF4EC;
            border-left: 3px solid #FF6B35;
            border-radius: 0 10px 10px 0;
            padding: 14px 18px;
            font-size: 13px;
            color: #7C4A2D;
            line-height: 1.6;
        }}

        /* ── Profile rows (welcome email) ── */
        .profile-card {{
            background: #F8F7F5;
            border-radius: 14px;
            padding: 24px;
            margin-bottom: 24px;
            border: 1px solid #EEECE9;
        }}
        .profile-row {{
            display: flex;
            align-items: center;
            justify-content: center;
            gap: 12px;
            font-size: 14px;
            color: #4B5563;
            padding: 16px 0;
            border-bottom: 1px solid #F0EFED;
            text-align: center;
        }}
        .profile-row:last-child {{
            border-bottom: none;
            padding-bottom: 0;
        }}
        .profile-row:first-child {{
            padding-top: 0;
        }}
        .profile-icon {{
            color: #FF6B35;
            font-size: 20px;
            flex-shrink: 0;
        }}
        .profile-label {{
            color: #9CA3AF;
            font-size: 12px;
            font-weight: 600;
            text-transform: uppercase;
            letter-spacing: 0.5px;
            width: 56px;
            text-align: right;
            flex-shrink: 0;
        }}
        .profile-value {{
            font-weight: 600;
            color: #1A1A2E;
            text-align: left;
            min-width: 140px;
        }}
        .role-badge {{
            display: inline-block;
            padding: 3px 12px;
            border-radius: 20px;
            font-size: 12px;
            font-weight: 700;
        }}

        /* ── Footer ── */
        .footer {{
            background: #F8F7F5;
            border-top: 1px solid #EEECE9;
            padding: 20px 40px;
            text-align: center;
        }}
        .footer p {{
            font-size: 12px;
            color: #9CA3AF;
            line-height: 1.6;
        }}
    </style>
</head>
<body>
    <div class='wrapper'>
        <div class='container'>
            <div class='header'>
                <div class='header-logo'><span>🎟️</span> Event Bridge</div>
                <div class='header-tagline'>Your gateway to unforgettable events</div>
            </div>
            <div class='perforation'>
                <div class='perf-circle' style='margin-left:-12px;'></div>
                <div class='perf-line'></div>
                <div class='perf-circle' style='margin-right:-12px;'></div>
            </div>
            {content}
            <div class='footer'>
                <p>© {DateTime.UtcNow.Year} Event Bridge. All rights reserved.<br/>
                This is an automated message — please do not reply to this email.</p>
            </div>
        </div>
    </div>
</body>
</html>";
    }

    // ─── Booking Confirmation ─────────────────────────────────────────────────

    public static string GetBookingConfirmationHtml(
        string attendeeName,
        string eventName,
        DateTime eventDate,
        int quantity,
        decimal ticketPrice)
    {
        var totalCost = quantity * ticketPrice;

        var content = $@"
            <div class='body'>
                <p class='heading'>You're going! 🎉</p>
                <p class='subheading'>Hi {attendeeName}, your booking is confirmed. Get ready for an amazing time!</p>

                <div class='event-card'>
                    <div class='event-name'>{eventName}</div>
                    <div class='event-row'>
                        <span class='event-icon'>📅</span>
                        <span>{eventDate:dddd, MMMM dd yyyy}</span>
                    </div>
                    <div class='event-row'>
                        <span class='event-icon'>🕐</span>
                        <span>{eventDate:h:mm tt}</span>
                    </div>
                </div>

                <div class='stats-row'>
                    <div class='stat-box'>
                        <span class='stat-value'>{quantity}</span>
                        <span class='stat-label'>{(quantity == 1 ? "Ticket" : "Tickets")}</span>
                    </div>
                    <div class='stat-box'>
                        <span class='stat-value'>${ticketPrice:N2}</span>
                        <span class='stat-label'>Per Ticket</span>
                    </div>
                    <div class='stat-box'>
                        <span class='stat-value'>${totalCost:N2}</span>
                        <span class='stat-label'>Total</span>
                    </div>
                </div>

                <div class='notice'>
                    🎫 <strong>Keep this email as your receipt.</strong> Present your ticket ID at the entrance.
                    Tickets can be transferred to a friend through the app if plans change.
                </div>
            </div>";

        return GetBaseTemplate($"Booking Confirmed – {eventName}", content);
    }

    // ─── Ticket Transfer Notification ────────────────────────────────────────

    public static string GetTicketTransferHtml(
        string recipientName,
        string senderName,
        string eventName,
        DateTime eventDate,
        Guid ticketId)
    {
        var shortTicketId = ticketId.ToString().ToUpper();

        var content = $@"
            <div class='body'>
                <p class='heading'>A ticket just arrived for you! 🎟️</p>
                <p class='subheading'>Hi {recipientName}, <strong>{senderName}</strong> has transferred a ticket to you — you're all set!</p>

                <div class='event-card'>
                    <div class='event-name'>{eventName}</div>
                    <div class='event-row'>
                        <span class='event-icon'>📅</span>
                        <span>{eventDate:dddd, MMMM dd yyyy}</span>
                    </div>
                    <div class='event-row'>
                        <span class='event-icon'>🕐</span>
                        <span>{eventDate:h:mm tt}</span>
                    </div>
                    <div class='event-row'>
                        <span class='event-icon'>👤</span>
                        <span>Transferred by <strong>{senderName}</strong></span>
                    </div>
                </div>

                <div class='ticket-id-box'>
                    <span class='ticket-id-label'>Ticket ID</span>
                    <span class='ticket-id-value'>{shortTicketId}</span>
                </div>

                <div class='notice'>
                    🚪 <strong>This ticket is now registered in your name.</strong> Show your Ticket ID at the venue entrance.
                </div>
            </div>";

        return GetBaseTemplate($"Ticket Received – {eventName}", content);
    }

    // ─── Ticket Cancellation Notification ───────────────────────────────────

    public static string GetTicketCancellationHtml(
        string fullName,
        string eventName,
        DateTime eventDate,
        Guid ticketId)
    {
        var shortTicketId = ticketId.ToString().ToUpper();

        var content = $@"
            <div class='body'>
                <p class='heading'>Ticket cancelled</p>
                <p class='subheading'>Hi {fullName}, your ticket cancellation has been completed.</p>

                <div class='event-card'>
                    <div class='event-name'>{eventName}</div>
                    <div class='event-row'>
                        <span class='event-icon'>Date</span>
                        <span>{eventDate:dddd, MMMM dd yyyy}</span>
                    </div>
                    <div class='event-row'>
                        <span class='event-icon'>Time</span>
                        <span>{eventDate:h:mm tt}</span>
                    </div>
                </div>

                <div class='ticket-id-box'>
                    <span class='ticket-id-label'>Cancelled Ticket ID</span>
                    <span class='ticket-id-value'>{shortTicketId}</span>
                </div>

                <div class='notice'>
                    <strong>Your seat has been released.</strong> This ticket can no longer be used at the venue.
                </div>
            </div>";

        return GetBaseTemplate($"Ticket Cancelled - {eventName}", content);
    }

    // ─── Welcome / Account Created ────────────────────────────────────────────

    public static string GetWelcomeEmailHtml(string fullName, string email, string role)
    {
        var isOrganizer = role.Equals("Organizer", StringComparison.OrdinalIgnoreCase);
        var roleEmoji   = isOrganizer ? "🎙️" : "🎫";
        var roleBgColor = isOrganizer ? "#FFF7E6" : "#ECFDF5";
        var roleColor   = isOrganizer ? "#D97706" : "#059669";
        var roleTip     = isOrganizer
            ? "Head over to the dashboard and create your first event. Set the capacity, price, and date — Event Bridge handles the rest."
            : "Browse upcoming events, secure your spot, and transfer tickets to friends right from the app.";

        var content = $@"
            <div class='body'>
                <p class='heading'>Welcome to Event Bridge! {roleEmoji}</p>
                <p class='subheading'>Hi {fullName}, your account is all set up. Welcome to the community — where great events come to life.</p>

                <div class='profile-card'>
                    <div class='profile-row'>
                        <span class='profile-icon'>👤</span>
                        <span class='profile-label'>Name</span>
                        <span class='profile-value'>{fullName}</span>
                    </div>
                    <div class='profile-row'>
                        <span class='profile-icon'>✉️</span>
                        <span class='profile-label'>Email</span>
                        <span class='profile-value'>{email}</span>
                    </div>
                    <div class='profile-row'>
                        <span class='profile-icon'>{roleEmoji}</span>
                        <span class='profile-label'>Role</span>
                        <span class='role-badge' style='background:{roleBgColor}; color:{roleColor};'>{role}</span>
                    </div>
                </div>

                <div class='notice'>
                    💡 <strong>What's next?</strong><br/>
                    {roleTip}
                </div>
            </div>";

        return GetBaseTemplate($"Welcome to Event Bridge, {fullName}!", content);
    }
}
