using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoExchange.Application.Dtos
{
    public class ExecutionOrderDto
    {
        public string Exchange { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public decimal Amount { get; set; }
    }
}
