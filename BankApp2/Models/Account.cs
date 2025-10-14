
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



        public Account(User user, string accountNumber, decimal balance)
        {
            AccountNumber = accountNumber;
            Balance = balance;
            Owner = user;
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
                Owner.transactions.Add(new Transaction
                (   
                    accountNumber: AccountNumber,
                    amount: amount,
                    type: "Deposit"
                ));
                Console.WriteLine($"Deposit successful: new balance is {Balance}.");
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        public void Deposit(decimal amount)
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

        }




        // 1️⃣ Version där användaren skriver in summan i konsolen
        public virtual void Withdraw()
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
               

            }
            else
            {
                Console.WriteLine("Invalid amount.");
            }
        }

        // 2️⃣ Version som tar emot belopp direkt (för överföring)
        public virtual void Withdraw(decimal amount)
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
        }
         

    }
}

