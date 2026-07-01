using System;
using MediatR;
using ExpenseFlow.Expense.Domain.Common;
using ExpenseFlow.Expense.Application.DTOs;

namespace ExpenseFlow.Expense.Application.Queries;

public record GetExpenseByIdQuery(Guid Id, Guid UserId) : IRequest<Result<ExpenseDto>>;
