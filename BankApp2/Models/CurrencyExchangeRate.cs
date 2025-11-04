namespace BankApp2.Models
{
    public class CurrencyExchangeRate
    {
        public string CurrencyCode { get; set; }
        public decimal ExchangeRate { get; set; }
        public DateTime LastUpdated { get; set; }

        public CurrencyExchangeRate(string currencyCode, decimal exchangeRate)
        {
            CurrencyCode = currencyCode;
            ExchangeRate = exchangeRate;
            LastUpdated = DateTime.Now;
        }
        public override string ToString()
        {
            return $"{CurrencyCode}: {ExchangeRate:F4} (Updated: {LastUpdated:yyyy-MM-dd HH:mm})";
        }
    }
}
