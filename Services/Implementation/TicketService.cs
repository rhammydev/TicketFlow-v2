using TicketFlow_v2.DTOs.Request;
using TicketFlow_v2.DTOs.Response;
using TicketFlow_v2.Models;
using TicketFlow_v2.Repository.Interface;
using TicketFlow_v2.Services.Interface;
using TicketFlow_v2.Utilities;

namespace TicketFlow_v2.Services.Implementation;

public class TicketService(
    ITicketRepository ticketRepository,
    IEventRepository eventRepository,
    IUserRepository userRepository,
    IEmailService emailService,
    ISmsService smsService,
    ILogger<TicketService> logger) : ITicketService
{
    private readonly ITicketRepository _ticketRepository = ticketRepository;
    private readonly IEventRepository _eventRepository = eventRepository;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IEmailService _emailService = emailService;
    private readonly ISmsService _smsService = smsService;
    private readonly ILogger<TicketService> _logger = logger;

    public async Task<ApiResponse<IEnumerable<TicketResponse>>> CreateTicketAsync(CreateTicketRequest request)
    {
        try
        {
            var eventDetails = await _eventRepository.GetEventUsersAsync(request.EventId);
            if (eventDetails == null)
            {
                _logger.LogInformation("Event with ID {EventId} not found", request.EventId);
                return ApiResponse<IEnumerable<TicketResponse>>.FailureResponse("Event not found");
            }

            var user = await _userRepository.GetUserAsync(request.UserId);
            if (user == null)
            {
                _logger.LogInformation("User with ID {UserId} not found", request.UserId);
                return ApiResponse<IEnumerable<TicketResponse>>.FailureResponse("User not found");
            }

            if (eventDetails.AvailableSeats < request.Quantity)
            {
                _logger.LogInformation(
                    "Event with ID {EventId} only has {AvailableSeats} available seats",
                    request.EventId,
                    eventDetails.AvailableSeats);
                return ApiResponse<IEnumerable<TicketResponse>>.FailureResponse("Not enough available seats");
            }

            var generatedTickets = new List<Ticket>();
            var generatedLogs = new List<BookingLog>();

            for (var i = 0; i < request.Quantity; i++)
            {
                var ticket = new Ticket()
                {
                    Id = Guid.NewGuid(),
                    EventId = request.EventId,
                    OwnerId = request.UserId,
                    Quantity = 1,
                    Status = TicketStatus.ACTIVE,
                    CreatedAt = DateTime.UtcNow
                };

                generatedTickets.Add(ticket);
                generatedLogs.Add(new BookingLog()
                {
                    TicketId = ticket.Id,
                    EventId = request.EventId,
                    UserId = request.UserId,
                    Action = ActionType.PURCHASE,
                    TimeStamp = DateTime.UtcNow,
                    Note = $"Ticket booked by attendee: {request.UserId}"
                });
            }

            eventDetails.AvailableSeats -= request.Quantity;
            var tickets = await _ticketRepository.CreateTicketsAsync(generatedTickets, generatedLogs, eventDetails);

            var attendeeName = $"{user.FirstName} {user.LastName}";
            await _emailService.SendBookingConfirmationAsync(
                user.Email,
                attendeeName,
                eventDetails.Name,
                eventDetails.Date,
                request.Quantity,
                eventDetails.TicketPrice);

            await _smsService.SendSmsAsync(
                user.PhoneNumber,
                SmsUtils.GetBookingConfirmationSms(attendeeName, eventDetails.Name, eventDetails.Date, request.Quantity));

            return ApiResponse<IEnumerable<TicketResponse>>.SuccessResponse(tickets.Select(MapToResponse));
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to create tickets");
            return ApiResponse<IEnumerable<TicketResponse>>.FailureResponse("An error occurred while booking tickets");
        }
    }

    public async Task<ApiResponse<TicketResponse>> TransferTicketAsync(TransferTicketRequest request)
    {
        try
        {
            var ticket = await _ticketRepository.GetTicketAsync(request.TicketId);
            if (ticket == null)
            {
                _logger.LogInformation("Ticket with ID {TicketId} not found", request.TicketId);
                return ApiResponse<TicketResponse>.FailureResponse("Ticket not found");
            }

            if (ticket.OwnerId != request.FromUserId)
            {
                _logger.LogWarning(
                    "User {UserId} attempted to transfer ticket {TicketId} which they do not own",
                    request.FromUserId,
                    request.TicketId);
                return ApiResponse<TicketResponse>.FailureResponse("Unauthorized: You do not own this ticket");
            }

            var owner = await _userRepository.GetUserAsync(request.FromUserId);
            if (owner == null)
            {
                _logger.LogInformation("Unauthorized transfer user {UserId}", request.FromUserId);
                return ApiResponse<TicketResponse>.FailureResponse("Unauthorized");
            }

            var recipient = await _userRepository.GetUserByEmailAsync(request.ToUserEmail);
            if (recipient == null)
            {
                _logger.LogInformation("User with email {Email} not found", request.ToUserEmail);
                return ApiResponse<TicketResponse>.FailureResponse("Recipient not found");
            }

            var eventDetails = await _eventRepository.GetEventUsersAsync(ticket.EventId);
            if (eventDetails == null)
            {
                _logger.LogInformation("Event with ID {EventId} not found", ticket.EventId);
                return ApiResponse<TicketResponse>.FailureResponse("Event not found");
            }

            ticket.OwnerId = recipient.Id;
            await _ticketRepository.UpdateTicketWithLogAsync(ticket, new BookingLog()
            {
                TicketId = ticket.Id,
                EventId = ticket.EventId,
                UserId = request.FromUserId,
                Action = ActionType.TRANSFER,
                TimeStamp = DateTime.UtcNow,
                Note = $"Ticket transferred to {recipient.Email}"
            });

            var senderName = $"{owner.FirstName} {owner.LastName}";
            var recipientName = $"{recipient.FirstName} {recipient.LastName}";

            await _emailService.SendTicketTransferNotificationAsync(
                recipient.Email,
                recipientName,
                senderName,
                eventDetails.Name,
                eventDetails.Date,
                ticket.Id);

            await _smsService.SendSmsAsync(
                recipient.PhoneNumber,
                SmsUtils.GetTicketTransferSms(recipientName, senderName, eventDetails.Name, eventDetails.Date, ticket.Id));

            _logger.LogInformation("Successfully transferred ticket with ID {TicketId}", ticket.Id);
            return ApiResponse<TicketResponse>.SuccessResponse(MapToResponse(ticket));
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to transfer ticket");
            return ApiResponse<TicketResponse>.FailureResponse("Failed to transfer ticket");
        }
    }

    public async Task<ApiResponse<TicketResponse>> CancelTicketAsync(CancelTicketRequest request)
    {
        try
        {
            var ticket = await _ticketRepository.GetTicketAsync(request.TicketId);
            if (ticket == null)
            {
                _logger.LogInformation("Ticket with ID {TicketId} not found", request.TicketId);
                return ApiResponse<TicketResponse>.FailureResponse("Ticket not found");
            }

            if (ticket.Status == TicketStatus.CANCELLED)
            {
                _logger.LogInformation("Ticket {TicketId} has already been cancelled", request.TicketId);
                return ApiResponse<TicketResponse>.FailureResponse("Ticket already cancelled");
            }

            var owner = await _userRepository.GetUserAsync(request.UserId);
            if (owner == null)
            {
                _logger.LogInformation("Owner with ID {UserId} not found", request.UserId);
                return ApiResponse<TicketResponse>.FailureResponse("Unauthorized");
            }

            if (ticket.OwnerId != owner.Id)
            {
                _logger.LogWarning(
                    "User {UserId} attempted to cancel ticket {TicketId} which they do not own",
                    owner.Id,
                    ticket.Id);
                return ApiResponse<TicketResponse>.FailureResponse("Unauthorized: You do not own this ticket");
            }

            var eventDetails = await _eventRepository.GetEventUsersAsync(ticket.EventId);
            if (eventDetails == null)
            {
                _logger.LogInformation("Event with ID {EventId} not found", ticket.EventId);
                return ApiResponse<TicketResponse>.FailureResponse("Event not found");
            }

            ticket.Status = TicketStatus.CANCELLED;
            eventDetails.AvailableSeats++;

            await _ticketRepository.UpdateTicketEventAndLogAsync(ticket, eventDetails, new BookingLog()
            {
                TicketId = ticket.Id,
                EventId = ticket.EventId,
                UserId = request.UserId,
                Action = ActionType.CANCEL,
                TimeStamp = DateTime.UtcNow,
                Note = "Ticket cancelled by user"
            });

            var ownerName = $"{owner.FirstName} {owner.LastName}";
            await _emailService.SendTicketCancellationNotificationAsync(
                owner.Email,
                ownerName,
                eventDetails.Name,
                eventDetails.Date,
                ticket.Id);

            await _smsService.SendSmsAsync(
                owner.PhoneNumber,
                SmsUtils.GetTicketCancellationSms(ownerName, eventDetails.Name, eventDetails.Date, ticket.Id));

            _logger.LogInformation("Successfully cancelled ticket with ID {TicketId}", request.TicketId);
            return ApiResponse<TicketResponse>.SuccessResponse(MapToResponse(ticket));
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to cancel ticket");
            return ApiResponse<TicketResponse>.FailureResponse("Failed to cancel ticket");
        }
    }

    private static TicketResponse MapToResponse(Ticket request)
    {
        return new TicketResponse()
        {
            Id = request.Id,
            EventId = request.EventId,
            OwnerId = request.OwnerId,
            Status = request.Status,
            CreatedAt = request.CreatedAt
        };
    }
}
