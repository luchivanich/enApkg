using ApkgCreator.DataModels;
using Cards;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace ApkgCreator
{
    public class AnkiPackageBuilder : IAnkiPackageBuilder
    {
        private IAnkiPackageDbContext _ankiPackageDbContext;
        private IAnkiEntityBuilder _ankiEntityBuilder;

        private DirectoryInfo _directoryInfo;
        private string _targetPath;
        private string _targetName;
        private string apkgExtension = ".apkg";
        private string _media = "";
        private AnkiCol _ankiCol;

        public AnkiPackageBuilder(IAnkiPackageDbContext ankiPackageDbContext, IAnkiEntityBuilder ankiEntityBuilder)
        {
            _ankiPackageDbContext = ankiPackageDbContext;
            _ankiEntityBuilder = ankiEntityBuilder;
        }

        public void BuildApkgPackage(string targetPath, string targetName, List<Card> cards)
        {
            _targetPath = targetPath;
            _targetName = targetName;
            _directoryInfo = new DirectoryInfo(_targetPath);

            Init();
            foreach(var card in cards)
            {
                AddCard(card);
            }
            _ankiPackageDbContext.SaveChanges();
            CreateMediaFile();
            CreateApkgFile();
            CleanupTargetDirectory();
        }

        public void Init()
        {
            var directoryInfo = Directory.CreateDirectory(_targetPath);
            foreach(var fi in directoryInfo.GetFiles())
            {
                fi.Delete();
            }

            _ankiPackageDbContext.Init(_targetPath);

            _ankiCol = _ankiEntityBuilder.BuildAnkiCol();

            _ankiPackageDbContext.Cols.Add(_ankiCol);
        }

        public void AddCard(Card card)
        {
            var ankiCard = _ankiEntityBuilder.BuildAnkiCard(card, _ankiCol);
            if (!string.IsNullOrWhiteSpace(card.AudioFileName))
            {
                SaveAudioFile(card.AudioFileName, card.AudioFileData);
            }
            _ankiPackageDbContext.Cards.Add(ankiCard);
        }

        private void SaveAudioFile(string fileName, byte[] fileData)
        {
            var fileFullPath = Path.Combine(_targetPath, fileName);
            if (File.Exists(fileFullPath))
            {
                return;
            }

            File.WriteAllBytes(fileFullPath, fileData);
            AddMedia(fileName);
        }

        private void CreateMediaFile()
        {
            _media = $"{{{_media.Trim(',')}}}";
            File.WriteAllText(Path.Combine(_targetPath, "media"), _media);
        }

        public void CreateApkgFile()
        {
            var files = _directoryInfo.GetFiles();

            var targetFileName = Path.Combine(_targetPath, _targetName + apkgExtension);
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

        private void AddMedia(string fileName)
        {
            _media += $"\"{fileName}\":\"{fileName}\",";
        }

        private void CleanupTargetDirectory()
        {
            foreach (FileInfo fi in _directoryInfo.GetFiles().Where(f => f.Extension != apkgExtension))
            {
                fi.Delete();
            }
        }
    }
}
