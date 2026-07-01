using FluentValidation;
using ExpenseFlow.Identity.Application.Commands;

namespace ExpenseFlow.Identity.Application.Validators;

public class RemoveClaimCommandValidator : AbstractValidator<RemoveClaimCommand>
{
    public RemoveClaimCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().WithMessage("User ID is required.");
        RuleFor(x => x.ClaimType).NotEmpty().WithMessage("Claim type is required.");
        RuleFor(x => x.ClaimValue).NotEmpty().WithMessage("Claim value is required.");
    }
}
