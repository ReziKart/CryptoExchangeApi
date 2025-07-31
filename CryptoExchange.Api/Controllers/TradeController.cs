using CryptoExchange.Api.Models.Request;
using CryptoExchange.Api.Models.Response;
using CryptoExchange.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CryptoExchange.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class TradeController(ILogger<TradeController> logger, ITradeService tradeService) : ControllerBase
{
    private readonly ILogger<TradeController> _logger = logger;
    private readonly ITradeService _tradeService = tradeService;


    [HttpPost("execute")]
    public async Task<IActionResult> Execute([FromBody] ExecuteRequest request)
    {
        var orders = await _tradeService.ExecuteTradeAsync(request.OrderType, request.Amount);

        return Ok(new ExecuteResponse
        {
            Requested = request.Amount,
            Fulfilled = orders.Sum(x => x.Amount),
            Orders = orders
        });
    }
}
