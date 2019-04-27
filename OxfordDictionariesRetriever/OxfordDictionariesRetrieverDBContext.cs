using Microsoft.EntityFrameworkCore;

namespace OxfordDictionariesRetriever
{
    public class OxfordDictionariesRetrieverDBContext : DbContext
    {
        public DbSet<CachedItem> Words { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=oxfordDictionary.db");
        }
    }
}
