using System.IO;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Options;
using Profile.Core.Options;

namespace Profile.Core.FileManager
{
    public class AvatarStorage : IAvatarStorage
    {
        private readonly BlobContainerClient _blobContainerClient;

        public AvatarStorage(BlobServiceClient blobServiceClient, IOptions<AzureBlobOptions> azureBlobOptions)
        {
            _blobContainerClient = blobServiceClient.GetBlobContainerClient(azureBlobOptions.Value.AvatarContainerName);
            _blobContainerClient.CreateIfNotExists(Azure.Storage.Blobs.Models.PublicAccessType.Blob);
        }

        public async Task<string> Upload(byte[] file, string fileName)
        {
            var blobClient = _blobContainerClient.GetBlobClient(fileName);

            using var ms = new MemoryStream(file, false);
            await blobClient.UploadAsync(ms);

            return blobClient.Uri.AbsoluteUri;
        }

        public async Task Delete(string fileName)
        {
            var blobClient = _blobContainerClient.GetBlobClient(fileName);
            await blobClient.DeleteIfExistsAsync();
        }
    }
}