using BankApp2.Models;
using BankApp2.Users;


namespace BankApp2.Menu
{
    public class BankMenu
    {

        public void ShowMenu()
        {
            // 1. Create an instance of LoginManager
            var loginManager = new LoginManager();
            Bank bank = new Bank();
            bank.Users.AddRange(loginManager.Users.Where(u => !bank.Users.Any(bu => bu.Username == u.Username)));

            while (true)
            {
                Console.WriteLine("=== Welcome to the UmeBank App ===");

                // 2. Ask user for input (for testing purposes)
                Console.Write("Enter username: ");
                string? username = Console.ReadLine();

                Console.Write("Enter password: ");
                string? password = Console.ReadLine();
                
                // 3. Call the login method
                var loginResult = loginManager.Login(username, password);
                
                // 4. Show the result
                Console.WriteLine(loginResult.Message);

                if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                {
                    Console.WriteLine("Please write your username and password.");
                }

                Console.WriteLine("\nPress any key to continue");
                Console.ReadKey();
                Console.Clear();

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
                        // The @ in front of the string is to make the print easier, preserving line breaks, ignoring backslashes as escape characters etc.
                        string title = @"
 __    __  .___  ___.  _______ .______        ___      .__   __.  __  ___ 
|  |  |  | |   \/   | |   ____||   _  \      /   \     |  \ |  | |  |/  / 
|  |  |  | |  \  /  | |  |__   |  |_)  |    /  ^  \    |   \|  | |  '  /  
|  |  |  | |  |\/|  | |   __|  |   _  <    /  /_\  \   |  . `  | |    <   
|  `--'  | |  |  |  | |  |____ |  |_)  |  /  _____  \  |  |\   | |  .  \  
 \______/  |__|  |__| |_______||______/  /__/     \__\ |__| \__| |__|\__\ 
";
                        Console.WriteLine(title);
                        Console.WriteLine($"Logged in as: {loginResult.LoggedInUser?.Username}");

                        Console.WriteLine($"You have {loginResult.LoggedInUser.Accounts.Count} accounts.");

                        Console.WriteLine("0. Sign out");
                        Console.WriteLine("1. Open new account");
                        Console.WriteLine("2. View accounts");
                        Console.WriteLine("3. View accounts with positive balance");
                        Console.WriteLine("4. Show account summary");
                        Console.WriteLine("5. Show top 3 transactions");
                        if (loginResult.LoggedInUser.Role == "Admin")
                        {
                            Console.WriteLine("6. Search users");
                            Console.WriteLine("7. Search account");
                            Console.WriteLine("8. Show all users");
                            Console.WriteLine("9. Show users with top transactions");
                            Console.WriteLine("10. Add user");
                            Console.WriteLine("11. Delete user");
                            Console.WriteLine("12. Set exchange rate");
                            Console.WriteLine("13. View all exchange rates");
                        }

                        string response = Console.ReadLine();
                        if (response == "0")
                        {
                            Console.Clear();
                            break;
                        }
                        else if (response == "1")
                        {
                            Random rng = new Random();
                            int random = rng.Next(999999, 9999999);
                            bank.OpenAccount(loginResult.LoggedInUser, random.ToString());
                        }
                        else if (response == "2")
                        {
                            loginResult.LoggedInUser.PrintAccounts(bank);
                        }
                        else if (response == "3")
                        {
                            loginResult.LoggedInUser.PrintPositiveAccounts();
                        }
                        else if (response == "4")
                        {
                            bank.PrintUserAccountSummaries();
                        }
                        else if (response == "5")
                        {
                            loginResult.LoggedInUser.PrintTopTransactions();
                        }
                        else if (response == "6" && loginResult.LoggedInUser.Role == "Admin")
                        {
                            Console.Write("Search for user:");
                            bank.FindUser(Console.ReadLine().ToLower());
                        }
                        else if (response == "7" && loginResult.LoggedInUser.Role == "Admin")
                        {
                            Console.Write("Search for account:");
                            bank.FindAccount(Console.ReadLine().ToLower());
                        }
                        else if (response == "8" && loginResult.LoggedInUser.Role == "Admin")
                        {
                            bank.ShowAllUsers();
                        }
                        else if (response == "9" && loginResult.LoggedInUser.Role == "Admin")
                        {
                            bank.FindUserWithTopTransactions();
                        }
                        else if (response == "10" && loginResult.LoggedInUser.Role == "Admin")
                        {
                            var newUser = loginManager.AddUser();
                            if( newUser != null) 
                            bank.Users.Add(newUser);
                        }
                        else if (response == "11" && loginResult.LoggedInUser.Role == "Admin")
                        {

                            var newUser = loginManager.DeleteUser();
                            if (newUser != null)
                            bank.Users.Remove(newUser);
                        }
                        else if (response == "12" && loginResult.LoggedInUser.Role == "Admin")
                        {
                            bank.CurrencyManager.SetExchangeRateInteractive();
                        }
                        else if (response == "13" && loginResult.LoggedInUser.Role == "Admin")
                        {
                            bank.CurrencyManager.ViewAllExchangeRates();
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
        }
    }
}
