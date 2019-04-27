using Microsoft.EntityFrameworkCore;
using System.IO;

namespace ApkgCreator
{
    public class ApkgDbContext : DbContext
    {
        private const string DB_NAME = "collection.anki2";
        private string _directoryName;

        private static bool _created = false;

        public ApkgDbContext()
        {

        }

        public ApkgDbContext(string directoryName)
        {
            _directoryName = directoryName;

            if (!_created)
            {
                _created = true;
                Database.EnsureDeleted();
                Database.EnsureCreated();
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionbuilder)
        {
            optionbuilder.UseSqlite($"Data Source={Path.Combine(_directoryName, DB_NAME)}");
        }

        //public DbSet<Category> Categories { get; set; }
    }
}
