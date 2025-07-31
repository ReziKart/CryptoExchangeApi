using CryptoExchange.Application.Services;
using CryptoExchange.Domain.Entities;
using CryptoExchange.Infrastructure.Interfaces;
using Moq;

namespace CryptoExchange.Tests
{
    public class BestPriceExecutionStrategyTests
    {
        [Fact]
        public async Task ExecuteAsync_Buy2BTC_FindsBestPricesAcrossExchanges()
        {
            // Arrange
            var exchange1 = new Exchange
            {
                Id = 1,
                Name = "ExchangeA",
                BalanceInEUR = 100000,
                BalanceInBTC = 1,
                Orders = new List<OrderBookEntry>
        {
            new() { Price = 30000, Amount = 1, OrderType = OrderType.Sell , ExchangeId = 1},
        }
            };

            var exchange2 = new Exchange
            {
                Id = 2,
                Name = "ExchangeB",
                BalanceInEUR = 100000,
                BalanceInBTC = 2,
                Orders = new List<OrderBookEntry>
        {
            new() { Price = 31000, Amount = 2, OrderType = OrderType.Sell , ExchangeId = 2},
        }
            };

            var exchanges = new List<Exchange> { exchange1, exchange2 };

            var exchangeRepoMock = new Mock<IExchangeRepository>();
            exchangeRepoMock.Setup(r => r.GetAllAsync())
                .ReturnsAsync(exchanges);

            var orderRepoMock = new Mock<IOrderBookEntryRepository>();
            orderRepoMock.Setup(r => r.GetAllAsync(OrderType.Sell))
                .ReturnsAsync(exchanges.SelectMany(e => e.Orders).Where(o => o.OrderType == OrderType.Sell).ToList());

            var strategy = new BestPriceExecutionStrategy(orderRepoMock.Object, exchangeRepoMock.Object );

            // Act
            var result = await strategy.ExecuteAsync(OrderType.Buy, 2);

            // Assert
            Assert.Equal(2, result.Sum(r => r.Amount));
            Assert.Equal(30000, result[0].Price);
            Assert.Equal(1, result[0].Amount);
            Assert.Equal(31000, result[1].Price);
            Assert.Equal(1, result[1].Amount);
        }

        [Fact]
        public async Task ExecuteAsync_BuyWithInsufficientBalance_DoesNotOverSpend()
        {
            var exchange = new Exchange
            {
                Id = 1,
                Name = "ExchangeX",
                BalanceInEUR = 1000,
                BalanceInBTC = 0,
                Orders = new List<OrderBookEntry>
        {
            new() { Price = 30000, Amount = 1, OrderType = OrderType.Sell, ExchangeId = 1}
        }
            };

            var exchanges = new List<Exchange> { exchange };

            var exchangeRepo = new Mock<IExchangeRepository>();
            exchangeRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(exchanges);

            var orderRepo = new Mock<IOrderBookEntryRepository>();
            orderRepo.Setup(r => r.GetAllAsync(OrderType.Sell))
                .ReturnsAsync(exchange.Orders);

            var strategy = new BestPriceExecutionStrategy(orderRepo.Object, exchangeRepo.Object);

            var result = await strategy.ExecuteAsync(OrderType.Buy, 1);

            Assert.Empty(result);
        }

        [Fact]
        public async Task ExecuteAsync_NoAvailableOrders_ReturnsEmptyList()
        {
            var exchange = new Exchange
            {
                Id = 1,
                Name = "ExchangeX",
                BalanceInEUR = 100_000,
                BalanceInBTC = 0,
                Orders = new List<OrderBookEntry>() 
            };

            var exchangeRepo = new Mock<IExchangeRepository>();
            exchangeRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Exchange> { exchange });

            var orderRepo = new Mock<IOrderBookEntryRepository>();
            orderRepo.Setup(r => r.GetAllAsync(OrderType.Sell))
                .ReturnsAsync(new List<OrderBookEntry>());

            var strategy = new BestPriceExecutionStrategy(orderRepo.Object, exchangeRepo.Object);

            var result = await strategy.ExecuteAsync(OrderType.Buy, 1);

            Assert.Empty(result);
        }

        [Fact]
        public async Task ExecuteAsync_PartialFillDueToOrderLimit()
        {
            var exchange = new Exchange
            {
                Id = 1,
                Name = "ExchangeX",
                BalanceInEUR = 100000,
                BalanceInBTC = 1,
                Orders = new List<OrderBookEntry>
        {
            new() { Price = 30000, Amount = 0.5m, OrderType = OrderType.Sell, ExchangeId = 1}
        }
            };

            var exchanges = new List<Exchange> { exchange };

            var exchangeRepo = new Mock<IExchangeRepository>();
            exchangeRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(exchanges);

            var orderRepo = new Mock<IOrderBookEntryRepository>();
            orderRepo.Setup(r => r.GetAllAsync(OrderType.Sell))
                .ReturnsAsync(exchange.Orders);

            var strategy = new BestPriceExecutionStrategy(orderRepo.Object, exchangeRepo.Object);

            var result = await strategy.ExecuteAsync(OrderType.Buy, 1);

            Assert.Single(result);
            Assert.Equal(0.5m, result[0].Amount);
        }
    }
}
