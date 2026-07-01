using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using ExpenseFlow.Expense.Domain.Common;
using ExpenseFlow.Expense.Domain.Interfaces;
using ExpenseFlow.Expense.Domain.Specifications;
using ExpenseFlow.Expense.Application.DTOs;
using ExpenseFlow.Expense.Application.Queries;

namespace ExpenseFlow.Expense.Application.Handlers;

public class GetExpenseByIdQueryHandler : IRequestHandler<GetExpenseByIdQuery, Result<ExpenseDto>>
{
    private readonly IExpenseRepository _expenseRepository;

    public GetExpenseByIdQueryHandler(IExpenseRepository expenseRepository)
    {
        _expenseRepository = expenseRepository;
    }

    public async Task<Result<ExpenseDto>> Handle(GetExpenseByIdQuery request, CancellationToken cancellationToken)
    {
        var spec = new ExpenseByIdWithCategorySpecification(request.Id, request.UserId);
        var expenses = await _expenseRepository.ListAsync(spec, cancellationToken);
        var expense = expenses.FirstOrDefault();

        if (expense == null)
        {
            return Result<ExpenseDto>.Failure(new Error("Expense.NotFound", "Expense not found."));
        }

        var dto = new ExpenseDto(
            expense.Id,
            expense.Description,
            expense.Amount,
            expense.Date,
            expense.CategoryId,
            new CategoryDto(expense.Category.Id, expense.Category.Name, expense.Category.Description),
            expense.UserId
        );

        return Result<ExpenseDto>.Success(dto);
    }
}
