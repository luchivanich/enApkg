using Microsoft.EntityFrameworkCore;

namespace Cards
{
    public interface ICardsDbContext
    {
        DbSet<Card> Cards { get; set; }

        int SaveChanges();
    }
}