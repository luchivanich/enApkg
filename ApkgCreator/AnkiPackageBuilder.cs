using Cards;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace ApkgCreator
{
    public class AnkiPackageBuilder : IAnkiPackageBuilder
    {
        private IAnkiPackageDbContext _ankiPackageDbContext;
        private IAnkiEntityBuilder _ankiEntityBuilder;

        private string _directoryName;
        private string apkgExtension = ".apkg";
        private string _media = "{}";
        private int _mediaCounter = 0;
        private long _deckId = 1413830276206;
        private long _modelId = 1397924728875;

        public AnkiPackageBuilder(IAnkiPackageDbContext ankiPackageDbContext, IAnkiEntityBuilder ankiEntityBuilder)
        {
            _ankiPackageDbContext = ankiPackageDbContext;
            _ankiEntityBuilder = ankiEntityBuilder;
        }

        public void BuildApkgPackage(string targetPath, string targetName, List<Card> cards)
        {
            Init(targetPath);
            foreach(var card in cards)
            {
                AddCard(card);
            }
            _ankiPackageDbContext.SaveChanges();
            CreateApkgFile(targetPath, targetName);
        }

        public void Init(string directoryName)
        {
            _directoryName = directoryName;

            var directoryInfo = Directory.CreateDirectory(_directoryName);
            _ankiPackageDbContext.Init(_directoryName);

            File.WriteAllText(Path.Combine(_directoryName, "media"), _media);

            var ankiCol = _ankiEntityBuilder.BuildAnkiCol(_deckId, _modelId);

            _ankiPackageDbContext.Cols.Add(ankiCol);
        }

        public void AddCard(Card card)
        {
            var ankiCard = _ankiEntityBuilder.BuildAnkiCard(card, _deckId, _modelId);
            _ankiPackageDbContext.Cards.Add(ankiCard);
        }

        public void CreateApkgFile(string directoryName, string targetName)
        {
            var directoryInfo = new DirectoryInfo(directoryName);
            var files = directoryInfo.GetFiles();

            var targetFileName = Path.Combine(directoryName, targetName + apkgExtension);
            File.Delete(targetFileName);

            using (var fileStream = new FileStream(targetFileName, FileMode.CreateNew))
            {
                using (var archive = new ZipArchive(fileStream, ZipArchiveMode.Create, true))
                {
                    foreach (var file in files)
                    {
                        var fileData = File.ReadAllBytes(file.FullName);
                        var zipArchiveEntry = archive.CreateEntry(file.Name, CompressionLevel.Fastest);
                        using (var zipStream = zipArchiveEntry.Open())
                        {
                            zipStream.Write(fileData, 0, fileData.Length);
                        }
                    }
                }
            }
        }

        private void AddMedia(string mediaFile)
        {
            _mediaCounter++;
            if (_mediaCounter > 1)
            {
                mediaFile.Insert(mediaFile.Length - 2, ",");
            }

            mediaFile.Insert(mediaFile.Length - 2, $"\"{_mediaCounter.ToString()}\" : \"{mediaFile.ToString()}\"");

            File.WriteAllText(Path.Combine(_directoryName, "media"), _media);
        }
    }
}
