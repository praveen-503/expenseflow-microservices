using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using ExpenseFlow.Expense.Domain.Common;
using ExpenseFlow.Expense.Domain.Interfaces;
using ExpenseFlow.Expense.Application.Commands;
using ExpenseFlow.Expense.Application.Interfaces;

namespace ExpenseFlow.Expense.Application.Handlers;

public class DeleteReceiptCommandHandler : IRequestHandler<DeleteReceiptCommand, Result<bool>>
{
    private readonly IExpenseRepository _expenseRepository;
    private readonly IStorageService _storageService;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteReceiptCommandHandler(
        IExpenseRepository expenseRepository,
        IStorageService storageService,
        IUnitOfWork unitOfWork)
    {
        _expenseRepository = expenseRepository;
        _storageService = storageService;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<bool>> Handle(DeleteReceiptCommand request, CancellationToken cancellationToken)
    {
        var expense = await _expenseRepository.GetByIdAsync(request.ExpenseId, cancellationToken);
        if (expense == null)
        {
            return Result<bool>.Failure(new Error("Expense.NotFound", "Expense not found."));
        }

        if (expense.UserId != request.UserId)
        {
            return Result<bool>.Failure(new Error("Expense.Forbidden", "You do not have permission to modify this expense."));
        }

        if (!string.IsNullOrEmpty(expense.ReceiptUrl))
        {
            // Delete blob from Azure Storage
            await _storageService.DeleteFileAsync(expense.ReceiptUrl, cancellationToken);

            // Clear database reference
            expense.ReceiptUrl = null;

            _expenseRepository.Update(expense);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        return Result<bool>.Success(true);
    }
}
