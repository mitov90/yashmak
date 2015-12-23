using System.IO;

namespace Yashmak.IO
{
    public interface IStorageProvider
    {
        Stream StreamFile(string userId, string path);
        string UploadStream(string userId, string filename, Stream stream);
    }
}