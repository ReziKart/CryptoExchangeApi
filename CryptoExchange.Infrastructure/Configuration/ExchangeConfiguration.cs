using CryptoExchange.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class ExchangeConfiguration : IEntityTypeConfiguration<Exchange>
{
    public void Configure(EntityTypeBuilder<Exchange> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.BalanceInEUR).IsRequired();
        builder.Property(e => e.BalanceInBTC).IsRequired();

        builder.HasMany(e => e.Orders)
               .WithOne()
               .HasForeignKey("ExchangeId")
               .IsRequired();

        builder.HasIndex(e => e.Name).IsUnique();
    }
}