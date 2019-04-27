using Microsoft.EntityFrameworkCore;

namespace OxfordDictionaries
{
    public class OxfordDictionariesCacheDBContext : DbContext, IOxfordDictionariesCacheDBContext
    {
        private string _connectionString;

        public DbSet<CachedItem> Words { get; set; }

        public OxfordDictionariesCacheDBContext(IOxfordCacheDbConnectionStringProvider oxfordCacheDbConnectionStringProvider)
        {
            _connectionString = oxfordCacheDbConnectionStringProvider.GetOxfordCacheDbConnectionString();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(_connectionString);
        }
    }
}
