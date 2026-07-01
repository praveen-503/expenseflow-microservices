using System;
using System.Collections.Generic;
using MediatR;
using ExpenseFlow.Expense.Domain.Common;
using ExpenseFlow.Expense.Application.DTOs;

namespace ExpenseFlow.Expense.Application.Queries;

public record GetExpensesQuery(Guid UserId) : IRequest<Result<IReadOnlyList<ExpenseDto>>>;
