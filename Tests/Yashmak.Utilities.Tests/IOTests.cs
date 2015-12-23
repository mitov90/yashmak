
using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Yashmak.IO;

namespace Yashmak.Utilities.Tests
{
    [TestClass]
    public class IoTests
    {
        [TestMethod]
        public void UploadToBlob()
        {
            const string filename = "EntityFramework.xml";
            string username = new Guid().ToString();
            var storage = new AzureStorageProvider();
            var uploadStream = File.OpenRead(filename);

            storage.UploadStream(username, filename, uploadStream);
            var downloadStream = storage.StreamFile(username, filename);
            Assert.AreEqual(uploadStream.Length, downloadStream.Length);

        }
    }
}
