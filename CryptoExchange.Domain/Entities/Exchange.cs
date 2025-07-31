namespace CryptoExchange.Domain.Entities
{
    public class Exchange
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal BalanceInEUR { get; set; }
        public decimal BalanceInBTC { get; set; }

        public List<OrderBookEntry> Orders { get; set; } = [];

        private readonly Lock _lock = new();

        public decimal GetExecutableAmount(OrderBookEntry entry)
        {
            return entry.OrderType == OrderType.Buy
                ? Math.Min(entry.Amount, BalanceInEUR/ entry.Price)
                : Math.Min(entry.Amount, BalanceInBTC);
        }

        public void Execute(OrderBookEntry entry, decimal executedAmount)
        {
            if (entry.OrderType == OrderType.Buy)
            {
                var requiredEur = entry.Price * executedAmount;
                if (BalanceInEUR < requiredEur)
                    throw new InvalidOperationException("Insufficient EUR");

                BalanceInEUR -= requiredEur;
                BalanceInBTC += executedAmount;
            }
            else
            {
                if (BalanceInBTC < executedAmount)
                    throw new InvalidOperationException("Insufficient BTC");

                BalanceInBTC -= executedAmount;
                BalanceInEUR += executedAmount * entry.Price;
            }
        }
        public void ExecuteAtomic(OrderBookEntry entry, decimal executedAmount)
        {
            lock (_lock)
            {
                Execute(entry, executedAmount);
            }
        }

    }

}
