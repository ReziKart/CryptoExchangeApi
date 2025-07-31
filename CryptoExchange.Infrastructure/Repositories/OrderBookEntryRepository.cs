using CryptoExchange.Domain.Entities;
using CryptoExchange.Infrastructure.Data;
using CryptoExchange.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CryptoExchange.Infrastructure.Repositories
{
    public class OrderBookEntryRepository(ExchangeDbContext db) : IOrderBookEntryRepository
    {
        private readonly ExchangeDbContext _db = db ?? throw new ArgumentNullException();

        public async Task<List<OrderBookEntry>> GetAllAsync(OrderType type)
        {
            var query = _db.OrderBookEntries.Where(obe => obe.OrderType == type);
            query = type == OrderType.Buy ? query.OrderBy(q => q.Price) : query.OrderByDescending(q => q.Price);
            var entities = await query.ToListAsync();
            return entities;
        }

    }
}
