using System;
using Microsoft.EntityFrameworkCore;
using OxfordDictionaries.DataModels;

namespace OxfordDictionaries
{
    public class OxfordDictionariesCacheDbContext : DbContext, IOxfordDictionariesCacheDBContext
    {
        public DbSet<LexicalEntry> LexicalEntries { get; set; }
        public DbSet<AudioFile> AudioFiles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(Environment.GetEnvironmentVariable(OxfordDictionariesConstants.OxfordCacheDbConnectionStringEnvironmentVariableName));
        }
    }
}
