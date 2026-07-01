using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using ExpenseFlow.Expense.Domain.Common;
using ExpenseFlow.Expense.Domain.Interfaces;
using ExpenseFlow.Expense.Application.Commands;

namespace ExpenseFlow.Expense.Application.Handlers;

public class DeleteExpenseCommandHandler : IRequestHandler<DeleteExpenseCommand, Result<bool>>
{
    private readonly IExpenseRepository _expenseRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteExpenseCommandHandler(IExpenseRepository expenseRepository, IUnitOfWork unitOfWork)
    {
        _expenseRepository = expenseRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<bool>> Handle(DeleteExpenseCommand request, CancellationToken cancellationToken)
    {
        var expense = await _expenseRepository.GetByIdAsync(request.Id, cancellationToken);
        if (expense == null || expense.UserId != request.UserId)
        {
            return Result<bool>.Failure(new Error("Expense.NotFound", "Expense not found."));
        }

        _expenseRepository.Delete(expense);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<bool>.Success(true);
    }
}
