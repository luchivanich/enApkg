using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace OxfordDictionaries
{
    public interface IOxfordDictionariesCacheDBContext
    {
        DbSet<CachedItem> Words { get; set; }

        int SaveChanges();

        DatabaseFacade Database { get; }
    }
}
