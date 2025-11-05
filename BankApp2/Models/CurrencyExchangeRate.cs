namespace BankApp2.Models
{
    // Represents an exchange rate for a specific currency
    public class CurrencyExchangeRate
    {
        public string CurrencyCode { get; set; }
        public decimal ExchangeRate { get; set; }
        public DateTime LastUpdated { get; set; }

        // Create a new exchange rate with the current timestamp
        public CurrencyExchangeRate(string currencyCode, decimal exchangeRate)
        {
            CurrencyCode = currencyCode;
            ExchangeRate = exchangeRate;
            LastUpdated = DateTime.Now;
        }
        
        // Format the exchange rate for display
        public override string ToString()
        {
            return $"{CurrencyCode}: {ExchangeRate:F4} (Updated: {LastUpdated:yyyy-MM-dd HH:mm})";
        }
    }
}
