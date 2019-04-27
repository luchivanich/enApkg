using Microsoft.EntityFrameworkCore;

namespace Cards
{
    public class CardsDbContext : DbContext
    {
        public DbSet<Card> Cards { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=cards.db");
        }
    }
}
