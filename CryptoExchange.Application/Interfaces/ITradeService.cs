using CryptoExchange.Application.Dtos;
using CryptoExchange.Domain.Entities;

namespace CryptoExchange.Application.Interfaces
{
    public interface ITradeService
    {
        Task<List<ExecutionOrderDto>> ExecuteTradeAsync(OrderType type, decimal amount);
    }
}
