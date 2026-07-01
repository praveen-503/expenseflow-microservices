using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using ExpenseFlow.Expense.Domain.Common;
using ExpenseFlow.Expense.Domain.Interfaces;
using ExpenseFlow.Expense.Application.DTOs;
using ExpenseFlow.Expense.Application.Queries;

namespace ExpenseFlow.Expense.Application.Handlers;

public class GetCategoriesQueryHandler : IRequestHandler<GetCategoriesQuery, Result<IEnumerable<CategoryDto>>>
{
    private readonly ICategoryRepository _categoryRepository;

    public GetCategoriesQueryHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<Result<IEnumerable<CategoryDto>>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
    {
        var categories = await _categoryRepository.ListAllAsync(cancellationToken);

        var dtos = categories.Select(c => new CategoryDto(
            c.Id,
            c.Name,
            c.Description
        ));

        return Result<IEnumerable<CategoryDto>>.Success(dtos);
    }
}
