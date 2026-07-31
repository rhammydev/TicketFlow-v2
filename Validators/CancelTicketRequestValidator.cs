using FluentValidation;
using TicketFlow_v2.DTOs.Request;

namespace TicketFlow_v2.Validators;

public class CancelTicketRequestValidator : AbstractValidator<CancelTicketRequest>
{
    public CancelTicketRequestValidator()
    {
        RuleFor(x => x.TicketId).NotEmpty();
        RuleFor(x => x.UserId).NotEmpty();
    }
}
