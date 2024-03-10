using System;

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
        public const string SQL_SERVER = "SqlServer";
        public const string AZURE_BLOB = "AzureBlob";
        public const string AZURE_SQL = "AzureSQL";
    }

    public static class Azure
    {
        public const string BLOB_CONTAINER = "musiclove";
        public const string BLOB_URL = "https://caseycha.blob.core.windows.net/musiclove/";
        public const string DEFAULT_IMAGE = "/images/NoImageAvailable.png";
    }
}