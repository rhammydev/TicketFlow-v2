using FluentValidation;
using TicketFlow_v2.DTOs.Request;
using TicketFlow_v2.DTOs.Response;
using TicketFlow_v2.Services.Interface;

namespace TicketFlow_v2.Endpoints;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/api/users")
            .WithTags("users");

        group.MapGet("/", async (IUserService userService) =>
        {
            var response = await userService.GetAllUsersAsync();
            return Results.Ok(response);
        });

        group.MapGet("/{userId:guid}", async (Guid userId, IUserService userService) =>
        {
            var response = await userService.GetUserAsync(userId);
            return Results.Ok(response);
        });

        group.MapGet("/{userId:guid}/tickets", async (Guid userId, IUserService userService) =>
        {
            var response = await userService.GetUserTicketsAsync(userId);
            return Results.Ok(response);
        });

        group.MapPost("/register", async (
            CreateUserRequest createUserRequest,
            IValidator<CreateUserRequest> validator,
            IUserService userService) =>
        {
            var validateResult = await validator.ValidateAsync(createUserRequest);
            if (!validateResult.IsValid)
            {
                var errors = string.Join(" ", validateResult.Errors.Select(e => e.ErrorMessage));
                return Results.BadRequest(ApiResponse<NewUserResponse>.FailureResponse(errors));
            }

            var response = await userService.CreateUserAsync(createUserRequest);
            return Results.Ok(response);
        });
    }
}
