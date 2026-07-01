using System.Collections.Generic;
using MediatR;
using ExpenseFlow.Expense.Domain.Common;
using ExpenseFlow.Expense.Application.DTOs;
using System;

namespace ExpenseFlow.Expense.Application.Queries;

public record GetCategoriesQuery(Guid UserId) : IRequest<Result<IReadOnlyList<CategoryDto>>>;
