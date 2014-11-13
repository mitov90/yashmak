namespace Yashmak.Common
{
    using System.IO;

    public static class FileSystemHelper
    {
        public const string DataFolder = "/App_Data/";

        public static bool CreateUserFolder(string mapPath, string newFolderName)
        {
            string dirPath = mapPath + DataFolder + newFolderName;

            bool exists = Directory.Exists(dirPath);
            if (!exists)
            {
                Directory.CreateDirectory(dirPath);
            }

            return Directory.Exists(dirPath);
        }
    }
}