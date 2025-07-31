using CryptoExchange.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace CryptoExchange.Infrastructure.Configuration
{
    public class OrderBookEntryConfiguration : IEntityTypeConfiguration<OrderBookEntry>
    {
        public void Configure(EntityTypeBuilder<OrderBookEntry> builder)
        {
            builder.HasKey(o => o.Id);

            builder.Property(o => o.Price).IsRequired();
            builder.Property(o => o.Amount).IsRequired();
            builder.Property(o => o.OrderType).IsRequired();

            builder.HasOne(o => o.Exchange)
                   .WithMany(e => e.Orders)
                   .HasForeignKey(o => o.ExchangeId)
                   .IsRequired();
        }
    }
}
