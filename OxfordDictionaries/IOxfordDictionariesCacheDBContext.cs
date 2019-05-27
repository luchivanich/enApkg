using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using OxfordDictionaries.DataModels;

namespace OxfordDictionaries
{
    public interface IOxfordDictionariesCacheDBContext
    {
        DbSet<LexicalEntry> LexicalEntries { get; set; }
        DbSet<AudioFile> AudioFiles { get; set; }

        int SaveChanges();

        DatabaseFacade Database { get; }
    }
}
