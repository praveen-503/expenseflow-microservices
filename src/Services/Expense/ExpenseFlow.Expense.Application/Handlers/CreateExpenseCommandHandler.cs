using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using ExpenseFlow.Expense.Domain.Common;
using ExpenseFlow.Expense.Domain.Entities;
using ExpenseFlow.Expense.Domain.Interfaces;
using ExpenseFlow.Expense.Application.Commands;

namespace ExpenseFlow.Expense.Application.Handlers;

public class CreateExpenseCommandHandler : IRequestHandler<CreateExpenseCommand, Result<Guid>>
{
    private readonly IExpenseRepository _expenseRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateExpenseCommandHandler(
        IExpenseRepository expenseRepository,
        ICategoryRepository categoryRepository,
        IUnitOfWork unitOfWork)
    {
        _expenseRepository = expenseRepository;
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(CreateExpenseCommand request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByIdAsync(request.CategoryId, cancellationToken);
        if (category == null)
        {
            return Result<Guid>.Failure(new Error("Category.NotFound", "Category not found."));
        }

        var expense = new Domain.Entities.Expense
        {
            Description = request.Description,
            Amount = request.Amount,
            Date = request.Date,
            CategoryId = request.CategoryId,
            UserId = request.UserId
        };

        await _expenseRepository.AddAsync(expense, cancellationToken);
        // TransactionBehavior commits transaction automatically for Commands

        return Result<Guid>.Success(expense.Id);
    }
}
