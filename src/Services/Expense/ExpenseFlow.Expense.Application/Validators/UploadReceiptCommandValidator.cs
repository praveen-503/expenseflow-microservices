using FluentValidation;
using ExpenseFlow.Expense.Application.Commands;
using System.IO;
using System.Linq;
using System;

namespace ExpenseFlow.Expense.Application.Validators;

public class UploadReceiptCommandValidator : AbstractValidator<UploadReceiptCommand>
{
    private static readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png", ".pdf" };
    private static readonly string[] AllowedMimeTypes = { "image/jpeg", "image/png", "application/pdf" };
    private const long MaxFileSizeInBytes = 5 * 1024 * 1024; // 5 MB

    public UploadReceiptCommandValidator()
    {
        RuleFor(x => x.ExpenseId).NotEmpty().WithMessage("Expense ID is required.");
        RuleFor(x => x.UserId).NotEmpty().WithMessage("User ID is required.");
        
        RuleFor(x => x.FileName)
            .NotEmpty().WithMessage("File name is required.")
            .Must(IsValidExtension).WithMessage("Only image (.jpg, .jpeg, .png) or PDF (.pdf) files are allowed.");

        RuleFor(x => x.ContentType)
            .NotEmpty().WithMessage("Content type is required.")
            .Must(IsValidMimeType).WithMessage("Only image (jpeg, png) or PDF MIME types are allowed.");

        RuleFor(x => x.FileStream)
            .NotNull().WithMessage("File content is required.")
            .Must(stream => stream != null && stream.Length <= MaxFileSizeInBytes)
            .WithMessage("File size cannot exceed 5 MB.");
    }

    private bool IsValidExtension(string fileName)
    {
        if (string.IsNullOrEmpty(fileName)) return false;
        var ext = Path.GetExtension(fileName);
        if (string.IsNullOrEmpty(ext)) return false;
        return AllowedExtensions.Contains(ext.ToLowerInvariant());
    }

    private bool IsValidMimeType(string contentType)
    {
        if (string.IsNullOrEmpty(contentType)) return false;
        return AllowedMimeTypes.Contains(contentType.ToLowerInvariant());
    }
}
