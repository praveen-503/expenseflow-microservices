using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using AutoMapper;
using ExpenseFlow.Expense.Domain.Common;
using ExpenseFlow.Expense.Domain.Interfaces;
using ExpenseFlow.Expense.Application.DTOs;
using ExpenseFlow.Expense.Application.Queries;

namespace ExpenseFlow.Expense.Application.Handlers;

public class GetExpenseSummaryQueryHandler : IRequestHandler<GetExpenseSummaryQuery, Result<ExpenseSummaryDto>>
{
    private readonly IExpenseRepository _expenseRepository;
    private readonly IMapper _mapper;

    public GetExpenseSummaryQueryHandler(IExpenseRepository expenseRepository, IMapper mapper)
    {
        _expenseRepository = expenseRepository;
        _mapper = mapper;
    }

    public async Task<Result<ExpenseSummaryDto>> Handle(GetExpenseSummaryQuery request, CancellationToken cancellationToken)
    {
        var domainSummary = await _expenseRepository.GetExpenseSummaryAsync(request.UserId, cancellationToken);
        var dto = _mapper.Map<ExpenseSummaryDto>(domainSummary);
        return Result<ExpenseSummaryDto>.Success(dto);
    }
}
