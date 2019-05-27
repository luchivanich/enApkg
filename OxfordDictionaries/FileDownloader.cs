using System.Net;

namespace OxfordDictionaries
{
    public class FileDownloader : IFileDownloader
    {
        public byte[] GetFileFromUrl(string url)
        {
            using (var client = new WebClient())
            {
                return client.DownloadData(url);
            }
        }
    }
}
