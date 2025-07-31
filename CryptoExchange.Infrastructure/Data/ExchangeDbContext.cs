using CryptoExchange.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using CryptoExchange.Infrastructure.Configuration;

namespace CryptoExchange.Infrastructure.Data
{
    public class ExchangeDbContext (DbContextOptions<ExchangeDbContext> options) : DbContext(options)
    {
        public DbSet<Exchange> Exchanges => Set<Exchange>();
        public DbSet<OrderBookEntry> OrderBookEntries => Set<OrderBookEntry>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new ExchangeConfiguration());
            modelBuilder.ApplyConfiguration(new OrderBookEntryConfiguration());
        }
    }
}
