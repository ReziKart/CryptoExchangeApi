using CryptoExchange.Application.Interfaces;
using CryptoExchange.Application.Services;
using CryptoExchange.Infrastructure.Data;
using CryptoExchange.Infrastructure.Interfaces;
using CryptoExchange.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CryptoExchange.Api.Extensions
{
    public static  class IServiceCollectionExtension
    {
        public static IServiceCollection AddMetaExchangeServices(this IServiceCollection services)
        {
            //Add DbContext
            services.AddDbContext<ExchangeDbContext>(options =>
                options.UseInMemoryDatabase("ExchangeDb"));

            // Repositories
            services.AddScoped<IExchangeRepository, ExchangeRepository>();
            services.AddScoped<IOrderBookEntryRepository, OrderBookEntryRepository>();

            // Application services
            services.AddScoped<IExecutionStrategy, BestPriceExecutionStrategy>();
            services.AddScoped<ITradeService, TradeService>();

            // Controllers and swagger
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            return services;
        }
    }
}
