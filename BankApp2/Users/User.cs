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
                else if(t.Status == "Completed")
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

        void AccountMenu(Account account, Bank bank)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine($"Account number: {account.AccountNumber}");
                Console.WriteLine($"Account balance: {account.Balance}");
                Console.WriteLine("0. Exit");
                Console.WriteLine("1. Deposit money");
                Console.WriteLine("2. Withdraw money");
                Console.WriteLine("3. Transfer money");
                Console.WriteLine("4. Check maximum loan amount");
                Console.WriteLine("5. Take loan");
                Console.WriteLine("6. Calculate loan interest");
                if (account is SavingsAccount)
                {
                    Console.WriteLine("7. Calculate Interest");
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
                else if (response == "4")
                {
                    decimal maxLoan = account.GetMaxLoanAmount();
                    Console.WriteLine($"Current amount available to take loan for: {maxLoan.ToString("C", new CultureInfo("sv-SE"))}");
                    Console.ReadKey();
                }
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
                    if (!int.TryParse(Console.ReadLine(), out int months) || months <=0)
                    {
                        Console.WriteLine("Invalid number of months.");
                        Console.ReadKey();
                        continue;
                    }

                    decimal interest = account.CalculateLoanInterest(loanAmount, annualRate, months);

                    Console.WriteLine($"Interest to pay after {months} month(s): {interest.ToString("C", new CultureInfo("sv-SE"))}");
                    Console.ReadKey();
                }
                else if (response == "7" && account is SavingsAccount)
                {
                    var savingsAccount = account as SavingsAccount;
                    savingsAccount.ShowInterest();
                }
            }
        }
    }
}