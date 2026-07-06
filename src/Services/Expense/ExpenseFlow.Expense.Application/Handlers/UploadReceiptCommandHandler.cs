using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using ExpenseFlow.Expense.Domain.Common;
using ExpenseFlow.Expense.Domain.Interfaces;
using ExpenseFlow.Expense.Application.Commands;
using ExpenseFlow.Expense.Application.Interfaces;

namespace ExpenseFlow.Expense.Application.Handlers;

public class UploadReceiptCommandHandler : IRequestHandler<UploadReceiptCommand, Result<string>>
{
    private readonly IExpenseRepository _expenseRepository;
    private readonly IStorageService _storageService;
    private readonly IUnitOfWork _unitOfWork;

    public UploadReceiptCommandHandler(
        IExpenseRepository expenseRepository,
        IStorageService storageService,
        IUnitOfWork unitOfWork)
    {
        _expenseRepository = expenseRepository;
        _storageService = storageService;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<string>> Handle(UploadReceiptCommand request, CancellationToken cancellationToken)
    {
        var expense = await _expenseRepository.GetByIdAsync(request.ExpenseId, cancellationToken);
        if (expense == null)
        {
            return Result<string>.Failure(new Error("Expense.NotFound", "Expense not found."));
        }

        if (expense.UserId != request.UserId)
        {
            return Result<string>.Failure(new Error("Expense.Forbidden", "You do not have permission to modify this expense."));
        }

        // Clean up previous receipt from storage if exists
        if (!string.IsNullOrEmpty(expense.ReceiptUrl))
        {
            await _storageService.DeleteFileAsync(expense.ReceiptUrl, cancellationToken);
        }

        // Upload new receipt to Azure Blob Storage
        var blobUrl = await _storageService.UploadFileAsync(
            request.FileStream, 
            request.FileName, 
            request.ContentType, 
            cancellationToken);

        // Update database record
        expense.ReceiptUrl = blobUrl;
        
        _expenseRepository.Update(expense);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<string>.Success(blobUrl);
    }
}
