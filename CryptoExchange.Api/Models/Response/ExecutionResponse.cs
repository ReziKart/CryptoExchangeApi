using CryptoExchange.Application.Dtos;

namespace CryptoExchange.Api.Models.Response
{
    public class ExecuteResponse
    {
        public decimal Requested { get; set; }
        public decimal Fulfilled { get; set; }
        public List<ExecutionOrderDto> Orders { get; set; } = [];
    }
}
