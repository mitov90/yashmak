namespace Yashmak.Common
{
    using System.IO;

    public static class FileSystemHelper
    {
        public static bool CreateUserFolder(string mapPath, string newFolderName)
        {
            string dirPath = mapPath + Constants.UserFilesPath + newFolderName;

            bool exists = Directory.Exists(dirPath);
            if (!exists)
            {
                Directory.CreateDirectory(dirPath);
            }

            return Directory.Exists(dirPath);
        }
    }
}