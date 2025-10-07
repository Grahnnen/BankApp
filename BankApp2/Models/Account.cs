
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.Transactions;

namespace BankApp2
{
    public class Account
    {
        public string AccountNumber { get; set; }
        public decimal Balance { get; set; }
        public User Owner { get; set; }


        public List<string> transactions = new List<string>();

        public Account(User user, string accountNumber, decimal balance)
        {
            AccountNumber = accountNumber;
            Balance = balance;
        }

        public void Deposit()
        {
            Console.Clear();
            Console.Write("Enter the amount to deposit: ");
            if (int.TryParse(Console.ReadLine(), out int amount))
            {
                if (amount <= 0)
                {
                    Console.WriteLine("Deposit failed: amount must be greater than 0.");
                    return;
                }
                Balance += amount;
                transactions.Add($"Deposited {amount} to account: {AccountNumber}");
                Console.WriteLine($"Deposit successful: new balance is {Balance}.");
            }


        }
        public virtual void Withdraw()
        {
            Console.Clear();
            Console.Write("Enter the amount to withdraw: ");
            if (int.TryParse(Console.ReadLine(), out int amount))
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
                Console.WriteLine($"Withdrawal successful: new balance is {Balance}.");
            }
        }
        public void TransferMoney()
        {
            
        }

        public List<Transaction> GetTopTransactions(int topCount)
        {
            return transactions
                .OrderByDescending(t => t.amount.ToString())
                .Take(topCount)
                .ToList();
        }
    }
}
