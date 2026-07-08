using AutoMapper;
using ExpenseFlow.Expense.Domain.Entities;
using ExpenseFlow.Expense.Domain.Common;
using ExpenseFlow.Expense.Application.DTOs;

namespace ExpenseFlow.Expense.Application.Common;

public class ExpenseMappingProfile : Profile
{
    public ExpenseMappingProfile()
    {
        CreateMap<Category, CategoryDto>();
        CreateMap<Domain.Entities.Expense, ExpenseDto>();
        CreateMap<CategorySummary, CategorySummaryDto>();
        CreateMap<ExpenseSummary, ExpenseSummaryDto>();
    }
}
