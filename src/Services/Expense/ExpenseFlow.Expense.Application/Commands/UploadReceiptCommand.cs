using MediatR;
using System;
using System.IO;
using ExpenseFlow.Expense.Domain.Common;

namespace ExpenseFlow.Expense.Application.Commands;

public record UploadReceiptCommand(
    Guid ExpenseId, 
    Guid UserId, 
    string FileName, 
    string ContentType, 
    Stream FileStream) : IRequest<Result<string>>;
