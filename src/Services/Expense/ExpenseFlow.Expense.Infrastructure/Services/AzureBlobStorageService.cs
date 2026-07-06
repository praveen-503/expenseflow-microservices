using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using ExpenseFlow.Expense.Application.Interfaces;

namespace ExpenseFlow.Expense.Infrastructure.Services;

public class AzureBlobStorageService : IStorageService
{
    private readonly BlobServiceClient _blobServiceClient;
    private readonly BlobStorageOptions _options;
    private readonly ILogger<AzureBlobStorageService> _logger;

    public AzureBlobStorageService(
        BlobServiceClient blobServiceClient,
        IOptions<BlobStorageOptions> options,
        ILogger<AzureBlobStorageService> logger)
    {
        _blobServiceClient = blobServiceClient;
        _options = options.Value;
        _logger = logger;
    }

    public async Task<string> UploadFileAsync(Stream fileStream, string fileName, string contentType, CancellationToken cancellationToken = default)
    {
        try
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(_options.ContainerName);
            
            // Create container if not exists with private access level
            await containerClient.CreateIfNotExistsAsync(PublicAccessType.None, cancellationToken: cancellationToken);

            // Generate unique blob name to prevent collisions
            var uniqueBlobName = $"{Guid.NewGuid()}{Path.GetExtension(fileName)}";
            var blobClient = containerClient.GetBlobClient(uniqueBlobName);

            _logger.LogInformation("Uploading file {FileName} as blob {BlobName} to container {ContainerName}...", 
                fileName, uniqueBlobName, _options.ContainerName);

            var httpHeaders = new BlobHttpHeaders { ContentType = contentType };
            
            await blobClient.UploadAsync(fileStream, new BlobUploadOptions { HttpHeaders = httpHeaders }, cancellationToken);

            _logger.LogInformation("Successfully uploaded blob {BlobName}.", uniqueBlobName);

            return blobClient.Uri.ToString();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to upload file {FileName} to Azure Blob Storage.", fileName);
            throw;
        }
    }

    public async Task DeleteFileAsync(string fileUrl, CancellationToken cancellationToken = default)
    {
        try
        {
            if (string.IsNullOrEmpty(fileUrl)) return;

            var blobName = GetBlobNameFromUrl(fileUrl);
            var containerClient = _blobServiceClient.GetBlobContainerClient(_options.ContainerName);
            var blobClient = containerClient.GetBlobClient(blobName);

            _logger.LogInformation("Deleting blob {BlobName} from container {ContainerName}...", blobName, _options.ContainerName);

            await blobClient.DeleteIfExistsAsync(DeleteSnapshotsOption.IncludeSnapshots, cancellationToken: cancellationToken);

            _logger.LogInformation("Successfully deleted blob {BlobName}.", blobName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to delete file from URL {FileUrl}.", fileUrl);
        }
    }

    public string GenerateSasUrl(string fileUrl, TimeSpan expiry)
    {
        try
        {
            if (string.IsNullOrEmpty(fileUrl)) return string.Empty;

            var blobName = GetBlobNameFromUrl(fileUrl);
            var containerClient = _blobServiceClient.GetBlobContainerClient(_options.ContainerName);
            var blobClient = containerClient.GetBlobClient(blobName);

            // Construct SAS token
            var sasBuilder = new BlobSasBuilder
            {
                BlobContainerName = _options.ContainerName,
                BlobName = blobName,
                Resource = "b", // b for blob
                ExpiresOn = DateTimeOffset.UtcNow.Add(expiry)
            };

            sasBuilder.SetPermissions(BlobSasPermissions.Read);

            // Generate SAS Uri
            var sasUri = blobClient.GenerateSasUri(sasBuilder);
            return sasUri.ToString();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to generate SAS URI for URL {FileUrl}.", fileUrl);
            return fileUrl; // Return raw fileUrl as fallback
        }
    }

    private string GetBlobNameFromUrl(string fileUrl)
    {
        var uri = new Uri(fileUrl);
        var pathSegments = uri.AbsolutePath.Split('/', StringSplitOptions.RemoveEmptyEntries);
        if (pathSegments.Length >= 2)
        {
            return string.Join('/', pathSegments[1..]);
        }
        return Path.GetFileName(uri.LocalPath);
    }
}
