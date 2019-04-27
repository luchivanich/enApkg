using Cards;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.IO.Compression;

namespace ApkgCreator
{
    public class ApkgBuilder
    {
        private string _directoryName;
        private string apkgExtension = ".apkg";
        private string _media = "{}";
        private int _sequence = 1;
        private int _mediaCounter = 0;
        private long _deckId = 1413830276206;
        private long _modelId = 1397924728875;
        private Byte fieldDelimeter = 0x1f;
        private string fieldsFormat = @"{1}{0}{2}{0}{3}{0}{4}{0}{5}{0}{6}{0}{7}";

        ApkgDbContext _dbContext;

        public void Init(string directoryName)
        {
            _directoryName = directoryName;

            var directoryInfo = Directory.CreateDirectory(_directoryName);
            _dbContext = new ApkgDbContext(_directoryName);

            File.WriteAllText(Path.Combine(_directoryName, "media"), _media);

            var createDbScript = File.ReadAllText(@"gistfile1.sql");
            _dbContext.Database.ExecuteSqlCommand(createDbScript);

            var addColDataScript = File.ReadAllText(@"collection.col.sql");

            var conf = File.ReadAllText(@"col.conf.txt");
            var confParam = new SqliteParameter("@conf", conf);

            var dconf = File.ReadAllText(@"col.dconf.txt");
            var dconfParam = new SqliteParameter("@dconf", dconf);

            var decks = File.ReadAllText(@"col.decks.txt");
            decks = decks.Replace("@deckId", _deckId.ToString());
            var decksParam = new SqliteParameter("@decks", decks);

            var models = File.ReadAllText(@"col.models.txt");
            models = models.Replace("@modelId", _modelId.ToString());
            var modelsParam = new SqliteParameter("@models", models);

            var tags = File.ReadAllText(@"col.tags.txt");
            var tagsParam = new SqliteParameter("@tags", tags);

            _dbContext.Database.ExecuteSqlCommand(addColDataScript, confParam, dconfParam, decksParam, modelsParam, tagsParam);

        }

        public void AddCard(Card card)
        {
            var addCardDataScript = File.ReadAllText(@"collection.cards.sql");
            var addNoteDataScript = File.ReadAllText(@"collection.notes.sql");

            var cardIdParam = new SqliteParameter("@cardId", _sequence);
            var noteIdParam = new SqliteParameter("@noteId", _sequence);
            var deckIdParam = new SqliteParameter("@deckId", _deckId);
            var guidParam = new SqliteParameter("@guid", Guid.NewGuid());
            var modelIdParam = new SqliteParameter("@modelId", _modelId);

            var fields = string.Format(fieldsFormat, "\u001f", _sequence.ToString(), string.Empty, card.Definition + " - {{c1::" + card.Word + "}}", card.Word, string.Empty, string.Empty, string.Empty);
            var fieldsParam = new SqliteParameter("@fields", fields);

            _dbContext.Database.ExecuteSqlCommand(addCardDataScript, cardIdParam, noteIdParam, deckIdParam);
            _dbContext.Database.ExecuteSqlCommand(addNoteDataScript, noteIdParam, guidParam, modelIdParam, fieldsParam);

            _sequence++;

        }

        public void CreateApkgFile(string directoryName, string targetName)
        {
            var directoryInfo = new DirectoryInfo(Path.Combine(directoryName, targetName));
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
