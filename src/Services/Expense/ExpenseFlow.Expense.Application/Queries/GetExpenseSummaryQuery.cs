using MediatR;
using ExpenseFlow.Expense.Domain.Common;
using ExpenseFlow.Expense.Application.DTOs;
using System;

namespace ExpenseFlow.Expense.Application.Queries;

public record GetExpenseSummaryQuery(Guid UserId) : IRequest<Result<ExpenseSummaryDto>>;
