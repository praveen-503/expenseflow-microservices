using System;
using MediatR;
using ExpenseFlow.Expense.Domain.Common;
using ExpenseFlow.Expense.Application.DTOs;

namespace ExpenseFlow.Expense.Application.Commands;

public record CreateExpenseCommand(
    string Title,
    decimal Amount,
    DateTime ExpenseDate,
    string Notes,
    Guid CategoryId,
    Guid UserId) : IRequest<Result<Guid>>;
