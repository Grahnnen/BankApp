namespace BankApp2.Models
{
    public class CurrencyManager
    {
        private Dictionary<string, CurrencyExchangeRate> exchangeRates;
        // This assumes that the base currency is SEK, could be made more clear
        // Uses the logic that 1 SEK = 0,1 USD for example
        public CurrencyManager()
        {
            exchangeRates = new Dictionary<string, CurrencyExchangeRate>
            {
                { "SEK", new CurrencyExchangeRate("SEK", 1.0m) },
                { "USD", new CurrencyExchangeRate("USD", 0.010m) },
                { "EUR", new CurrencyExchangeRate("EUR", 0.091m) },
                { "GBP", new CurrencyExchangeRate("GBP", 0.080m) }
            };
        }

        public void SetExchangeRate(string currencyCode, decimal rate)
        {
            if (rate <= 0)
            {
                Console.WriteLine("Error: Exchange rate must be greater than 0.");
                return;
            }

            if (exchangeRates.ContainsKey(currencyCode.ToUpper()))
            {
                exchangeRates[currencyCode.ToUpper()].ExchangeRate = rate;
                exchangeRates[currencyCode.ToUpper()].LastUpdated = DateTime.Now;
                Console.WriteLine($"Updated exchange rate for {currencyCode.ToUpper()} to {rate:F4}");
            }
            else
            {
                Console.WriteLine($"Error: Currency {currencyCode.ToUpper()} not found.");
            }
        }

        public void SetExchangeRateInteractive()
        {
            Console.Clear();
            Console.WriteLine("=== Set Exchange Rate ===");
            Console.Write("Enter currency code (SEK, USD, EUR, GBP): ");
            string? currencyCode = Console.ReadLine()?.ToUpper();

            if (string.IsNullOrWhiteSpace(currencyCode))
            {
                Console.WriteLine("Invalid currency code.");
                Console.WriteLine("\nPress any key to continue");
                Console.ReadKey();
                return;
            }

            Console.Write($"Enter exchange rate for {currencyCode} (relative to base currency): ");
            if (decimal.TryParse(Console.ReadLine(), out decimal rate))
            {
                SetExchangeRate(currencyCode, rate);
            }
            else
            {
                Console.WriteLine("Invalid exchange rate.");
            }

            Console.WriteLine("\nPress any key to continue");
            Console.ReadKey();
        }

        public void ViewAllExchangeRates()
        {
            Console.Clear();
            Console.WriteLine("=== Current Exchange Rates ===");
            Console.WriteLine();

            if (exchangeRates.Count == 0)
            {
                Console.WriteLine("No exchange rates configured.");
            }
            else
            {
                foreach (var rate in exchangeRates.Values.OrderBy(r => r.CurrencyCode))
                {
                    Console.WriteLine(rate);
                }
            }

            Console.WriteLine("\nPress any key to continue");
            Console.ReadKey();
        }

        public decimal? GetExchangeRate(string currencyCode)
        {
            if (exchangeRates.TryGetValue(currencyCode.ToUpper(), out var rate))
            {
                return rate.ExchangeRate;
            }
            return null;
        }

        public decimal ConvertCurrency(decimal amount, string fromCurrency, string toCurrency)
        {
            var fromRate = GetExchangeRate(fromCurrency);
            var toRate = GetExchangeRate(toCurrency);

            if (!fromRate.HasValue || !toRate.HasValue)
            {
                throw new InvalidOperationException("Currency not found.");
            }
            
            decimal baseAmount = amount / fromRate.Value;
            return baseAmount * toRate.Value;
        }
    }
}