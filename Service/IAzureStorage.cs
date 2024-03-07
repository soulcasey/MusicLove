namespace MusicLove.Azure;

public interface IAzureStorage
{
    public Task UploadFile(IFormFile file, string blobName);
}