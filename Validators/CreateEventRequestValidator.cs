using FluentValidation;
using TicketFlow_v2.DTOs.Request;

namespace TicketFlow_v2.Validators;

public class CreateEventRequestValidator : AbstractValidator<CreateEventRequest>
{
    public CreateEventRequestValidator()
    {
        RuleFor(x => x.OrganizerId).NotEmpty();
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Date).GreaterThan(DateTime.UtcNow).WithMessage("Event date must be in the future.");
        RuleFor(x => x.TotalCapacity).GreaterThan(0);
        RuleFor(x => x.TicketPrice).GreaterThanOrEqualTo(0);
    }
}
