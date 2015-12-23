using System.IO;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Yashmak.IO
{
    public class AzureStorageProvider : IStorageProvider
    {
        private readonly CloudBlobClient _blobClient;

        public AzureStorageProvider()
        {
            var connectionString = CloudConfigurationManager.GetSetting("StorageConnectionString");
            var storageAccount = CloudStorageAccount.Parse(connectionString);
            _blobClient = storageAccount.CreateCloudBlobClient();
        }

        public string UploadStream(string userId, string filename, Stream stream)
        {
            var container = _blobClient.GetContainerReference(userId.Replace("-", string.Empty));

            container.CreateIfNotExistsAsync();
            var blockBlob = container.GetBlockBlobReference(filename);

            blockBlob.UploadFromStream(stream);

            return blockBlob.StorageUri.PrimaryUri.ToString();
        }

        public Stream StreamFile(string userId, string filename)
        {
            var container = _blobClient.GetContainerReference(userId.Replace("-", string.Empty));
            return container.GetBlockBlobReference(filename).OpenRead();
        }
    }
}