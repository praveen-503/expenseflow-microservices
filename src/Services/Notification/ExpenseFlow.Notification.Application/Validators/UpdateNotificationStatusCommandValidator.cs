using FluentValidation;
using ExpenseFlow.Notification.Application.Commands;

namespace ExpenseFlow.Notification.Application.Validators;

public class UpdateNotificationStatusCommandValidator : AbstractValidator<UpdateNotificationStatusCommand>
{
    public UpdateNotificationStatusCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Status).IsInEnum();
    }
}
