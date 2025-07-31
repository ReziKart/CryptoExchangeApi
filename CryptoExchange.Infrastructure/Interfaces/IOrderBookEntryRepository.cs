using CryptoExchange.Domain.Entities;

namespace CryptoExchange.Infrastructure.Interfaces
{
    public interface IOrderBookEntryRepository
    {
        Task<List<OrderBookEntry>> GetAllAsync(OrderType type);
    }
}
