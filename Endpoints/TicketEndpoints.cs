using FluentValidation;
using TicketFlow_v2.DTOs.Request;
using TicketFlow_v2.DTOs.Response;
using TicketFlow_v2.Services.Interface;

namespace TicketFlow_v2.Endpoints;

public static class TicketEndpoints
{
    public static void MapTicketEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/api/tickets")
            .WithTags("tickets");

        group.MapGet("/audit-log", async (IEventService eventService) =>
        {
            var response = await eventService.GetAllAuditLogsAsync();
            return Results.Ok(response);
        });

        group.MapGet("/{eventId:guid}/audit-log", async (Guid eventId, IEventService eventService) =>
        {
            var response = await eventService.GetAuditLogAsync(eventId);
            return Results.Ok(response);
        });

        group.MapPost("/book", async (
            CreateTicketRequest createTicketRequest,
            IValidator<CreateTicketRequest> validator,
            ITicketService ticketService) =>
        {
            var validateResult = await validator.ValidateAsync(createTicketRequest);
            if (!validateResult.IsValid)
            {
                var errors = string.Join(" ", validateResult.Errors.Select(e => e.ErrorMessage));
                return Results.BadRequest(ApiResponse<IEnumerable<TicketResponse>>.FailureResponse(errors));
            }

            var response = await ticketService.CreateTicketAsync(createTicketRequest);
            return Results.Ok(response);
        });

        group.MapPost("/transfer", async (
            TransferTicketRequest transferTicketRequest,
            IValidator<TransferTicketRequest> validator,
            ITicketService ticketService) =>
        {
            var validateResult = await validator.ValidateAsync(transferTicketRequest);
            if (!validateResult.IsValid)
            {
                var errors = string.Join(" ", validateResult.Errors.Select(e => e.ErrorMessage));
                return Results.BadRequest(ApiResponse<TicketResponse>.FailureResponse(errors));
            }

            var response = await ticketService.TransferTicketAsync(transferTicketRequest);
            return Results.Ok(response);
        });

        group.MapPost("/cancel", async (
            CancelTicketRequest cancelTicketRequest,
            IValidator<CancelTicketRequest> validator,
            ITicketService ticketService) =>
        {
            var validateResult = await validator.ValidateAsync(cancelTicketRequest);
            if (!validateResult.IsValid)
            {
                var errors = string.Join(" ", validateResult.Errors.Select(e => e.ErrorMessage));
                return Results.BadRequest(ApiResponse<TicketResponse>.FailureResponse(errors));
            }

            var response = await ticketService.CancelTicketAsync(cancelTicketRequest);
            return Results.Ok(response);
        });
    }
}
