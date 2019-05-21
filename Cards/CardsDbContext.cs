using System;
using Microsoft.EntityFrameworkCore;

namespace Cards
{
    public class CardsDbContext : DbContext, ICardsDbContext
    {
        public DbSet<Card> Cards { get; set; }
        public DbSet<Example> Examples { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(Environment.GetEnvironmentVariable(CardsConstants.CardsDbConnectionStringEnvironmentVariableName));
        }
    }
}
