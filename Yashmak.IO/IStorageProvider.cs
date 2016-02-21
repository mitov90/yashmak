namespace Yashmak.IO
{
    using System.IO;

    public interface IStorageProvider
    {
        Stream StreamFile(string userId, string path);

        string UploadStream(string userId, string filename, Stream stream);
    }
}