using CryptoExchange.Domain.Entities;
using CryptoExchange.Infrastructure.Data;
using CryptoExchange.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CryptoExchange.Infrastructure.Repositories
{
    public class ExchangeRepository(ExchangeDbContext db) : IExchangeRepository
    {
        private readonly ExchangeDbContext _db = db ?? throw new ArgumentNullException(nameof(db));

        public async Task<List<Exchange>> GetAllAsync()
        {
            var result = await _db.Exchanges.ToListAsync();
            return result;
        }

        public async Task UpdateRangeAsync(List<Exchange> entities)
        {
            foreach (var exchange in entities)
            {
                _db.Exchanges.Attach(exchange);

                _db.Entry(exchange).Property(e => e.BalanceInEUR).IsModified = true;
                _db.Entry(exchange).Property(e => e.BalanceInBTC).IsModified = true;
            }
            await _db.SaveChangesAsync();

        }
    }
}
