using CryptoExchange.Application.Dtos;
using CryptoExchange.Application.Interfaces;
using CryptoExchange.Domain.Entities;
using CryptoExchange.Infrastructure.Interfaces;

namespace CryptoExchange.Application.Services
{
    public class BestPriceExecutionStrategy(IOrderBookEntryRepository orderBookEntryRepository, IExchangeRepository exchangeRepository) : IExecutionStrategy
    {
        private readonly IOrderBookEntryRepository _orderBookEntryRepository = orderBookEntryRepository ?? throw new ArgumentNullException(nameof(orderBookEntryRepository));
        private readonly IExchangeRepository _exchangeRepository = exchangeRepository ?? throw new ArgumentNullException(nameof(exchangeRepository));

        public async Task<List<ExecutionOrderDto>> ExecuteAsync(OrderType orderType, decimal amount)
        {
            var exchanges = await _exchangeRepository.GetAllAsync();
            var sortedOrders = await _orderBookEntryRepository.GetAllAsync(orderType == OrderType.Buy ? OrderType.Sell : OrderType.Buy);

            var exchangeMap = exchanges.ToDictionary(e => e.Id);

            decimal remaining = amount;
            var executionOrders = new List<ExecutionOrderDto>();

            foreach (var order in sortedOrders)
            {
                if (remaining <= 0)
                    break;

                if (!exchangeMap.TryGetValue(order.ExchangeId, out var exchange))
                    continue;

                var maxExecutable = exchange.GetExecutableAmount(order);
                if (maxExecutable <= 0)
                    continue;

                var toExecute = Math.Min(remaining, maxExecutable);
                
                if (toExecute <= 0)
                    continue;

                toExecute = Math.Min(toExecute, maxExecutable);

                exchange.ExecuteAtomic(order, toExecute);

                executionOrders.Add(new ExecutionOrderDto
                {
                    Exchange = exchange.Name,
                    Price = order.Price,
                    Amount = toExecute
                });

                remaining -= toExecute;
            }
            if (executionOrders.Count > 0)
            {
                await _exchangeRepository.UpdateRangeAsync(exchanges);
            }

            return executionOrders;
        }
    }

}
