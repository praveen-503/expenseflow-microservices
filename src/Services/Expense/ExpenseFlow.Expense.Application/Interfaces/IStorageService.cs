using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace ExpenseFlow.Expense.Application.Interfaces;

public interface IStorageService
{
    Task<string> UploadFileAsync(Stream fileStream, string fileName, string contentType, CancellationToken cancellationToken = default);
    Task DeleteFileAsync(string fileUrl, CancellationToken cancellationToken = default);
    string GenerateSasUrl(string fileUrl, TimeSpan expiry);
}
