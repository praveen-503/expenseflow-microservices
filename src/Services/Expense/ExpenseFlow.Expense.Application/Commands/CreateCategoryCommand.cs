using System;
using MediatR;
using ExpenseFlow.Expense.Domain.Common;

namespace ExpenseFlow.Expense.Application.Commands;

public record CreateCategoryCommand(string Name, string Description) : IRequest<Result<Guid>>;
