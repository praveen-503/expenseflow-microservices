using System;
using MediatR;
using ExpenseFlow.Expense.Domain.Common;
using ExpenseFlow.Expense.Application.DTOs;

namespace ExpenseFlow.Expense.Application.Commands;

public record CreateExpenseCommand(string Description, decimal Amount, DateTime Date, Guid CategoryId, Guid UserId) : IRequest<Result<Guid>>;
