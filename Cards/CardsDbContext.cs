using Microsoft.EntityFrameworkCore;

namespace Cards
{
    public class CardsDbContext : DbContext, ICardsDbContext
    {
        private string _connectionString;

        public DbSet<Card> Cards { get; set; }
        public DbSet<Example> Examples { get; set; }

        public CardsDbContext(ICardsDbConnectionStringProvider connectionStringProvider)
        {
            _connectionString = connectionStringProvider.GetCardsDbConnectionString();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(_connectionString);
        }
    }
}
