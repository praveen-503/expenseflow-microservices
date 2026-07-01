using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using ExpenseFlow.Expense.Domain.Common;
using ExpenseFlow.Expense.Domain.Interfaces;
using ExpenseFlow.Expense.Application.Commands;

namespace ExpenseFlow.Expense.Application.Handlers;

public class DeleteExpenseCommandHandler : IRequestHandler<DeleteExpenseCommand, Result>
{
    private readonly IExpenseRepository _expenseRepository;

    public DeleteExpenseCommandHandler(IExpenseRepository expenseRepository)
    {
        _expenseRepository = expenseRepository;
    }

    public async Task<Result> Handle(DeleteExpenseCommand request, CancellationToken cancellationToken)
    {
        var expense = await _expenseRepository.GetByIdAsync(request.Id, cancellationToken);
        if (expense == null || expense.UserId != request.UserId)
        {
            return Result.Failure(new Error("Expense.NotFound", "Expense not found."));
        }

        _expenseRepository.Delete(expense);

        return Result.Success();
    }
}
