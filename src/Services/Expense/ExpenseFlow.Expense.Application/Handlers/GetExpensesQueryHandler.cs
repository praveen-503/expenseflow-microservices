using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using AutoMapper;
using ExpenseFlow.Expense.Domain.Common;
using ExpenseFlow.Expense.Domain.Interfaces;
using ExpenseFlow.Expense.Domain.Specifications;
using ExpenseFlow.Expense.Application.DTOs;
using ExpenseFlow.Expense.Application.Queries;

namespace ExpenseFlow.Expense.Application.Handlers;

public class GetExpensesQueryHandler : IRequestHandler<GetExpensesQuery, Result<IReadOnlyList<ExpenseDto>>>
{
    private readonly IExpenseRepository _expenseRepository;
    private readonly IMapper _mapper;

    public GetExpensesQueryHandler(IExpenseRepository expenseRepository, IMapper _mapper)
    {
        this._expenseRepository = expenseRepository;
        this._mapper = _mapper;
    }

    public async Task<Result<IReadOnlyList<ExpenseDto>>> Handle(GetExpensesQuery request, CancellationToken cancellationToken)
    {
        var spec = new ExpenseWithCategorySpecification(request.UserId);
        var expenses = await _expenseRepository.ListAsync(spec, cancellationToken);
        var dtos = _mapper.Map<IReadOnlyList<ExpenseDto>>(expenses);
        return Result<IReadOnlyList<ExpenseDto>>.Success(dtos);
    }
}
