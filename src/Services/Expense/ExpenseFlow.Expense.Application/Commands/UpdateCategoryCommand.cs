using MediatR;
using ExpenseFlow.Expense.Domain.Common;
using System;

namespace ExpenseFlow.Expense.Application.Commands;

public record UpdateCategoryCommand(Guid Id, string Name, string Description, Guid UserId) : IRequest<Result<bool>>;
