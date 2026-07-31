using TicketFlow_v2.DTOs.Request;
using TicketFlow_v2.DTOs.Response;
using TicketFlow_v2.Models;
using TicketFlow_v2.Repository.Interface;
using TicketFlow_v2.Services.Interface;

namespace TicketFlow_v2.Services.Implementation;

public class EventService(
    IEventRepository eventRepository,
    IUserRepository userRepository,
    ILogger<EventService> logger) : IEventService
{
    private readonly IEventRepository _eventRepository = eventRepository;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly ILogger<EventService> _logger = logger;

    public async Task<ApiResponse<EventResponse>> CreateEventAsync(CreateEventRequest request)
    {
        try
        {
            var organizer = await _userRepository.GetUserAsync(request.OrganizerId);
            if (organizer == null)
            {
                _logger.LogInformation("User with ID {OrganizerId} not found", request.OrganizerId);
                return ApiResponse<EventResponse>.FailureResponse("User not found");
            }

            if (organizer.Role != UserRole.ORGANIZER)
            {
                _logger.LogInformation("User with ID {OrganizerId} is not an organizer", request.OrganizerId);
                return ApiResponse<EventResponse>.FailureResponse("User is not eligible to create an event");
            }

            if (request.Date < DateTime.UtcNow)
            {
                _logger.LogInformation("Event date {EventDate} is in the past", request.Date);
                return ApiResponse<EventResponse>.FailureResponse("Event date cannot be in the past");
            }

            var newEvent = new Event()
            {
                OrganizerId = request.OrganizerId,
                Name = request.Name,
                TotalCapacity = request.TotalCapacity,
                AvailableSeats = request.TotalCapacity,
                TicketPrice = request.TicketPrice,
                Date = request.Date,
                CreatedAt = DateTime.UtcNow
            };

            await _eventRepository.CreateEventAsync(newEvent);

            _logger.LogInformation("Successfully created event with ID {EventId}", newEvent.Id);
            return ApiResponse<EventResponse>.SuccessResponse(MapToResponse(newEvent));
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to create event");
            return ApiResponse<EventResponse>.FailureResponse("An error occurred while creating event");
        }
    }

    public async Task<ApiResponse<IEnumerable<EventResponse>>> GetAllEventsAsync()
    {
        try
        {
            var events = await _eventRepository.GetAllEventsAsync();
            return ApiResponse<IEnumerable<EventResponse>>.SuccessResponse(events.Select(MapToResponse));
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to get all events");
            return ApiResponse<IEnumerable<EventResponse>>.FailureResponse("Failed to get all events");
        }
    }

    public async Task<ApiResponse<IEnumerable<EventResponse>>> GetAvailableEventsAsync()
    {
        try
        {
            var events = await _eventRepository.GetAvailableEventsAsync();
            return ApiResponse<IEnumerable<EventResponse>>.SuccessResponse(events.Select(MapToResponse));
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to get all available events");
            return ApiResponse<IEnumerable<EventResponse>>.FailureResponse("Failed to get all available events");
        }
    }

    public async Task<ApiResponse<IEnumerable<BookingLogResponse>>> GetAuditLogAsync(Guid eventId)
    {
        try
        {
            var logs = await _eventRepository.GetAuditLogAsync(eventId);
            var response = logs.Select(log => new BookingLogResponse()
            {
                Id = log.Id,
                TicketId = log.TicketId,
                EventId = log.EventId,
                UserId = log.UserId,
                Action = log.Action,
                Note = log.Note,
                Timestamp = log.TimeStamp
            });

            _logger.LogInformation("Successfully retrieved audit log for event {EventId}", eventId);
            return ApiResponse<IEnumerable<BookingLogResponse>>.SuccessResponse(response);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to retrieve audit log");
            return ApiResponse<IEnumerable<BookingLogResponse>>.FailureResponse("Failed to retrieve audit log");
        }
    }

    public async Task<ApiResponse<IEnumerable<BookingLogResponse>>> GetAllAuditLogsAsync()
    {
        try
        {
            var logs = await _eventRepository.GetAllAuditLogsAsync();
            var response = logs.Select(MapBookingLogToResponse);

            _logger.LogInformation("Successfully retrieved all audit logs");
            return ApiResponse<IEnumerable<BookingLogResponse>>.SuccessResponse(response);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to retrieve all audit logs");
            return ApiResponse<IEnumerable<BookingLogResponse>>.FailureResponse("Failed to retrieve audit logs");
        }
    }

    public async Task<ApiResponse<EventUsersResponse>> GetEventUsersAsync(Guid eventId)
    {
        try
        {
            var eventDetails = await _eventRepository.GetEventUsersAsync(eventId);
            if (eventDetails == null)
            {
                _logger.LogInformation("Event with ID {EventId} not found", eventId);
                return ApiResponse<EventUsersResponse>.FailureResponse("Event not found");
            }

            var users = await _eventRepository.GetUsersByEventAsync(eventId);
            var response = new EventUsersResponse()
            {
                Id = eventDetails.Id,
                Name = eventDetails.Name,
                Date = eventDetails.Date,
                TotalCapacity = eventDetails.TotalCapacity,
                AvailableSeats = eventDetails.AvailableSeats,
                TicketPrice = eventDetails.TicketPrice,
                CreatedAt = eventDetails.CreatedAt,
                Users = users.Select(MapUserToResponse)
            };

            _logger.LogInformation("Successfully retrieved event {EventId} users", eventId);
            return ApiResponse<EventUsersResponse>.SuccessResponse(response);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to retrieve event with users");
            return ApiResponse<EventUsersResponse>.FailureResponse("Failed to retrieve event details");
        }
    }

    private static EventResponse MapToResponse(Event request)
    {
        return new EventResponse()
        {
            Id = request.Id,
            Name = request.Name,
            TotalCapacity = request.TotalCapacity,
            AvailableSeats = request.AvailableSeats,
            TicketPrice = request.TicketPrice,
            Date = request.Date,
            CreatedAt = request.CreatedAt
        };
    }

    private static UserResponse MapUserToResponse(User request)
    {
        return new UserResponse()
        {
            Id = request.Id,
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            PhoneNumber = request.PhoneNumber,
            Role = request.Role,
            CreatedAt = request.CreatedAt,
        };
    }

    private static BookingLogResponse MapBookingLogToResponse(BookingLog request)
    {
        return new BookingLogResponse()
        {
            Id = request.Id,
            TicketId = request.TicketId,
            EventId = request.EventId,
            UserId = request.UserId,
            Action = request.Action,
            Note = request.Note,
            Timestamp = request.TimeStamp
        };
    }
}
