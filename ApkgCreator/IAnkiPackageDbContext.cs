using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace ApkgCreator
{
    public interface IAnkiPackageDbContext
    {
        DatabaseFacade Database { get; }
        DbSet<AnkiCard> Cards { get; set; }
        DbSet<AnkiCol> Cols { get; set; }
        DbSet<AnkiGraves> Graves { get; set; }
        DbSet<AnkiNote> Notes { get; set; }
        DbSet<AnkiRevlog> Revlogs { get; set; }

        void Init(string directoryName);

        int SaveChanges();
    }
}