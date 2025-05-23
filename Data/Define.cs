using System;
using System.Text.RegularExpressions;

namespace MusicLove;

public static class Define
{
    public static class Toastr
    {
        public const string SUCCESS = "Success";
        public const string ERROR = "Error";
    }

    public static class Role
    {
        public const string ADMIN = "Admin";
        public const string CUSTOMER = "Customer";
    }

    public static class ConnectionString
    {
        public const string AZURE_BLOB = "AzureBlob";
        public const string AIVEN_MYSQL = "AivenMySQL";
    }

    public static class EnvironmentVariable
    {
        public const string ADMIN_KEY = "AdminKey";
    }

    public static class Azure
    {
        public const int FILE_SIZE_LIMIT =  5 * 1024 * 1024;
        public static readonly string[] IMAGE_FILE_FORMATS = { ".png", ".jpg", ".jpeg", ".gif" };
        public const string BLOB_CONTAINER = "musiclove";
        public const string BLOB_URL = "https://musiclovestorage.blob.core.windows.net/musiclove/";
        public const string DEFAULT_IMAGE = "/images/NoImageAvailable.png";
    }

    public static class Youtube
    {
        public static bool TryGetId(string videoUrl, out string id)
        {
            string pattern = @"(?:https?:\/\/)?(?:www\.)?(?:youtube\.com\/(?:[^\/\n\s]+\/\S+\/|(?:v|e(?:mbed)?)\/|\S*?[?&]v=)|youtu\.be\/)([a-zA-Z0-9_-]{11})";
            Match match = Regex.Match(videoUrl, pattern);

            id = match.Success ? match.Groups[1].Value : string.Empty;
            return match.Success;
        }

        public const string EMBEDED_LINK = "https://www.youtube.com/embed/";
        public static string GetThumbnail(string id)
        {
            return "https://img.youtube.com/vi/" + id + "/sddefault.jpg";
        }
    }

    public static class File
    {
        public static bool IsImage(string fileName)
        {
            var imageExtensions = new List<string> { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".tiff" };
            string fileExtension = Path.GetExtension(fileName)?.ToLower();
            return imageExtensions.Contains(fileExtension);
        }
    }
}