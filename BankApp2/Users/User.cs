using BankApp2.Models;

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
            Account.Add(new SavingsAccount(this, random.ToString(), 0, 3));
        }
        public void PrintAccounts()
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
                
                if(int.TryParse(Console.ReadLine(), out int response))
                {
                    if (response == 0)
                        break;
                    if (Account.Count >= response)
                    {
                        var selectedAccount = Account[response - 1];
                        AccountMenu(selectedAccount);

                    } 
                }
                
            }
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

        void AccountMenu(Account account)
        {
            while (true) {
                Console.Clear();
                Console.WriteLine($"Accountnumber: {account.AccountNumber}");
                Console.WriteLine($"Account balance: {account.Balance}");
                Console.WriteLine("0. Exit");
                Console.WriteLine("1. Deposit money");
                Console.WriteLine("2. Withdraw money");
                Console.WriteLine("3. Transfer money");
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
                    account.TransferMoney();
            }
        }
    }
}