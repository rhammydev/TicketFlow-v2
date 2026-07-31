using FluentValidation;
using TicketFlow_v2.DTOs.Request;
using TicketFlow_v2.DTOs.Response;
using TicketFlow_v2.Services.Interface;

namespace TicketFlow_v2.Endpoints;

public static class EventEndpoints
{
    public static void MapEventEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/api/events")
            .WithTags("events");

        group.MapGet("/", async (IEventService eventService) =>
        {
            var response = await eventService.GetAllEventsAsync();
            return Results.Ok(response);
        });

        group.MapGet("/available", async (IEventService eventService) =>
        {
            var response = await eventService.GetAvailableEventsAsync();
            return Results.Ok(response);
        });

        group.MapGet("/GetAlluserbyEventID/{eventId:guid}", async (Guid eventId, IEventService eventService) =>
        {
            var response = await eventService.GetEventUsersAsync(eventId);
            return Results.Ok(response);
        });

        group.MapPost("/create", async (
            CreateEventRequest createEventRequest,
            IValidator<CreateEventRequest> validator,
            IEventService eventService) =>
        {
            var validateResult = await validator.ValidateAsync(createEventRequest);
            if (!validateResult.IsValid)
            {
                var errors = string.Join(" ", validateResult.Errors.Select(e => e.ErrorMessage));
                return Results.BadRequest(ApiResponse<EventResponse>.FailureResponse(errors));
            }

            var response = await eventService.CreateEventAsync(createEventRequest);
            return Results.Ok(response);
        });
    }
}
