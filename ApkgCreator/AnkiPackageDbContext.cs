using ApkgCreator.DataModels;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace ApkgCreator
{
    public class AnkiPackageDbContext : DbContext, IAnkiPackageDbContext
    {
        private const string DB_NAME = "collection.anki2";
        private string _directoryName;

        public DbSet<AnkiCard> Cards { get; set; }
        public DbSet<AnkiCol> Cols { get; set; }
        public DbSet<AnkiGraves> Graves { get; set; }
        public DbSet<AnkiNote> Notes { get; set; }
        public DbSet<AnkiRevlog> Revlogs { get; set; }

        public AnkiPackageDbContext()
        {

        }

        public void Init(string directoryName)
        {
            _directoryName = directoryName;
            Database.EnsureCreated();
        }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionbuilder)
        {
            optionbuilder.UseSqlite($"Data Source={Path.Combine(_directoryName, DB_NAME)}");
        }
    }
}
