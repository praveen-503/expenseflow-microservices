using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using ExpenseFlow.Expense.Domain.Common;
using ExpenseFlow.Expense.Domain.Interfaces;
using ExpenseFlow.Expense.Application.Commands;

namespace ExpenseFlow.Expense.Application.Handlers;

public class UpdateExpenseCommandHandler : IRequestHandler<UpdateExpenseCommand, Result>
{
    private readonly IExpenseRepository _expenseRepository;
    private readonly ICategoryRepository _categoryRepository;

    public UpdateExpenseCommandHandler(
        IExpenseRepository expenseRepository,
        ICategoryRepository categoryRepository)
    {
        _expenseRepository = expenseRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task<Result> Handle(UpdateExpenseCommand request, CancellationToken cancellationToken)
    {
        var expense = await _expenseRepository.GetByIdAsync(request.Id, cancellationToken);
        if (expense == null || expense.UserId != request.UserId)
        {
            return Result.Failure(new Error("Expense.NotFound", "Expense not found."));
        }

        var category = await _categoryRepository.GetByIdAsync(request.CategoryId, cancellationToken);
        if (category == null)
        {
            return Result.Failure(new Error("Category.NotFound", "Category not found."));
        }

        expense.Description = request.Description;
        expense.Amount = request.Amount;
        expense.Date = request.Date;
        expense.CategoryId = request.CategoryId;

        _expenseRepository.Update(expense);

        return Result.Success();
    }
}
