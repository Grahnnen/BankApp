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
                // Add the logged-in user to the bank's Users list if not already present
                if (!bank.Users.Any(u => u.Username == loginResult.LoggedInUser.Username))
                {
                    bank.Users.Add(loginResult.LoggedInUser);
                }

                while (true)
                {
                    Console.Clear();
                    Console.WriteLine($"Logged in as: {loginResult.LoggedInUser?.Username}");

                    Console.WriteLine($"You have {loginResult.LoggedInUser.Account.Count} accounts.");

                    Console.WriteLine("0. Exit");
                    Console.WriteLine("1. Open new account");
                    Console.WriteLine("2. View accounts");
                    Console.WriteLine("3. View accounts with positive balance");
                    Console.WriteLine("4. View user account summaries");

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
                        bank.PrintUserAccountSummaries();
                    }
                }
            }
        }
    }
}