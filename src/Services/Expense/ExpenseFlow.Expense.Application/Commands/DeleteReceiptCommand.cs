using MediatR;
using System;
using ExpenseFlow.Expense.Domain.Common;

namespace ExpenseFlow.Expense.Application.Commands;

public record DeleteReceiptCommand(Guid ExpenseId, Guid UserId) : IRequest<Result<bool>>;
