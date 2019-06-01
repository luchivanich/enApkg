using ApkgCreator.DataModels;
using Cards;
using Microsoft.EntityFrameworkCore;
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
        private IAnkiFieldsBuilder _ankiFieldsBuilder;

        private DirectoryInfo _directoryInfo;
        private string _targetPath;
        private string _targetName;
        private string _targetFileFullName;
        private string _targetFileName;
        private string apkgExtension = ".apkg";
        private Dictionary<string, string> _mediaPairs = new Dictionary<string, string>();
        private AnkiCol _ankiCol;

        public AnkiPackageBuilder(IAnkiPackageDbContext ankiPackageDbContext, IAnkiEntityBuilder ankiEntityBuilder, IAnkiFieldsBuilder ankiFieldsBuilder)
        {
            _ankiPackageDbContext = ankiPackageDbContext;
            _ankiEntityBuilder = ankiEntityBuilder;
            _ankiFieldsBuilder = ankiFieldsBuilder;
        }

        public void BuildApkgPackage(string targetPath, string targetName, List<Card> cards)
        {
            _targetPath = targetPath;
            _targetName = targetName;
            _targetFileName = _targetName + apkgExtension;
            _targetFileFullName = Path.Combine(_targetPath, _targetName + apkgExtension);
            _directoryInfo = new DirectoryInfo(_targetPath);

            Init();
            CreateCol();
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
            CleanupTargetDirectory();
            UnpackApkgFile();
            LoadMedia();

            var fi = _directoryInfo.GetFiles().FirstOrDefault(f => f.Name == _targetFileName);
            if (fi != null)
            {
                fi.Delete();
            }

            _ankiPackageDbContext.Init(_targetPath);
        }

        private void LoadMedia()
        {
            if (!File.Exists(Path.Combine(_targetPath, "media")))
            {
                return;
            }

            var media = File.ReadAllText(Path.Combine(_targetPath, "media"));
            media = media.Trim().Trim('{').Trim('}');
            _mediaPairs = media.Split(',').Select(i =>
            {
                var splitedPair = i.Split(':');
                return new KeyValuePair<string, string>(splitedPair[0].Trim().Trim('"'), splitedPair[1].Trim().Trim('"'));
            }).ToDictionary(x => x.Key, x => x.Value);
        }

        private void CreateCol()
        {
            _ankiCol = _ankiPackageDbContext.Cols.FirstOrDefault();
            if (_ankiCol == null)
            {
                _ankiCol = _ankiEntityBuilder.BuildAnkiCol();
                _ankiPackageDbContext.Cols.Add(_ankiCol);
            }
            else
            {
                _ankiCol.DeserializeProperties();
            }
        }

        private void UnpackApkgFile()
        {
            if (File.Exists(_targetFileFullName))
            {
                ZipFile.ExtractToDirectory(_targetFileFullName, _targetPath);
            }
        }

        public void AddCard(Card card)
        {
            var newAnkiCard = _ankiEntityBuilder.BuildAnkiCard(card, _ankiCol);

            var ankiCard = FindAnkiCardByKeyword(card.Word);
            if (ankiCard != null)
            {
                ankiCard.Note.Fields = newAnkiCard.Note.Fields;
            }
            else
            {
                _ankiPackageDbContext.Cards.Add(newAnkiCard);
            }

            if (!string.IsNullOrWhiteSpace(card.AudioFileName))
            {
                SaveAudioFile(card.AudioFileName, card.AudioFileData);
            }
        }

        private AnkiCard FindAnkiCardByKeyword(string keyword)
        {
            foreach(var c in _ankiPackageDbContext.Cards.Include(c => c.Note))
            {
                var fields = _ankiFieldsBuilder.BuildFieldsPairs(c.Note, _ankiCol);
                if (fields.ContainsKey("Keyword") && fields["Keyword"] == keyword)
                {
                    return c;
                }
            }
            return null;
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
            var media = string.Join(",", _mediaPairs.Select(i => $"\"{i.Key}\":\"{i.Value}\""));
            media = $"{{{media}}}";
            File.WriteAllText(Path.Combine(_targetPath, "media"), media);
        }

        public void CreateApkgFile()
        {
            var files = _directoryInfo.GetFiles();

            File.Delete(_targetFileFullName);

            using (var fileStream = new FileStream(_targetFileFullName, FileMode.CreateNew))
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
            if (_mediaPairs.Values.Contains(fileName))
            {
                return;
            }
            _mediaPairs.Add(fileName, fileName);
        }

        private void CleanupTargetDirectory()
        {
            foreach (FileInfo fi in _directoryInfo.GetFiles().Where(f => f.Name != _targetFileName))
            {
                fi.Delete();
            }
        }
    }
}
