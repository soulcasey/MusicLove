using System.Reflection.Metadata;
using Azure.Storage.Blobs;

namespace MusicLove.Azure;

public class AzureStorage : IAzureStorage
{
    private readonly BlobServiceClient blobServiceClient;

    public AzureStorage(IConfiguration configuration)
    {
        blobServiceClient = new BlobServiceClient(configuration.GetConnectionString(Define.ConnectionString.BLOB));
    }

    public async Task UploadFile(IFormFile file, string blobName)
    {
        BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(Define.Azure.BLOB_CONTAINER);
        BlobClient blobClient = containerClient.GetBlobClient(blobName);

        Stream stream = file.OpenReadStream();

        await blobClient.UploadAsync(stream, true);
    }

    public async Task DeleteFile(string blobName)
    {
        BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(Define.Azure.BLOB_CONTAINER);

        bool deleted = await containerClient.DeleteBlobIfExistsAsync(blobName);

        if (deleted == true)
        {
            Console.WriteLine(blobName + " Deleted Successfully");
        }
        else
        {
            Console.WriteLine(blobName + " Delete Fail");
        }
    }

    public async Task<bool> Exists(string blobName)
    {
        BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(Define.Azure.BLOB_CONTAINER);
        BlobClient blobClient = containerClient.GetBlobClient(blobName);

        return await blobClient.ExistsAsync();
    }
}