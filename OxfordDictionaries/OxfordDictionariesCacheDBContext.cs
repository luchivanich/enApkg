using System;
using Microsoft.EntityFrameworkCore;

namespace OxfordDictionaries
{
    public class OxfordDictionariesCacheDbContext : DbContext, IOxfordDictionariesCacheDBContext
    {
        private string _connectionString;

        public DbSet<CachedItem> Words { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(Environment.GetEnvironmentVariable(OxfordDictionariesConstants.OxfordCacheDbConnectionStringEnvironmentVariableName));
        }
    }
}
