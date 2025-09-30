using System;

namespace BankApp2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // 1. Create an instance of LoginManager
            var loginManager = new LoginManager();

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
                Console.WriteLine($"Logged in as: {loginResult.LoggedInUser?.Role}");
            }
        }
    }
}