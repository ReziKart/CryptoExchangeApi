using CryptoExchange.Application.Dtos;
using CryptoExchange.Domain.Entities;

namespace CryptoExchange.Application.Interfaces
{
    public interface IExecutionStrategy
    {
        Task<List<ExecutionOrderDto>> ExecuteAsync(OrderType type, decimal amount);

    }
}
