using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using RookiEcom.Application.Common;
using RookiEcom.Application.Storage;

namespace RookiEcom.Infrastructure.Storage;

public class BlobService : IBlobService
{
    private readonly BlobServiceClient _blobService;
    
    public BlobService(string connectionString)
    {
        _blobService = new BlobServiceClient(connectionString);
    }
    
    public async Task<string> GetBlob(string blobName, string containerName)
    {
        if (string.IsNullOrEmpty(blobName) || string.IsNullOrEmpty(containerName))
        {
            throw new ArgumentException("Blob name and container name must not be empty.");
        }
        
        var containerClient = await GetContainerClientAsync(containerName);
        var blobClient = containerClient.GetBlobClient(blobName);

        if (!await blobClient.ExistsAsync())
        {
            return string.Empty;
        }

        return blobClient.Uri.AbsoluteUri;
    }

    public async Task<bool> DeleteBlob(string blobName, string containerName)
    {
        BlobContainerClient client = _blobService.GetBlobContainerClient(containerName);
        BlobClient blobClient = client.GetBlobClient(blobName);

        return await blobClient.DeleteIfExistsAsync();
    }

    public async Task<string> UploadBlob(string blobName, string containerName, IFormFile file)
    {
        if (string.IsNullOrEmpty(blobName) || string.IsNullOrEmpty(containerName))
        {
            throw new ArgumentException("Blob name and container name must not be empty.");
        }
        
        if (file == null || file.Content == null)
        {
            throw new ArgumentException("File must not be null or empty.");
        }

        BlobContainerClient clientContainer = await GetContainerClientAsync(containerName);
        BlobClient blobClient = clientContainer.GetBlobClient(blobName);

        var httpHeaders = new BlobHttpHeaders()
        {
            ContentType = file.ContentType
        };
        
        var result = await blobClient.UploadAsync(file.Content,httpHeaders);
        if (result != null)
        {
            return await GetBlob(blobName, containerName);
        }

        return "";
    }
    
    private async Task<BlobContainerClient> GetContainerClientAsync(string containerName)
    {
        var containerClient = _blobService.GetBlobContainerClient(containerName);
        await containerClient.CreateIfNotExistsAsync(PublicAccessType.None);
        return containerClient;
    }
}
