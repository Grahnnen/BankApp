using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace BankApp2.Models
{
    public class Bank
    {
        public List<User> Users { get; set; } = new List<User>();

        public IEnumerable<Account> Accounts => Users.SelectMany(u => u.Account);

        public void OpenAccount(User user, string accountNumber)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Choose account type to open:");
                Console.WriteLine("0. Cancel");
                Console.WriteLine("1. Checking account");
                Console.WriteLine("2. Saving account");
                string response = Console.ReadLine();
                if (response == "0")
                {
                    break;
                }
                else if (response == "1")
                {
                    var account = new CheckingAccount(user, accountNumber, 0);
                    user.Account.Add(account);
                    break;
                }
                else if (response == "2")
                {
                    var account = new SavingsAccount(user, accountNumber, 0, 3);
                    user.Account.Add(account);
                    break;
                }
                
            }
        }
        public void PrintUserAccountSummaries()
        {
            Console.Clear();

            var summaries = Users
 
                .Select(u => new
                {
                    UserName = u.Username,
                    AccountCount = u.Account.Count,
                    TotalBalance = u.Account.Sum(account => account.Balance)
                });

            foreach (var summary in summaries)
            {
                Console.WriteLine("-----------------------------");
                Console.WriteLine($"Name: {summary.UserName}");
                Console.WriteLine($"Account: {summary.AccountCount}");
                Console.WriteLine($"Balance: {summary.TotalBalance:C}");
                Console.WriteLine("-----------------------------");
                Console.WriteLine("Press any key to go back");
            }

            Console.ReadKey();
        }
        public void TransferMoney(User sender, string fromAccountNumber, string toAccountNumber, decimal amount)

        {
            var fromAccount = sender.Account.FirstOrDefault(a => a.AccountNumber == fromAccountNumber);

            if (fromAccount == null)
            {
                Console.WriteLine("Account to transfer from not found.");
                return;
            }
            var receiverAccount = Users
             .SelectMany(u => u.Account)
             .FirstOrDefault(a => a.AccountNumber == toAccountNumber);

            if (receiverAccount == null)
            {
                Console.WriteLine("Receiver account not found");
                return;
            }

            // Kontrollera saldo
            if (fromAccount.Balance < amount)
            {
                Console.WriteLine("Insufficient balance.");
                return;
            }

            // Utför överföring
            fromAccount.Withdraw(amount);
            receiverAccount.Deposit(amount);

            Console.WriteLine($"Transferred {amount} kr from {fromAccountNumber} to {toAccountNumber}.");

            // Logga händelsen
            Console.WriteLine($"{sender.Username} transferred {amount} kr from {fromAccountNumber} to {toAccountNumber}");
            Console.ReadKey();       
        }

        public void FindUser(string username)
        {
            var foundUser = Users.Where(u => u.Username.Contains(username));
            if (foundUser.Count() <= 0)
            {
                Console.WriteLine("No user found!");
                Console.ReadKey();
            }
            else
            {
                Console.Clear();
                foreach (var user in foundUser)
                {
                    Console.WriteLine("----------------");
                    Console.WriteLine($"User: {user.Username}");
                    Console.WriteLine($"- Role: {user.Role}");
                    Console.WriteLine($"- Transactions: {user.transactions.Count}");
                    Console.WriteLine($"- Accounts: {user.Account.Count}");
                    foreach (var account in user.Account)
                    {
                        Console.WriteLine($" -{account.AccountNumber} ({account.Balance}kr)");
                    }
                }
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
            }
        }
        public void FindAccount(string accountNumber)
        {
            var foundAccounts = Accounts.Where(u => u.AccountNumber.Contains(accountNumber)).OrderByDescending(u => u.Balance);
            if (foundAccounts == null)
            {
                Console.WriteLine("Account not found!");
                Console.ReadKey();
            }
            else
            {
                Console.Clear();
                foreach(var account in foundAccounts)
                {
                    Console.WriteLine("----------------");
                    Console.WriteLine($"Account number: {account.AccountNumber}");
                    Console.WriteLine($"- Owner: {account.Owner.Username}");
                    Console.WriteLine($"- Balance: {account.Balance}");
                }
                Console.WriteLine("----------------");

                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
            }
        }
        public void ShowAllUsers()
        {
            if (Users.Count() <= 0)
            {
                Console.WriteLine("No user found!");
                Console.ReadKey();
            }
            else
            {
                Console.Clear();
                foreach (var user in Users)
                {
                    Console.WriteLine("----------------");
                    Console.WriteLine($"User: {user.Username}");
                    Console.WriteLine($"- Role: {user.Role}");
                    Console.WriteLine($"- Accounts: {user.Account.Count}");
                    Console.WriteLine($"- Transactions: {user.transactions.Count}");
                }
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
            }
        }
        public void FindUserWithTopTransactions()
        {
            var foundUser = Users.OrderByDescending(u => u.transactions.Count).FirstOrDefault(); Console.Clear();
            Console.Clear();
            if (foundUser != null)
            {
                Console.WriteLine("----------------");
                Console.WriteLine($"User: {foundUser.Username}");
                Console.WriteLine($"- Role: {foundUser.Role}");
                Console.WriteLine($"- Accounts: {foundUser.Account.Count}");
                Console.WriteLine($"- Transactions: {foundUser.transactions.Count}");
            }
            else
            {
                Console.WriteLine("No users found.");
            }
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
    }
}
