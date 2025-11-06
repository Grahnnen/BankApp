using BankApp2.Models;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.Linq;

namespace BankApp2
{
    public class Account 
    {
        public string AccountNumber { get; set; }
        public decimal Balance { get; set; }
        public User Owner { get; set; }
        public string CurrencyCode { get; set; } = "SEK";

        public string AccountName { get; private set; }

        public Account(User user, string accountNumber, decimal balance, string accountName = "Standard Account", string currency = "SEK")
        {
            AccountNumber = accountNumber;
            Balance = balance;
            Owner = user;
            CurrencyCode = currency;
            AccountName = "Unnamed Account";
        }

        public void RenameAccount(string newName)
        {
            if (!string.IsNullOrWhiteSpace(newName))
            {
                AccountName = newName.Trim();
                Console.WriteLine($"Changed account name to: {AccountName}");
            }
            else
            {
                Console.WriteLine("Invalid name, try again.");
            }
        }
        // Deposit money into the account user inputs amount in console
        public void Deposit()
        {
            Console.Clear();
            try
            {
                Console.Write("Enter the amount to deposit: ");
                if (int.TryParse(Console.ReadLine(), out int amount))
                {
                    if (amount <= 0)
                    {
                        Console.WriteLine("Deposit failed: amount must be greater than 0.");
                        return;
                    }
                    Balance += amount;
					Owner.transactions.Add(new Transaction(
                        AccountNumber, 
                        amount, 
                        "Deposit"		
					)
					{ Status = "Completed" });
					Console.WriteLine($"Deposit successful: new balance is {Balance}.");
                    Console.WriteLine("\nPress any key to continue...");
                    Console.ReadKey();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error has occured: {ex.Message}");
                Console.ReadKey();
            }
        }
        // Deposit money directly by passing amount as a parameter
        public void Deposit(decimal amount)
		{
			try
			{
				if (amount <= 0)
				{
					Console.WriteLine("Deposit failed: amount must be greater than 0.");
					return;
				}
				Balance += amount;

				Console.WriteLine($"Deposit successful: new balance is {Balance}.");
			}
			catch (Exception ex)
			{
				Console.WriteLine($"An error has occured: {ex.Message}");
				Console.ReadKey();
			}
		}

        // Withdraw money user inputs amount in console
        public virtual void Withdraw()
        {
            try
            {
                Console.Clear();
                Console.Write("Enter the amount to withdraw: ");
                if (decimal.TryParse(Console.ReadLine(), out decimal amount))
                {
                    if (amount <= 0)
                    {
                        Console.WriteLine("Withdrawal failed: amount must be greater than 0.");
                        return;
                    }
                    if (amount > Balance)
                    {
                        Console.WriteLine("Withdrawal failed: insufficient funds.");
                        return;
                    }

                    Balance -= amount;
                    // Log the withdrawal as a transaction
                    Owner.transactions.Add(new Transaction(
					                AccountNumber, 
                                    amount, 
                                    "Withdraw"
					)
					{ Status = "Completed" });
                    Console.WriteLine($"Withdrawal successful: new balance is {Balance}.");
                    Console.WriteLine("\nPress any key to continue...");
                    Console.ReadKey();
                }
                else
                {
                    Console.WriteLine("Invalid amount.");
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine($"An error has occured: {ex.Message}");
                Console.ReadKey();
            }
        }

        // Withdraw money directly by passing amount as a parameter
        public virtual void Withdraw(decimal amount)
        {
            try
            {
                if (amount <= 0)
                {
                    Console.WriteLine("Withdrawal failed: amount must be greater than 0.");
                    return;
                }
                if (amount > Balance)
                {
                    Console.WriteLine("Withdrawal failed: insufficient funds.");
                    return;
                }

                Balance -= amount;
                Owner.transactions.Add(new Transaction(AccountNumber, amount, "Outgoing transfer"));
            }
            catch (Exception ex)
            {

                Console.WriteLine($"An error has occured: {ex.Message}");
                Console.ReadKey();
            }
        }
        // Calculate the maximum loan amount based on balance
        public virtual decimal GetMaxLoanAmount()
        {
	        return Balance * 5;
        }

        public decimal CalculateLoanInterest(decimal principal, decimal annualRate, int months)
        {
	        decimal monthlyRate = annualRate / 12;
	        decimal futureValue = principal * (decimal)Math.Pow((double)(1 + monthlyRate), months);
	        return futureValue - principal;
        }
	}
}