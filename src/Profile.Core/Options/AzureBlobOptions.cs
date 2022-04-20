using System.Collections.Generic;

namespace Profile.Core.Options
{
    public class AzureBlobOptions
    {
        public const string SectionName = "StorageAccount";

        public string ConnectionString { get; set; }
        public string AvatarContainerName { get; set; }
        public int MaxFileSize { get; set; }
        public List<string> AllowedExtensions { get; set; }
    }
}
