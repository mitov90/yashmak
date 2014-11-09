namespace Yashmak.Common
{
    using System.IO;

    public static class FileSystemHelper
    {
        private const string DataFolder = "/App_Data/";

        public static bool CreateFolder(string path, string newFolderName)
        {
            string newDirPath = path + "/" + newFolderName;

            bool exists = Directory.Exists(newDirPath);
            if (!exists)
            {
                Directory.CreateDirectory(newDirPath);
            }

            return Directory.Exists(newDirPath);
        }

        public static bool CreateUserFolder(string path, string newFolderName)
        {
            string newDirPath = path + DataFolder + newFolderName;

            bool exists = Directory.Exists(newDirPath);
            if (!exists)
            {
                Directory.CreateDirectory(newDirPath);
            }

            return Directory.Exists(newDirPath);
        }
    }
}