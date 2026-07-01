using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using ExpenseFlow.Expense.Domain.Common;
using ExpenseFlow.Expense.Domain.Entities;
using ExpenseFlow.Expense.Domain.Interfaces;
using ExpenseFlow.Expense.Application.Commands;

namespace ExpenseFlow.Expense.Application.Handlers;

public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, Result<Guid>>
{
    private readonly ICategoryRepository _categoryRepository;

    public CreateCategoryCommandHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<Result<Guid>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = new Category
        {
            Name = request.Name,
            Description = request.Description
        };

        await _categoryRepository.AddAsync(category, cancellationToken);

        return Result<Guid>.Success(category.Id);
    }
}
