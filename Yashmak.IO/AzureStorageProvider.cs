namespace Yashmak.IO
{
    using System.IO;

    using Microsoft.Azure;
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Blob;

    public class AzureStorageProvider : IStorageProvider
    {
        private readonly CloudBlobClient blobClient;

        public AzureStorageProvider()
        {
            var connectionString = CloudConfigurationManager.GetSetting("StorageConnectionString");
            var storageAccount = CloudStorageAccount.Parse(connectionString);
            this.blobClient = storageAccount.CreateCloudBlobClient();
        }

        public Stream StreamFile(string userId, string filename)
        {
            var container = this.blobClient.GetContainerReference(userId.Replace("-", string.Empty));
            return container.GetBlockBlobReference(filename).OpenRead();
        }

        public string UploadStream(string userId, string filename, Stream stream)
        {
            var container = this.blobClient.GetContainerReference(userId.Replace("-", string.Empty));

            container.CreateIfNotExistsAsync();
            var blockBlob = container.GetBlockBlobReference(filename);

            blockBlob.UploadFromStream(stream);

            return blockBlob.StorageUri.PrimaryUri.ToString();
        }
    }
}