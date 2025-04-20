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
        BlobContainerClient client = _blobService.GetBlobContainerClient(containerName);
        BlobClient blobClient = client.GetBlobClient(blobName);

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
        BlobContainerClient client = _blobService.GetBlobContainerClient(containerName);
        BlobClient blobClient = client.GetBlobClient(blobName);

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
}
