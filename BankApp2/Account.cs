
using System.Security.Cryptography.X509Certificates;
using System.Transactions;

namespace BankApp2
{
    public class Account
    {
        public string AccountNumber { get; set; }
        public decimal Balance { get; set; }
        public User Owner { get; set; }


        List<Transaction> transactions = new List<Transaction>();

        public Account(string accountNumber, decimal balance)
        {
            AccountNumber = accountNumber;
            Balance = balance;
        }

        public void Deposit()
        {
            Console.Clear();
            Console.WriteLine("Enter the amount to deposit");
            if (int.TryParse(Console.ReadLine(), out int amount))
            {
                if (amount <= 0)
                {
                    Console.WriteLine("Deposit failed: amount must be greater than 0.");
                    return;
                }
                Balance += amount;
                Console.WriteLine($"Deposit successful: new balance is {Balance}.");
            }


        }
        public void Withdraw(decimal amount)
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
        public void TransferMoney(Account fromAccount, Account toAccount, decimal amount)
        {
            toAccount.Balance += amount;
            fromAccount.Balance -= amount;
            Console.WriteLine($"Transfered {amount} from {fromAccount} to {toAccount}");
        }
    }
}
