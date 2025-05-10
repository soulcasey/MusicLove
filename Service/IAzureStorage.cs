namespace MusicLove.Azure;

public interface IAzureStorage
{
    public Task<bool> Exists(string blobName);
    public Task DeleteFile(string blobName);
    public Task UploadFile(IFormFile file, string blobName);
}