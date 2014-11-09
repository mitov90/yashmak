namespace Yashmak.Utilities.Tests
{
    using System;
    using System.IO;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Yashmak.Common;

    [TestClass]
    public class FileSystemTests
    {
        [TestMethod]
        public void UserFolder_OnCreating_ShouldCreate()
        {
            const string Username = "test@test.test";

            if (Directory.Exists(Username))
            {
                Directory.Delete(Username);
            }

            var actual = FileSystemHelper.CreateUserFolder(Username);

            Assert.AreEqual(true, actual);
        }
        [TestMethod]
        public void Folder_OnCreating_ShouldCreate()
        {
            const string Username = "test@test.test";

            var actual = FileSystemHelper.CreateFolder(Environment.CurrentDirectory, Username);

            Assert.AreEqual(true, actual);
        }
    }
}