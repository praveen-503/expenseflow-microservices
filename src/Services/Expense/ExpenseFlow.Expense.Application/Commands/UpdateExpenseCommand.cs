using System;
using MediatR;
using ExpenseFlow.Expense.Domain.Common;

namespace ExpenseFlow.Expense.Application.Commands;

public record UpdateExpenseCommand(Guid Id, string Description, decimal Amount, DateTime Date, Guid CategoryId, Guid UserId) : IRequest<Result>;
