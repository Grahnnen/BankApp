
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


        public Account(User user, string accountNumber, decimal balance)
        {
            AccountNumber = accountNumber;
            Balance = balance;
            Owner = user;
        }

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
                    Owner.transactions.Add(new Transaction
                    (
                        accountNumber: AccountNumber,
                        amount: amount,
                        type: "Deposit"
                    ));
                    Console.WriteLine($"Deposit successful: new balance is {Balance}.");
                    Console.WriteLine("\nPress any key to continue...");
                    Console.ReadKey();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ett oväntat fel uppstod: {ex.Message}");
                Console.ReadKey();
            }
        }

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
                Owner.transactions.Add(new Transaction
    (
                    accountNumber: AccountNumber,
                    amount: amount,
                    type: "Incoming transfer"
                ));
                Console.WriteLine($"Deposit successful: new balance is {Balance}.");

            }
            catch (Exception ex)
            {

                Console.WriteLine($"Ett oväntat fel uppstod {ex.Message}");
                Console.ReadKey();

            }
        }


        // 1️⃣ Version där användaren skriver in summan i konsolen
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
                    Owner.transactions.Add(new Transaction(AccountNumber, amount, "Withdraw"));
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

                Console.WriteLine($"Ett oväntat fel uppstod {ex.Message}");
                Console.ReadKey();
            }
        }

        // 2️⃣ Version som tar emot belopp direkt (för överföring)
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

                Console.WriteLine($"Ett oväntat fel uppstod {ex.Message}");
                Console.ReadKey();
            }
        }
        
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

