using System;
using System.Collections.Generic;
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

public class GetExpensesQueryHandler : IRequestHandler<GetExpensesQuery, Result<IEnumerable<ExpenseDto>>>
{
    private readonly IExpenseRepository _expenseRepository;

    public GetExpensesQueryHandler(IExpenseRepository expenseRepository)
    {
        _expenseRepository = expenseRepository;
    }

    public async Task<Result<IEnumerable<ExpenseDto>>> Handle(GetExpensesQuery request, CancellationToken cancellationToken)
    {
        var spec = new ExpenseWithCategorySpecification(request.UserId);
        var expenses = await _expenseRepository.ListAsync(spec, cancellationToken);

        var dtos = expenses.Select(e => new ExpenseDto(
            e.Id,
            e.Description,
            e.Amount,
            e.Date,
            e.CategoryId,
            new CategoryDto(e.Category.Id, e.Category.Name, e.Category.Description),
            e.UserId
        ));

        return Result<IEnumerable<ExpenseDto>>.Success(dtos);
    }
}
