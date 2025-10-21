using BankApp2.Models;
using System;

namespace BankApp2
{
    public class User
    {

        public string Username { get; set; }
        public string Password { get; set; }
        public decimal Balance { get; set; }
        public string Role { get; set; }
        public int FailedAttempts { get; set; } = 0;
        public bool IsLocked { get; set; } = false;
        public List<Account> Account = new List<Account>();
        public List<Transaction> transactions = new List<Transaction>();


        public User(string username, string password, decimal balance, string role)
        {
            Username = username;
            Password = password;
            Balance = balance;
            Role = role;
            Random rng = new Random();
            int random = rng.Next(999999, 9999999);
            Account.Add(new CheckingAccount(this, random.ToString(), 0));
            random = rng.Next(999999, 9999999);
            Account.Add(new SavingsAccount(this, random.ToString(), 0, 0.03m));
        }
        public void PrintAccounts(Bank bank)
        {
            while (true)
            {
                Console.Clear();
                for (int i = 0; i < Account.Count; i++)
                {
                    Console.WriteLine("-----------------------------");
                    Console.WriteLine($"{i + 1}. Account number: {Account[i].AccountNumber}");
                    if (Account[i] is SavingsAccount)
                    {
                        Console.WriteLine($"Account type: Savings account");
                    }
                    else if (Account[i] is CheckingAccount)
                    {
                        Console.WriteLine($"Account type: Checking account");
                    }
                    Console.WriteLine($"Balance: {Account[i].Balance}");
                    Console.WriteLine("-----------------------------");
                }
                Console.Write("Account to manage: (0 to exit)");

                if (int.TryParse(Console.ReadLine(), out int response))
                {
                    if (response == 0)
                        break;
                    if (Account.Count >= response)
                    {
                        var selectedAccount = Account[response - 1];
                        AccountMenu(selectedAccount, bank);

                    }
                }

            }
        }
        public void PrintTopTransactions(int count = 3)
        {
            Console.Clear();
            var topTransactions = GetTopTransactions(count);
            Console.WriteLine($"Top {count} transactions:");
            foreach (var t in topTransactions)
            {
                Console.WriteLine(t.ToString());
            }
            Console.ReadKey();
        }
        public List<Transaction> GetTopTransactions(int topCount)
        {
            return transactions
                .OrderByDescending(t => t.Amount)
                .Take(topCount)
                .ToList();
        }
        public void PrintPositiveAccounts()
        {
            var positiveAccounts = Account.Where(a => a.Balance > 0);
            Console.Clear();
            Console.WriteLine("Konton med positivt saldo:\n");

            if (positiveAccounts.Count() <= 0)
            {
                Console.WriteLine("Inga konton med postivt saldo");
            }
            else
            {
                foreach (var account in positiveAccounts)
                {
                    Console.WriteLine("-----------------------------");
                    Console.WriteLine($"Ägare: {account.Owner.Username}");
                    Console.WriteLine($"Kontonummer: {account.AccountNumber}");
                    Console.WriteLine($"Saldo: {account.Balance}");
                    Console.WriteLine("-----------------------------");
                }
            }
            Console.ReadKey();
        }

        void AccountMenu(Account account, Bank bank)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine($"Accountnumber: {account.AccountNumber}");
                Console.WriteLine($"Account balance: {account.Balance}");
                Console.WriteLine("0. Exit");
                Console.WriteLine("1. Deposit money");
                Console.WriteLine("2. Withdraw money");
                Console.WriteLine("3. Transfer money");
                if (account is SavingsAccount)
                {
                    Console.WriteLine("4. Calculate Interest");
                }
                string response = Console.ReadLine();
                if (response == "0")
                {
                    break;
                }
                else if (response == "1")
                {

                    account.Deposit();
                }
                else if (response == "2")
                {
                    account.Withdraw();
                }
                else if (response == "3")
                {
                    Console.WriteLine("Enter Account number: ");
                    var accountNumber = Console.ReadLine();
                    Console.WriteLine("Enter amount for transfer: ");
                    var inputAmount = (Console.ReadLine());

                    if (decimal.TryParse(inputAmount, out decimal amount))
                    {
                      bank.TransferMoney(this, account.AccountNumber, accountNumber, amount);

                    }

                }
                else if (response == "4" && account is SavingsAccount)
                {
                    var savingsAccount = account as SavingsAccount;
                    savingsAccount.ShowInterest();
                }

            }
        }
        
    }
}