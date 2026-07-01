using FluentValidation;
using ExpenseFlow.Identity.Application.Commands;

namespace ExpenseFlow.Identity.Application.Validators;

public class LogoutCommandValidator : AbstractValidator<LogoutCommand>
{
    public LogoutCommandValidator()
    {
        RuleFor(x => x.RefreshToken).NotEmpty().WithMessage("Refresh token is required.");
    }
}
