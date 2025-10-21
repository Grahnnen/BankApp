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
        public List<Account> Accounts = new List<Account>();
        public List<Transaction> transactions = new List<Transaction>();


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
            Accounts.Add(new SavingsAccount(this, random.ToString(), 0, 3));
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

            }
        }

    }
}