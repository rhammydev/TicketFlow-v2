using FluentValidation;
using TicketFlow_v2.DTOs.Request;

namespace TicketFlow_v2.Validators;

public class TransferTicketRequestValidator : AbstractValidator<TransferTicketRequest>
{
    public TransferTicketRequestValidator()
    {
        RuleFor(x => x.TicketId).NotEmpty();
        RuleFor(x => x.FromUserId).NotEmpty();
        RuleFor(x => x.ToUserEmail).NotEmpty().EmailAddress();
    }
}
