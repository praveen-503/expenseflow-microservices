using MediatR;
using ExpenseFlow.Expense.Domain.Common;
using System;

namespace ExpenseFlow.Expense.Application.Commands;

public record DeleteCategoryCommand(Guid Id, Guid UserId) : IRequest<Result<bool>>;
