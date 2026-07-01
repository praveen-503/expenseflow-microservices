using System;
using MediatR;
using ExpenseFlow.Expense.Domain.Common;

namespace ExpenseFlow.Expense.Application.Commands;

public record UpdateExpenseCommand(
    Guid Id,
    string Title,
    decimal Amount,
    DateTime ExpenseDate,
    string Notes,
    Guid CategoryId,
    Guid UserId) : IRequest<Result<bool>>;
