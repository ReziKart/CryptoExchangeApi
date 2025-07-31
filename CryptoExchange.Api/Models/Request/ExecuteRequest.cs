using CryptoExchange.Domain.Entities;

namespace CryptoExchange.Api.Models.Request
{
    public class ExecuteRequest
    {
        public OrderType OrderType { get; set; } 
        public decimal Amount { get; set; }
    }
}
