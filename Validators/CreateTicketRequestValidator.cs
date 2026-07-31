using FluentValidation;
using TicketFlow_v2.DTOs.Request;

namespace TicketFlow_v2.Validators;

public class CreateTicketRequestValidator : AbstractValidator<CreateTicketRequest>
{
    public CreateTicketRequestValidator()
    {
        RuleFor(x => x.EventId).NotEmpty();
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.Quantity).GreaterThan(0);
    }
}
