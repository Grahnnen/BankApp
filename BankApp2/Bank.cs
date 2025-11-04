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
        public CurrencyManager CurrencyManager { get; set; } = new CurrencyManager();

        public IEnumerable<Account> Accounts => Users.SelectMany(u => u.Accounts);
        
        public Bank()
		{
			Task.Run(ProcessPendingTransactions);
		}
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
                    user.Accounts.Add(account);
                    break;
                }
                else if (response == "2")
                {
                    var account = new SavingsAccount(user, accountNumber, 0, 0.03m);
                    user.Accounts.Add(account);
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
                    AccountCount = u.Accounts.Count,
                    TotalBalance = u.Accounts.Sum(account => account.Balance)
                });

            foreach (var summary in summaries)
            {
                Console.WriteLine("-----------------------------");
                Console.WriteLine($"Name: {summary.UserName}");
                Console.WriteLine($"Account: {summary.AccountCount}");
                Console.WriteLine($"Balance: {summary.TotalBalance:C}");
            }

            Console.WriteLine("-----------------------------");
            Console.WriteLine("Press any key to go back");
            Console.ReadKey();
        }
        public void TransferMoney(User sender, string fromAccountNumber, string toAccountNumber, decimal amount)
        {
            var fromAccount = sender.Accounts.FirstOrDefault(a => a.AccountNumber == fromAccountNumber);
            if (fromAccount == null)
            {
                Console.WriteLine("Account to transfer from not found.");
                Console.ReadKey();
                return;
            }
            var receiverAccount = Users
                .SelectMany(u => u.Accounts)
                .FirstOrDefault(a => a.AccountNumber == toAccountNumber);

            if (receiverAccount == null)
            {
                Console.WriteLine("Receiver account not found");
                Console.ReadKey();
                return;
            }

            if (fromAccount.Balance < amount)
            {
                Console.WriteLine("Insufficient balance.");
                Console.ReadKey();
                return;
            }

            var transaction = new Transaction(fromAccountNumber, amount, "Outgoing Transfer", toAccountNumber)
            {
                Status = "Pending",
                ScheduledCompletionTime = DateTime.Now.AddSeconds(20)
            };

            sender.PendingTransactions.Add(transaction);
            sender.transactions.Add(transaction);

            var incomingTransaction = new Transaction(fromAccountNumber, amount, "Incoming Transfer", toAccountNumber)
            {
                Status = "Pending",
                ScheduledCompletionTime = DateTime.Now.AddSeconds(20)
            };
            receiverAccount.Owner.transactions.Add(incomingTransaction);

            Console.WriteLine($"Transferred {amount} kr from {fromAccountNumber} to {toAccountNumber}.");
            Console.ReadKey();
        }

        public virtual void CancelTransaction(User user)
        {
            var pending = user.PendingTransactions.Where(t => t.Status == "Pending").ToList();

            if (!pending.Any())
            {
                Console.WriteLine("No pending transactions.");
                return;
            }

            Console.WriteLine("Choose transaction to cancel:");
            for (int i = 0; i < pending.Count; i++)
            {
                var t = pending[i];
                Console.WriteLine($"{i + 1}. {t.Amount} kr from {t.AccountNumber} to {t.TargetAccount} (Scheduled: {t.ScheduledCompletionTime})");
            }

            Console.Write("\nYour Choice: ");
            if (int.TryParse(Console.ReadLine(), out int choice) && choice > 0 && choice <= pending.Count)
            {
                var transaction = pending[choice - 1];
                transaction.Status = "Cancelled";
                user.PendingTransactions.Remove(transaction);
                Console.WriteLine("✅ Transactionen canceled.");
            }
            else
            {
                Console.WriteLine("❌ Ogiltigt val.");
            }
        }
        public Dictionary<string, string> FavoriteRecipients { get; } = new Dictionary<string, string>();

        public void AddFavorite(string alias, string accountNumber)
        {
            if (string.IsNullOrWhiteSpace(alias) || string.IsNullOrWhiteSpace(accountNumber))
            {
                Console.WriteLine("Type name and account number to add favorite.");
                return;
            }

            if (!FavoriteRecipients.ContainsKey(alias))
            {
                FavoriteRecipients[alias] = accountNumber;
                Console.WriteLine($"Favorite '{alias}' saved for account {accountNumber}.");
            }
            else
            {
                Console.WriteLine("Favorite already exist");
            }
        }

        public void ShowFavorites()
        {
            Console.WriteLine("Saved favorites:");
            foreach (var fav in FavoriteRecipients)
            {
                Console.WriteLine($"{fav.Key}, {fav.Value}");
            }
        }


        private async Task ProcessPendingTransactions()
		{
			while (true)
			{
				var now = DateTime.Now;
				foreach (var user in Users)
				{
					var toProcess = user.PendingTransactions
						.Where(t => t.Status == "Pending" && t.ScheduledCompletionTime <= now)
						.ToList();

					foreach (var transaction in toProcess)
					{
                        var fromAccount = user.Accounts.FirstOrDefault(a => a.AccountNumber == transaction.AccountNumber);
                        var toAccount = Users.SelectMany(u => u.Accounts)
                                     .FirstOrDefault(a => a.AccountNumber == transaction.TargetAccount);

                        if (fromAccount != null && toAccount != null)
                        {
                            fromAccount.Withdraw(transaction.Amount);
                            toAccount.Deposit(transaction.Amount);

                            var receiverUser = toAccount.Owner;
                            receiverUser.transactions.Add(new Transaction(
                                toAccount.AccountNumber,
                                transaction.Amount,
                                "Incoming transfer",
                                fromAccount.AccountNumber
                            )
                            { Status = "Completed" });

                            receiverUser.transactions.RemoveAll(t =>
                                t.AccountNumber == transaction.AccountNumber &&
                                t.TargetAccount == transaction.TargetAccount &&
                                t.Amount == transaction.Amount &&
                                t.Status == "Pending"
                            );
                            receiverUser.PendingTransactions.RemoveAll(t =>
                                t.AccountNumber == transaction.AccountNumber &&
                                t.TargetAccount == transaction.TargetAccount &&
                                t.Amount == transaction.Amount &&
                                t.Status == "Pending"
                            );
                        }
                        transaction.Status = "Completed";
					}
					user.PendingTransactions.RemoveAll(t => t.Status == "Completed");
				}
				await Task.Delay(TimeSpan.FromSeconds(1));
			}
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
                    string status = user.IsSuspended ? "⛔ Avstängd" : "✅ Aktiv";
                    Console.WriteLine($"User: {user.Username} ({status})");
                    Console.WriteLine($"- Email : {user.Email}");
                    Console.WriteLine($"- Role: {user.Role}");
                    Console.WriteLine($"- Transactions: {user.transactions.Count}");
                    Console.WriteLine($"- Accounts: {user.Accounts.Count}");
                    foreach (var account in user.Accounts)
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
            var foundAccounts = Accounts.Where(u => u.AccountNumber.Contains(accountNumber));
            if (foundAccounts.Count() <= 0)
            {
                Console.WriteLine("No accounts found!");
                Console.ReadKey();
            }
            else
            {
                Console.Clear();
                foreach (var account in foundAccounts)
                {
                    Console.WriteLine("----------------");
                    string status = account.Owner.IsSuspended ? "⛔ Avstängd" : "✅ Aktiv";
                    Console.WriteLine($"Account number: {account.AccountNumber}");
                    Console.WriteLine($"- Owner: {account.Owner.Username} ({status})");
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
                Console.WriteLine("No users found!");
                Console.ReadKey();
            }
            else
            {
                Console.Clear();
                foreach (var user in Users)
                {
                    string status = user.IsSuspended ? "⛔ Avstängd" : "✅ Aktiv";
                    Console.WriteLine($"User: {user.Username} ({status})");
                    Console.WriteLine($"- Email: {user.Email}");
                    Console.WriteLine($"- Role: {user.Role}");
                    Console.WriteLine($"- Transactions: {user.transactions.Count}");
                    Console.WriteLine($"- Accounts: {user.Accounts.Count}");
                    foreach (var account in user.Accounts)
                    {
                        Console.WriteLine($" -{account.AccountNumber} ({account.Balance}kr)");
                    }
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
                Console.WriteLine($"- Accounts: {foundUser.Accounts.Count}");
                Console.Write($"- Transactions: ");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(foundUser.transactions.Count);
                Console.ForegroundColor = ConsoleColor.White;
            }
            else
            {
                Console.WriteLine("No users found.");
            }
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        public void ShowBanksBiggestTransactions()
        {
            Console.Clear();

            var allTransactions = Users
                .SelectMany(u => u.transactions)
                .OrderByDescending(t => t.Amount)
                .Take(3)
                .ToList();

            if (allTransactions.Count == 0)
            {
                Console.WriteLine("No transactions found.");
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("Top 3 transactions across the bank:");
            int rank = 1;
            foreach (var transactions in allTransactions)
            {
                var ownerName = Accounts.FirstOrDefault(a => a.AccountNumber == transactions.AccountNumber).Owner.Username;

                Console.WriteLine("----------------");
                Console.WriteLine($"#{rank} Amount: {transactions.Amount:C}");
                Console.WriteLine($"Owner: {ownerName}");
                Console.WriteLine($"Type: {transactions.Type}");
                Console.WriteLine($"From: {transactions.AccountNumber}");
                Console.WriteLine($"To: {transactions.TargetAccount}");
                Console.WriteLine($"Status: {transactions.Status}");
                Console.WriteLine($"Date: {transactions.DateTime}");
                rank++;
            }
            Console.WriteLine("----------------");

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
        public void ConvertCurrency(Account account)
        {


            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Yellow;
            while (true)
            {

                Console.Clear();
                Console.WriteLine("=== Convert Currency 🌍 ===");
                Console.WriteLine($"Current balance: {account.Balance:F2} {account.CurrencyCode}");
                Console.WriteLine();
                Console.WriteLine("\n[Press enter to Exit]");
                Console.Write("Enter target currency (SEK| USD| EUR| GBP): ");
                string? newCurrency = Console.ReadLine()?.ToUpper();
                if (string.IsNullOrWhiteSpace(newCurrency))
                {
                    break;
                }
                var currencyManager = CurrencyManager;
                var currentRate = currencyManager.GetExchangeRate(account.CurrencyCode);
                var targetRate = currencyManager.GetExchangeRate(newCurrency);

                if (!currentRate.HasValue || !targetRate.HasValue)
                {
                    Console.WriteLine("Currency not found, press enter to try again!");
                    Console.ReadKey();
                    continue;
                }

                decimal newBalance = currencyManager.ConvertCurrency(account.Balance, account.CurrencyCode, newCurrency);

                account.Balance = newBalance;
                account.CurrencyCode = newCurrency;

                Console.WriteLine();
                Console.WriteLine($"✅ Your balance is now {newBalance:F2} {newCurrency}");
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();

            }



        }
    }
}
