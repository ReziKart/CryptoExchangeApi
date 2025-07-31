using CryptoExchange.Api.Data;
using CryptoExchange.Api.Extensions;
using CryptoExchange.Application.Interfaces;
using CryptoExchange.Infrastructure.Data;
using Microsoft.Extensions.Hosting;
using System.IO;

var builder = WebApplication.CreateBuilder(args);



// Register all CryptoExchange dependencies
builder.Services.AddMetaExchangeServices();
var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ExchangeDbContext>();

    var exchangesJsonPath = Path.Combine(builder.Environment.ContentRootPath, "Data", "cryptoExchanges.json");

    await DataSeeder.SeedExchangesAsync(context, exchangesJsonPath);
}
app.Run();
