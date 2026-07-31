using TicketFlow_v2.Repository.Interface;
using TicketFlow_v2.Services.Interface;
using TicketFlow_v2.DTOs.Request;
using TicketFlow_v2.DTOs.Response;
using TicketFlow_v2.Models;
using TicketFlow_v2.Utilities;

namespace TicketFlow_v2.Services.Implementation;

public class UserService(
    IUserRepository userRepository,
    ILogger<UserService> logger,
    IEmailService emailService,
    ISmsService smsService) : IUserService
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IEmailService _emailService = emailService;
    private readonly ISmsService _smsService = smsService;
    private readonly ILogger<UserService> _logger = logger;
    
    public async Task<ApiResponse<NewUserResponse>> CreateUserAsync(CreateUserRequest request)
    {
        try
        {
            var checkUserEmail = await _userRepository.GetUserByEmailAsync(request.Email);
            if (checkUserEmail != null)
            {
                _logger.LogInformation($"Account with email {request.Email} already exists");
                return ApiResponse<NewUserResponse>.FailureResponse("Account with email already exists");
            }

            var user = new User()
            {
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                PhoneNumber = request.PhoneNumber,
                Role = request.Role,
            };
            
            await _userRepository.CreateUserAsync(user);
            
            _logger.LogInformation($"Successfully created an  account with email {request.Email}");
            
            await _emailService.SendWelcomeEmailAsync(user.Email, $"{user.FirstName} {user.LastName}", request.Role.ToString());

            var fullName = $"{user.FirstName} {user.LastName}";
            await _smsService.SendSmsAsync(user.PhoneNumber, SmsUtils.GetWelcomeSms(fullName, user.Role.ToString()));

            var response = new NewUserResponse()
            {
                UserId = user.Id,
            };
            return ApiResponse<NewUserResponse>.SuccessResponse(response);

        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to create user");
            return ApiResponse<NewUserResponse>.FailureResponse("An error occurred while creating the user");
        }
    }

    public async Task<ApiResponse<IEnumerable<UserResponse>>> GetAllUsersAsync()
    {
        try
        {
            var users = await _userRepository.GetAllUsersAsync();
            _logger.LogInformation("Successfully retrieved all users");
            return ApiResponse<IEnumerable<UserResponse>>.SuccessResponse(users.Select(MapToResponse));
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to get all users");
            return ApiResponse<IEnumerable<UserResponse>>.FailureResponse("An error occurred while getting all users");
        }
    }

    public async Task<ApiResponse<UserResponse>> GetUserAsync(Guid userId)
    {
        try
        {
            var user = await _userRepository.GetUserAsync(userId);
            if (user == null)
            {
                _logger.LogError($"User with id {userId} not found");
                return ApiResponse<UserResponse>.FailureResponse($"User with id {userId} not found");
            }
            
            _logger.LogInformation("Successfully retrieved all users");
            return ApiResponse<UserResponse>.SuccessResponse(MapToResponse(user)); 
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to get user");
            return ApiResponse<UserResponse>.FailureResponse("An error occurred while getting the user");
        }
    }
    
    public async Task<ApiResponse<UserResponse>> GetUserByEmailAsync(string email)
    {
        try
        {
            var user = await _userRepository.GetUserByEmailAsync(email);
            if (user == null)
            {
                _logger.LogError($"User with email {email} not found");
                return ApiResponse<UserResponse>.FailureResponse($"User with email {email} not found");
            }
            
            _logger.LogInformation("Successfully retrieved all users");
            return ApiResponse<UserResponse>.SuccessResponse(MapToResponse(user)); 
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to get user");
            return ApiResponse<UserResponse>.FailureResponse("An error occurred while getting the user");
        }
    }

    public async Task<ApiResponse<UserTicketsResponse>> GetUserTicketsAsync(Guid userId)
    {
        try
        {
            var user = await _userRepository.GetUserAsync(userId);
            if (user == null)
            {
                _logger.LogInformation($"User with ID {userId} not found");
                return ApiResponse<UserTicketsResponse>.FailureResponse("User not found");
            }
            
            var tickets = await _userRepository.GetUserTicketsAsync(userId);

            var response = new UserTicketsResponse()
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                Role = user.Role,
                Tickets = tickets.Select(MapTicketToResponse)
            };

            _logger.LogInformation($"Successfully retrieved tickets for user with ID {userId}");
            return ApiResponse<UserTicketsResponse>.SuccessResponse(response);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to get user tickets");
            return ApiResponse<UserTicketsResponse>.FailureResponse("An error occurred while getting user tickets");
        }
    }

    private static TicketResponse MapTicketToResponse(Ticket request)
    {
        return new TicketResponse()
        {
            Id = request.Id,
            EventId = request.EventId,
            OwnerId = request.OwnerId,
            Status = request.Status,
            CreatedAt = request.CreatedAt,
        };
    }
    
    private static UserResponse MapToResponse(User request)
    {
        return new UserResponse()
        {
            Id = request.Id,
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            PhoneNumber = request.PhoneNumber,
            Role = request.Role,
            CreatedAt =  request.CreatedAt,
        };
    }
}
