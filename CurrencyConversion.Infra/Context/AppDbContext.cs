using CurrencyConversion.Domain.Entites;
using Microsoft.EntityFrameworkCore;

namespace CurrencyConversion.Infra.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public virtual DbSet<ExchangeRates> ExchangeRates { get; set; }
        public virtual DbSet<Rates> Rates { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ExchangeRates>()
                .Property(b => b.Rate)
                .HasPrecision(18, 6);
        }
    }
}
