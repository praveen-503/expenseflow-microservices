using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using ExpenseFlow.Expense.Domain.Common;
using ExpenseFlow.Expense.Domain.Entities;
using ExpenseFlow.Expense.Domain.Interfaces;
using ExpenseFlow.Expense.Application.Commands;

namespace ExpenseFlow.Expense.Application.Handlers;

public class UpdateExpenseCommandHandler : IRequestHandler<UpdateExpenseCommand, Result<bool>>
{
    private readonly IExpenseRepository _expenseRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateExpenseCommandHandler(
        IExpenseRepository expenseRepository,
        ICategoryRepository categoryRepository,
        IUnitOfWork unitOfWork)
    {
        _expenseRepository = expenseRepository;
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<bool>> Handle(UpdateExpenseCommand request, CancellationToken cancellationToken)
    {
        var expense = await _expenseRepository.GetByIdAsync(request.Id, cancellationToken);
        if (expense == null || expense.UserId != request.UserId)
        {
            return Result<bool>.Failure(new Error("Expense.NotFound", "Expense not found."));
        }

        var category = await _categoryRepository.GetByIdAsync(request.CategoryId, cancellationToken);
        if (category == null)
        {
            return Result<bool>.Failure(new Error("Expense.CategoryNotFound", "Selected category not found."));
        }

        expense.Title = request.Title;
        expense.Amount = request.Amount;
        expense.ExpenseDate = request.ExpenseDate;
        expense.Notes = request.Notes;
        expense.CategoryId = request.CategoryId;

        _expenseRepository.Update(expense);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<bool>.Success(true);
    }
}
