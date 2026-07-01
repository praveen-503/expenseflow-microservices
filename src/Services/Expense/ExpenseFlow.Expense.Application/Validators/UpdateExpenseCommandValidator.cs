using FluentValidation;
using ExpenseFlow.Expense.Application.Commands;
using System;

namespace ExpenseFlow.Expense.Application.Validators;

public class UpdateExpenseCommandValidator : AbstractValidator<UpdateExpenseCommand>
{
    public UpdateExpenseCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Expense ID is required.");
        RuleFor(x => x.Title).NotEmpty().WithMessage("Title is required.")
            .MaximumLength(150).WithMessage("Title cannot exceed 150 characters.");
        RuleFor(x => x.Amount).GreaterThan(0).WithMessage("Amount must be greater than zero.");
        RuleFor(x => x.ExpenseDate).NotEmpty().WithMessage("Expense Date is required.");
        RuleFor(x => x.CategoryId).NotEmpty().WithMessage("CategoryId is required.");
        RuleFor(x => x.UserId).NotEmpty().WithMessage("UserId is required.");
    }
}
