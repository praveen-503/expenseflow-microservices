using System;
using MediatR;
using ExpenseFlow.Expense.Domain.Common;

namespace ExpenseFlow.Expense.Application.Commands;

public record DeleteExpenseCommand(Guid Id, Guid UserId) : IRequest<Result>;
