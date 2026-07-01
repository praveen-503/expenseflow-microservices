using FluentValidation;
using ExpenseFlow.Identity.Application.Commands;

namespace ExpenseFlow.Identity.Application.Validators;

public class RemoveRoleCommandValidator : AbstractValidator<RemoveRoleCommand>
{
    public RemoveRoleCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().WithMessage("User ID is required.");
        RuleFor(x => x.RoleName).NotEmpty().WithMessage("Role name is required.");
    }
}
