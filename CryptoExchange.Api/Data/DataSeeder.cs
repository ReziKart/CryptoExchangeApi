using CryptoExchange.Domain.Entities;
using CryptoExchange.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CryptoExchange.Api.Data
{
    public static class DataSeeder
    {
        public static async Task SeedExchangesAsync(ExchangeDbContext context, string jsonFilePath)
        {
            if (await context.Exchanges.AnyAsync())
                return; 

            var json = await File.ReadAllTextAsync(jsonFilePath);
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            options.Converters.Add(new JsonStringEnumConverter());
            var exchanges = JsonSerializer.Deserialize<List<Exchange>>(json, options);

            if (exchanges != null && exchanges.Count != 0)
            {
                context.Exchanges.AddRange(exchanges);
                await context.SaveChangesAsync();
            }
        }
    }
}
