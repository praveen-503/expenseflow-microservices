using FluentValidation;
using ExpenseFlow.Identity.Application.Commands;

namespace ExpenseFlow.Identity.Application.Validators;

public class CreateRoleCommandValidator : AbstractValidator<CreateRoleCommand>
{
    public CreateRoleCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Role name is required.")
            .MaximumLength(100).WithMessage("Role name cannot exceed 100 characters.");
    }
}
