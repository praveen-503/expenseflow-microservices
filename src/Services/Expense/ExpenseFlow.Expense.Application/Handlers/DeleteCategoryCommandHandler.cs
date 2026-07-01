using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using ExpenseFlow.Expense.Domain.Common;
using ExpenseFlow.Expense.Domain.Interfaces;
using ExpenseFlow.Expense.Application.Commands;

namespace ExpenseFlow.Expense.Application.Handlers;

public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, Result<bool>>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IExpenseRepository _expenseRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteCategoryCommandHandler(
        ICategoryRepository categoryRepository,
        IExpenseRepository expenseRepository,
        IUnitOfWork unitOfWork)
    {
        _categoryRepository = categoryRepository;
        _expenseRepository = expenseRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<bool>> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByIdAsync(request.Id, cancellationToken);
        if (category == null || category.UserId != request.UserId)
        {
            return Result<bool>.Failure(new Error("Category.NotFound", "Category not found or access denied."));
        }

        var hasExpenses = await _expenseRepository.HasExpensesWithCategoryAsync(request.Id, cancellationToken);
        if (hasExpenses)
        {
            return Result<bool>.Failure(new Error("Category.HasAssociatedExpenses", "Cannot delete category because it has associated expenses."));
        }

        _categoryRepository.Delete(category);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<bool>.Success(true);
    }
}
