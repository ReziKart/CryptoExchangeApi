namespace CryptoExchange.Domain.Entities
{
    public class OrderBookEntry
    {
        public int Id { get; set; }
        public decimal Price { get; set; }
        public decimal Amount { get; set; }
        public OrderType OrderType { get; set; }

        public int ExchangeId { get; set; }

        public Exchange Exchange { get; set; } = new();
    }

}
