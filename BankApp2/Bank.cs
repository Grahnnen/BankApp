using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace BankApp2
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
                Console.WriteLine("1. Checking account");
                Console.WriteLine("2. Saving account");
                Console.WriteLine("3. Cancel");
                string response = Console.ReadLine();
                if (response == "1")
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
                else if (response == "3")
                {
                    break;
                }
            }
        }
        public void PrintAccounts(User user)
        {
            Console.Clear();
            for (int i = 0; i < user.Account.Count; i++)
            {
                Console.WriteLine("-----------------------------");
                Console.WriteLine($"Account number: {user.Account[i].AccountNumber}");
                if (user.Account[i] is SavingsAccount)
                {
                    Console.WriteLine($"Account type: Savings account");
                }
                else if (user.Account[i] is CheckingAccount)
                {
                    Console.WriteLine($"Account type: Checking account");
                }
                Console.WriteLine($"Balance: {user.Account[i].Balance}");
                Console.WriteLine("-----------------------------");
            }
            Console.ReadKey();
        }

        public void FindAccount(string id)
        {
            foreach (var user in Users)
            {
                if (user.id == id)
                    Console.WriteLine("hittade user");

                else
                {
                    Console.WriteLine("hittade inte user");
                }
            }
        }

        public void PrintPositiveAccounts()
        {
            var positiveAccounts = Accounts.Where(a => a.Balance > 0);
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
                    Console.WriteLine($"Ägare: {account.Owner}");
                    Console.WriteLine($"Kontonummer: {account.AccountNumber}");
                    Console.WriteLine($"Saldo: {account.Balance}");
                }
            }
            Console.ReadKey();
        }
    }
}
