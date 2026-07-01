using FluentValidation;
using ExpenseFlow.Identity.Application.Commands;

namespace ExpenseFlow.Identity.Application.Validators;

public class AssignClaimCommandValidator : AbstractValidator<AssignClaimCommand>
{
    public AssignClaimCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().WithMessage("User ID is required.");
        RuleFor(x => x.ClaimType).NotEmpty().WithMessage("Claim type is required.");
        RuleFor(x => x.ClaimValue).NotEmpty().WithMessage("Claim value is required.");
    }
}
