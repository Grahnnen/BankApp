using System;

namespace BankApp2
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
                }
            }
        }
    }
}