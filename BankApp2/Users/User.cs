using BankApp2.Models;
using System;
using System.Globalization;

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
        public List<Account> Accounts = new List<Account>();
        public List<Transaction> transactions = new List<Transaction>();
        public List<Transaction> PendingTransactions { get; set; } = new List<Transaction>();


        public User(string username, string password, decimal balance, string role)
        {
            Username = username;
            Password = password;
            Balance = balance;
            Role = role;
            Random rng = new Random();
            int random = rng.Next(999999, 9999999);
            Accounts.Add(new CheckingAccount(this, random.ToString(), 0));
            random = rng.Next(999999, 9999999);
            Accounts.Add(new SavingsAccount(this, random.ToString(), 0, 0.03m));
        }
        public void PrintAccounts(Bank bank)
        {
            try
            {
                while (true)
                {
                    Console.Clear();

                    for (int i = 0; i < Accounts.Count; i++)
                    {
                        Console.WriteLine("-----------------------------");
                        Console.WriteLine($"{i + 1}. Account number: {Accounts[i].AccountNumber}");
                        if (Accounts[i] is SavingsAccount)
                        {
                            Console.WriteLine($"Account type: Savings account");
                        }
                        else if (Accounts[i] is CheckingAccount)
                        {
                            Console.WriteLine($"Account type: Checking account");
                        }
                        Console.WriteLine($"Balance: {Accounts[i].Balance}");
                        Console.WriteLine("-----------------------------");
                    }

                    Console.Write("Account to manage: (0 to exit)");

                    if (int.TryParse(Console.ReadLine(), out int response))
                    {
                        if (response == 0)
                            break;

                        if (response > 0 && response <= Accounts.Count)
                        {
                            var selectedAccount = Accounts[response - 1];
                            AccountMenu(selectedAccount, bank);
                        }
                        else
                        {
                            Console.WriteLine("Ogiltigt val. Försök igen.");
                            Console.ReadKey();
                        }
                    }
                    else
                    {
                        Console.WriteLine("Du måste skriva ett nummer!");
                        Console.ReadKey();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ett fel uppstod: {ex.Message}");
                Console.ReadKey();
            }
        }

        public void PrintTopTransactions(int count = 3)
        {
            Console.Clear();
            var topTransactions = GetTopTransactions(count);
            Console.WriteLine($"Top {count} transactions:");
            foreach (var t in topTransactions)
            {
                if (t.Status == "Pending")
                    Console.ForegroundColor = ConsoleColor.Yellow;
                else if (t.Status == "Completed")
                    Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(t.ToString());
                Console.ForegroundColor = ConsoleColor.White;
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
            var positiveAccounts = Accounts.Where(a => a.Balance > 0);
            Console.Clear();
            Console.WriteLine("Account with positive balance:\n");

            if (positiveAccounts.Count() <= 0)
            {
                Console.WriteLine("No accounts with positive balance");
            }
            else
            {
                foreach (var account in positiveAccounts)
                {
                    Console.WriteLine("-----------------------------");
                    Console.WriteLine($"Owner: {account.Owner.Username}");
                    Console.WriteLine($"Account number: {account.AccountNumber}");
                    Console.WriteLine($"Balance: {account.Balance}");
                    Console.WriteLine("-----------------------------");
                }
            }
            Console.ReadKey();
        }

        public void ShowPendingRecurringPayments()
        {
            Console.WriteLine("Pending recurring payments:");
            foreach (var t in PendingTransactions.Where(t => t.IsRecurring))
            {
                Console.WriteLine($"To: {t.TargetAccount}, Amount: {t.Amount:C}, Next Execution: {t.NextExecutionDate}");
            }
            Console.ReadKey();
        }

        void AccountMenu(Account account, Bank bank)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine($"🏦 Account number: {account.AccountNumber}");
                string currencySymbol = account.CurrencyCode switch
                {
                    "USD" => "$",
                    "EUR" => "€",
                    "GBP" => "£",
                    "SEK" => "kr",
                    _ => account.CurrencyCode 
                };
                Console.WriteLine($"💰 Account balance: {currencySymbol} {account.Balance:N2}");
                Console.WriteLine("");
                Console.WriteLine("0.🚪 Exit");
                Console.WriteLine("1.💵 Deposit money  ");
                Console.WriteLine("2.🏧 Withdraw money  ");
                Console.WriteLine("3.🔄 Transfer money  ");
                Console.WriteLine("4.💸 Check maximum loan amount");
                Console.WriteLine("5.🏦 Take loan");
                Console.WriteLine("6.📊 Calculate loan interest");
                Console.WriteLine("7.⭐ Add favorite");
                Console.WriteLine("8.📋 Show favorites and transfer");
                Console.WriteLine("9.⚙️ Enable Autopay for bills");
                Console.WriteLine("10.⏳ View Pending Recurring Payments");
                Console.WriteLine("11.🌍 Convert Currency");

                if (account is SavingsAccount)
                {
                    Console.WriteLine("12. Calculate Interest");
                }

                string response = Console.ReadLine();
                if (response == "0")
                {
                    break;
                }
                else if (response == "1")
                {
                    try
                    {
                        account.Deposit();
                        Console.WriteLine("Insättningen lyckades!");
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine("Fel format – skriv ett giltigt belopp.");
                    }
                    catch (ArgumentException ex)
                    {
                        Console.WriteLine($"Fel: {ex.Message}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Ett oväntat fel uppstod: {ex.Message}");
                    }
                }
                else if (response == "2")
                {
                    try
                    {
                        account.Withdraw();
                        Console.WriteLine("Uttag lyckades!");
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine("Fel format – skriv ett giltigt belopp.");
                    }
                    catch (ArgumentException ex)
                    {
                        Console.WriteLine($"Fel: {ex.Message}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Ett oväntat fel uppstod: {ex.Message}");
                    }
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
                else if (response == "4")
                {
                    decimal maxLoan = account.GetMaxLoanAmount();
                    Console.WriteLine($"Current amount available to take loan for: {maxLoan.ToString("C", new CultureInfo("sv-SE"))}");
                    Console.ReadKey();
                }
                // Added max loan limit and validation Jordan
                else if (response == "5")
                {
                    Console.WriteLine($"Enter loan amount up to limit (max {account.GetMaxLoanAmount():C}):");
                    string input = Console.ReadLine();

                    if (decimal.TryParse(input, out decimal loanAmount))
                    {
                        decimal maxLoan = account.GetMaxLoanAmount();

                        if (loanAmount > 0 && loanAmount <= maxLoan)
                        {
                            account.Balance += loanAmount;

                            account.Owner.transactions.Add(new Transaction(accountNumber: account.AccountNumber, amount: loanAmount, type: "Loan"));

                            Console.WriteLine($"Loan successful! New balance is {account.Balance.ToString("C", new CultureInfo("sv-SE"))}");
                            Console.ReadKey();
                        }
                        else
                        {
                            Console.WriteLine($"Invalid loan amount. Please enter an amount up to {maxLoan:C}.");
                            Console.ReadKey();
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Invalid loan amount. Please enter a numeric value.");
                        Console.ReadKey();
                    }
                }
                // Loan interest calculation Jordan
                else if (response == "6")
                {
                    Console.WriteLine("Calculate compound interest for a loan: ");

                    Console.Write("Enter loan amount: ");
                    if (!decimal.TryParse(Console.ReadLine(), out decimal loanAmount) || loanAmount <= 0)
                    {
                        Console.WriteLine("Invalid loan amount.");
                        Console.ReadKey();
                        continue;
                    }

                    decimal annualRate = 0.12m;

                    Console.Write("Enter loan term in months: ");
                    if (!int.TryParse(Console.ReadLine(), out int months) || months <= 0)
                    {
                        Console.WriteLine("Invalid number of months.");
                        Console.ReadKey();
                        continue;
                    }

                    decimal interest = account.CalculateLoanInterest(loanAmount, annualRate, months);

                    Console.WriteLine($"Interest to pay after {months} month(s): {interest.ToString("C", new CultureInfo("sv-SE"))}");
                    Console.ReadKey();
                }

                else if (response == "7")
                {
                    Console.WriteLine("Enter name for favorite: ");
                    var alias = Console.ReadLine();
                    Console.WriteLine("Enter account number to save: ");
                    var favAccount = Console.ReadLine();
                    bank.AddFavorite(alias, favAccount);
                    Console.ReadKey();
                }
                else if (response == "8")
                {
                    if (bank.FavoriteRecipients.Count == 0)
                    {
                        Console.WriteLine("No saved favorites.");
                        Console.ReadKey();
                        continue;
                    }
                    bank.ShowFavorites();
                    Console.WriteLine("Enter favorite to transfer to: ");
                    var alias = Console.ReadLine();

                    if (bank.FavoriteRecipients.TryGetValue(alias, out string favAccount))
                    {
                        Console.WriteLine("Enter amount for transfer: ");
                        var inputAmount = Console.ReadLine();
                        if (decimal.TryParse(inputAmount, out decimal amount))
                        {
                            bank.TransferMoney(this, account.AccountNumber, favAccount, amount);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Favorite not found.");
                    }
                    Console.ReadKey();
                }
                // Added recurring payments/autopay Jordan
                else if (response == "9")
                {
                    Console.WriteLine("Set up a recurring payment: ");

                    Console.Write("Enter recipient account number: ");
                    string recipient = Console.ReadLine();

                    Console.Write("Enter the amount: ");
                    if (!decimal.TryParse(Console.ReadLine(), out decimal amount) || amount <= 0)
                    {
                        Console.WriteLine("Invalid amount.");
                        Console.ReadKey();
                        continue;
                    }

                    Console.Write("Enter interval in days: e.g., 30 for monthly: ");
                    if (!int.TryParse(Console.ReadLine(), out int intervalDays) || intervalDays <= 0)
                    {
                        Console.WriteLine("Invalid interval.");
                        Console.ReadKey();
                        continue;
                    }

                    var recurringPayment = new Transaction(account.AccountNumber, amount, "RecurringPayment", recipient)
                    {
                        IsRecurring = true,
                        IntervalDays = intervalDays,
                        NextExecutionDate = DateTime.Now.AddDays(intervalDays),
                        Status = "Pending"
                    };

                    PendingTransactions.Add(recurringPayment);

                    Console.WriteLine($"Recurring payment scheduled. Next execution: {recurringPayment.NextExecutionDate}");
                    Console.ReadKey();

                }
                // Added show pending recurring payments Jordan
                else if (response == "10")
                {
                    ShowPendingRecurringPayments();
                }
                else if (response == "12" && account is SavingsAccount)
                {
                    var savingsAccount = account as SavingsAccount;
                    savingsAccount.ShowInterest();
                }
                else if (response == "11")
                {

                    bank.ConvertCurrency(account);
                }

            }
        }
    }
}