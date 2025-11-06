using BankApp2.Models;
using System;
using System.Globalization;

namespace BankApp2
{
    public class User
    {
        // The date and time when the user account was created
        public DateTime CreatedDate { get; set; }
        // The next scheduled date when the user must change their password
        public DateTime NextPasswordChangeDate { get; set; }

        // Basic user credentials and information
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; } // Defines user role, e.g., admin or customer
        public string Email { get; set; }
        // Tracks failed login attempts and account status flags
        public int FailedAttempts { get; set; } = 0;
        public bool IsLocked { get; set; } = false;
        public bool IsSuspended { get; set; } = false;

        // Toggles the user's suspension status (activated <-> suspended)
        public void ToggleSuspension()
        {
            if (IsSuspended)
            {
                IsSuspended = false;
            }
            else
            {
                IsSuspended = true;
            }
            
            string status = IsSuspended ? "Suspended" : "Activated";
            Console.WriteLine($"The account {Username} has now been {status}.");
        }

        // Stores all user accounts, transaction history, and pending transactions
        public List<Account> Accounts = new List<Account>();
        public List<Transaction> transactions = new List<Transaction>();
        public List<Transaction> PendingTransactions { get; set; } = new List<Transaction>();

        // Constructor for creating a new user and automatically generating default accounts
        public User(string username, string password, string role, string email = "")
        {
            Username = username;
            Password = password;
            Role = role;
            Email = email;

            // Record creation time and set next password change
            CreatedDate = DateTime.Now; 
            NextPasswordChangeDate = CreatedDate.AddMinutes(90); 

            // Create a checking and savings account with random account numbers
            Random rng = new Random();
            int random = rng.Next(999999, 9999999);
            Accounts.Add(new CheckingAccount(this, random.ToString(), 0));
            random = rng.Next(999999, 9999999);
            Accounts.Add(new SavingsAccount(this, random.ToString(), 0, 0.03m));
        }

        // Displays all user accounts and allows account-specific management options
        public void PrintAccounts(Bank bank)
        {
            try
            {
                while (true)
                {
                    Console.Clear();
                    // List all accounts with details
                    for (int i = 0; i < Accounts.Count; i++)
                    {
                        Console.WriteLine("-----------------------------");
                        Console.WriteLine($"{i + 1}. Account Name: {Accounts[i].AccountName}");
                        Console.WriteLine($"Account number: {Accounts[i].AccountNumber}");

                        // Display account type (Savings or Checking)
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

                    // Handle account selection
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
                            Console.WriteLine("Please enter a valid account choice!");
                            Console.ReadKey();
                        }
                    }
                    else
                    {
                        Console.WriteLine("Please enter a valid account choice!");
                        Console.ReadKey();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error has occured: {ex.Message}");
                Console.ReadKey();
            }
        }

        // Displays the top N largest transactions for the user
        public void PrintTopTransactions(int count = 3)
        {
            Console.Clear();
            var topTransactions = GetTopTransactions(count);
            Console.WriteLine($"Top {count} transactions:");

            // Display transactions in color-coded status
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

        // Returns a list of the top N transactions ordered by amount
        public List<Transaction> GetTopTransactions(int topCount)
        {
            return transactions
                .OrderByDescending(t => t.Amount)
                .Take(topCount)
                .ToList();
        }

        // Prints all accounts that currently have a positive balance
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

        // Displays all recurring payments that are still pending
        public void ShowPendingRecurringPayments()
        {
            Console.WriteLine("Pending recurring payments:");
            foreach (var t in PendingTransactions.Where(t => t.IsRecurring))
            {
                Console.WriteLine($"To: {t.TargetAccount}, Amount: {t.Amount:C}, Next Execution: {t.NextExecutionDate}");
            }
            Console.ReadKey();
        }

        // Checks if the user is due for a password change based on schedule
        public bool IsPasswordChangeDue() 
        {
            return DateTime.Now >= NextPasswordChangeDate;
        }

        // Displays a detailed account management menu for deposits, withdrawals, transfers, etc.
        void AccountMenu(Account account, Bank bank)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine($"🏦 Account number: {account.AccountNumber}");

                // Selects the proper currency symbol for display
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
                Console.WriteLine("4.❌ Cancel Transfer");
                Console.WriteLine("5.💸 Check maximum loan amount");
                Console.WriteLine("6.🏦 Take loan");
                Console.WriteLine("7.📊 Calculate loan interest");
                Console.WriteLine("8.⭐ Add favorite");
                Console.WriteLine("9.📋 Show favorites and transfer");
                Console.WriteLine("10.⚙️ Enable Autopay for bills");
                Console.WriteLine("11.⏳ View Pending Recurring Payments");
                Console.WriteLine("12.🌍 Convert Currency");
                Console.WriteLine("13.🔃Rename Account");

                // Option for savings accounts to calculate interest
                if (account is SavingsAccount)
                {
                    Console.WriteLine("14.🟰Calculate Interest");
                }

                string response = Console.ReadLine();

                // Exit option
                if (response == "0")
                {
                    break;
                }

                // Deposit operation
                else if (response == "1")
                {
                    try
                    {
                        account.Deposit();
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine("Wrong format!.");
                    }
                    catch (ArgumentException ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"An error has occured: {ex.Message}");
                    }
                }

                // Withdrawal operation
                else if (response == "2")
                {
                    try
                    {
                        account.Withdraw();
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine("Wrong format!.");
                    }
                    catch (ArgumentException ex)
                    {
                        Console.WriteLine($"An error has occured: {ex.Message}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"An error has occured: {ex.Message}");
                    }
                }

                // Handles money transfer between accounts
                else if (response == "3")
                {
                    Console.Clear();
                    Console.WriteLine("Transfering money.");
                    Console.Write("Enter Account number: ");
                    var accountNumber = Console.ReadLine();
                    Console.Write("Enter amount for transfer: ");
                    var inputAmount = (Console.ReadLine());

                    if (decimal.TryParse(inputAmount, out decimal amount))
                    {
                        bank.TransferMoney(this, account.AccountNumber, accountNumber, amount);
                    }
                }

                // Cancels an existing pending transaction
                else if (response == "4")
                {
                    Console.Clear();
                    bank.CancelTransaction(this);
                    Console.ReadKey();
                }

                // Displays the user's maximum eligible loan amount
                else if (response == "5")
                {
                    Console.Clear();
                    decimal maxLoan = account.GetMaxLoanAmount();
                    Console.WriteLine($"Current amount available to take loan for: {maxLoan.ToString("C", new CultureInfo("sv-SE"))}");
                    Console.ReadKey();
                }

                // Processes a loan request with validation and updates balance if approved
                else if (response == "6")
                {
                    Console.Clear();
                    Console.WriteLine($"Enter loan amount up to limit (max {account.GetMaxLoanAmount():C}):");
                    string input = Console.ReadLine();

                    if (decimal.TryParse(input, out decimal loanAmount))
                    {
                        decimal maxLoan = account.GetMaxLoanAmount();

                        if (loanAmount > 0 && loanAmount <= maxLoan)
                        {
                            Console.WriteLine("Processing your loan application...");
                            Thread.Sleep(1000);
                            if (loanAmount <= account.Balance * 5)
                            {
                                Console.WriteLine("Loan approved! Funds will be deposited shortly.");
                                account.Balance += loanAmount;

                                account.Owner.transactions.Add(new Transaction(accountNumber: account.AccountNumber, amount: loanAmount, type: "Loan"));
                                Console.WriteLine($"New balance: {account.Balance.ToString("C", new CultureInfo("sv-SE"))}");
                            }
                            else
                            {
                                Console.WriteLine("Loan denied: Requested amount is too high compared to your current account balance.");
                            }
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

                // Calculates compound interest for a loan
                else if (response == "7")
                {
                    Console.Clear();
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

                // Adds a recipient account to user's favorites
                else if (response == "8")
                {
                    Console.Clear();
                    Console.WriteLine("Enter name for favorite: ");
                    var alias = Console.ReadLine();
                    Console.WriteLine("Enter account number to save: ");
                    var favAccount = Console.ReadLine();
                    bank.AddFavorite(alias, favAccount);
                    Console.ReadKey();
                }

                // Displays saved favorite recipients and allows transfers to them
                else if (response == "9")
                {
                    Console.Clear();

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

                // Sets up a recurring automatic payment
                else if (response == "10")
                {
                    Console.Clear();

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
                // Shows all pending recurring payments
                else if (response == "11")
                {
                    Console.Clear();

                    ShowPendingRecurringPayments();
                }
                
                // Converts account balance into a different currency
                else if (response == "12")
                {
                    Console.Clear();

                    bank.ConvertCurrency(account);
                }

                // Renames the selected account
                else if (response == "13")
                {
                    Console.Clear();
                    Console.Write("Choose new account name: ");
                    string newName = Console.ReadLine();
                    account.RenameAccount(newName);
                    Console.WriteLine("Press any key to continue..");
                    Console.ReadKey();
                }

                // Displays calculated interest for a savings account
                else if (response == "14" && account is SavingsAccount)
                {
                    Console.Clear();
                    var savingsAccount = account as SavingsAccount;
                    savingsAccount.ShowInterest();
                }
                
            }
        }

        // Allows user to set or update their email address
        public void SetEmail()
        {
            Console.Clear();
            Console.WriteLine("=== Set Email Address ===");
    
            if (!string.IsNullOrWhiteSpace(Email))
            {
                Console.WriteLine($"Current email: {Email}");
            }
    
            Console.Write("Enter your email address (or press Enter to cancel): ");
            string newEmail = Console.ReadLine();
    
            if (string.IsNullOrWhiteSpace(newEmail))
            {
                Console.WriteLine("Email update cancelled.");
                Console.ReadKey();
                return;
            }
    
            if (IsValidEmail(newEmail))
            {
                Email = newEmail;
                Console.WriteLine($"✅ Email successfully set to: {Email}");
            }
            else
            {
                Console.WriteLine("❌ Invalid email format. Please try again.");
            }
    
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        // Validates email format using a regex
        private bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {   // Regex taken from: https://regex101.com/r/lHs2R3/1
                return System.Text.RegularExpressions.Regex.IsMatch(email, 
                    @"^[\w\-\.]+@([\w-]+\.)+[\w-]{2,}$");
            }
            catch
            {
                return false;
            }
        }
    }
}