namespace OxfordDictionaries
{
    public interface IFileDownloader
    {
        byte[] GetFileFromUrl(string url);
    }
}
