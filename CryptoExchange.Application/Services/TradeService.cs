using CryptoExchange.Application.Dtos;
using CryptoExchange.Application.Interfaces;
using CryptoExchange.Domain.Entities;
using CryptoExchange.Infrastructure.Interfaces;

namespace CryptoExchange.Application.Services
{
    public class TradeService(IExchangeRepository repo, IExecutionStrategy strategy) : ITradeService
    {
        private readonly IExecutionStrategy _strategy = strategy ?? throw new ArgumentNullException(nameof(strategy));
        public async Task<List<ExecutionOrderDto>> ExecuteTradeAsync(OrderType type, decimal amount)
        {
            var result = await _strategy.ExecuteAsync(type, amount);
            return result;
        }
    }
}
