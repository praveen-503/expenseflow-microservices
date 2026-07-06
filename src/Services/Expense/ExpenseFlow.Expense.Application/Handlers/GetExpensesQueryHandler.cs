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
using ExpenseFlow.Expense.Application.Interfaces;

namespace ExpenseFlow.Expense.Application.Handlers;

public class GetExpensesQueryHandler : IRequestHandler<GetExpensesQuery, Result<IReadOnlyList<ExpenseDto>>>
{
    private readonly IExpenseRepository _expenseRepository;
    private readonly IMapper _mapper;
    private readonly IStorageService _storageService;

    public GetExpensesQueryHandler(IExpenseRepository expenseRepository, IMapper _mapper, IStorageService storageService)
    {
        this._expenseRepository = expenseRepository;
        this._mapper = _mapper;
        this._storageService = storageService;
    }

    public async Task<Result<IReadOnlyList<ExpenseDto>>> Handle(GetExpensesQuery request, CancellationToken cancellationToken)
    {
        var spec = new ExpenseWithCategorySpecification(request.UserId);
        var expenses = await _expenseRepository.ListAsync(spec, cancellationToken);
        var dtos = _mapper.Map<IReadOnlyList<ExpenseDto>>(expenses);

        // Convert storage blob URLs into temporary download SAS URIs
        var mappedDtos = System.Linq.Enumerable.ToList(
            System.Linq.Enumerable.Select(dtos, d => !string.IsNullOrEmpty(d.ReceiptUrl)
                ? d with { ReceiptUrl = _storageService.GenerateSasUrl(d.ReceiptUrl, TimeSpan.FromHours(1)) }
                : d)
        ).AsReadOnly();

        return Result<IReadOnlyList<ExpenseDto>>.Success(mappedDtos);
    }
}
