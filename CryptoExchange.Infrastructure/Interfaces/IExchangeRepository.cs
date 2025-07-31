using CryptoExchange.Domain.Entities;

namespace CryptoExchange.Infrastructure.Interfaces
{
    public interface IExchangeRepository
    {
        Task<List<Exchange>> GetAllAsync();
        Task UpdateRangeAsync(List<Exchange> entities);
    }
}
