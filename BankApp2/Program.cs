using BankApp2.Users;
using System;

namespace BankApp2.Models
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // 1. Create an instance of LoginManager
            var loginManager = new LoginManager();
            Bank bank = new Bank();

            Console.WriteLine("=== Welcome to the Bank App Login Test ===");

            // 2. Ask user for input (for testing purposes)
            Console.Write("Enter username: ");
            string? username = Console.ReadLine();

            Console.Write("Enter password: ");
            string? password = Console.ReadLine();

            if (username == null || password == null)
            {
                Console.WriteLine("Username and password cannot be null.");
                return;
            }

            // 3. Call the login method
            var loginResult = loginManager.Login(username, password);

            // 4. Show the result
            Console.WriteLine(loginResult.Message);

            // 5. Optional: show role if login succeeded
            if (loginResult.Success)
            {
                while (true)
                {
                    Console.Clear();
                    Console.WriteLine($"Logged in as: {loginResult.LoggedInUser?.Username}");

                    Console.WriteLine($"You have {loginResult.LoggedInUser.Account.Count} accounts.");

                    Console.WriteLine("0. Exit");
                    Console.WriteLine("1. Open new account");
                    Console.WriteLine("2. View accounts");
                    Console.WriteLine("3. View accounts with positive balance");
                    Console.WriteLine("4. Show top 3 transactions");

                    string response = Console.ReadLine();
                    if (response == "0")
                    {
                        Environment.Exit(0);
                    }
                    else if (response == "1")
                    {
                        Random rng = new Random();
                        int random = rng.Next(999999, 9999999);
                        bank.OpenAccount(loginResult.LoggedInUser, random.ToString());
                    }
                    else if (response == "2")
                    {
                        loginResult.LoggedInUser.PrintAccounts();
                    }
                    else if (response == "3")
                    {
                        loginResult.LoggedInUser.PrintPositiveAccounts();
                    }
                    else if (response == "4")
                    {
                        ShowTopTransactionsMenu(loginResult.LoggedInUser);
                    }
                    else
                    {
                        Console.WriteLine("Ogiltig inmatning, Vänligen välj 1, 2, 3, 4 eller 5");
                        Console.WriteLine("Tryck på valfri tangent för att fortsätta...");
                        Console.ReadKey();
                    }
                }
            }
        }
        static void ShowTopTransactionsMenu(User currentUser)
        {
            for (int i = 0; i < currentUser.Account.Count; i++)
            {
                var acc = currentUser.Account[i];
                Console.WriteLine($"{i + 1}. {acc.AccountNumber} - Balance: {acc.Balance:C}");
            }
            Console.WriteLine("\nSelect account number: ");
            if (int.TryParse(Console.ReadLine(), out int choice)
                && choice > 0 && choice <= currentUser.Account.Count)
            {
                var selected = currentUser.Account[choice - 1];

                var topTransactions = selected.GetTopTransactions(3);

                if (topTransactions.Count == 0)
                {
                    Console.WriteLine("No transactions yet.");
                }
                else
                {
                    Console.WriteLine("\nTop 3 transactions:");
                    foreach (var t in topTransactions)
                    {
                        Console.WriteLine(t);
                    }
                }


                Console.WriteLine("\nPress any key to return...");
                Console.ReadKey();
            }
        }
    }
}