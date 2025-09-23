using System;

namespace BankApp2
{
    internal class Program
    {
        static Bank bank = new Bank();
        static Random rng = new Random();
        static void Main(string[] args)
        {
            //User user1 = new User("20001009", "Robin");
            //int random = rng.Next(1000, 9999);
            //bank.OpenAccount(user1, random.ToString());
			//random = rng.Next(1000, 9999);
			//bank.OpenAccount(user1, random.ToString());
			//random = rng.Next(1000, 9999);
			//bank.OpenAccount(user1, random.ToString());

            while (true)
            {
				Console.Clear();
                Console.WriteLine("Hello and welcome to UmeBank!");
                Console.WriteLine("1. Create user");
                Console.WriteLine("2. Login");
                Console.WriteLine("3. Exit");
                if (int.TryParse(Console.ReadLine(), out int response))
                {
                    if (response == 1)
                    {
                        
                        Console.Clear();
                        Console.Write("Personnummer: ");
                        long personNumber;
						if (long.TryParse(Console.ReadLine(), out personNumber))
						{
							bool userExists = false;
							foreach (var user in bank.Users)
							{
								if (user.id == personNumber.ToString())
								{
									userExists = true;
									break;
								}
							}

							if (!userExists)
							{
								Console.Write("Name: ");
								string username = Console.ReadLine();
								User newUser = new User(personNumber.ToString(), username);
								bank.Users.Add(newUser);
								LoggedIn(newUser);
								Console.Clear();
							}
							else
							{
								Console.Clear();
								Console.WriteLine("User with that id already exists! Please Login Instead.");
                                Console.ReadKey();
							}
						}
						else
                        {
                            Console.WriteLine("Invalid PersonNumber");
                            Console.ReadKey();
                            Console.Clear();
                        }
                    }
					else if (response == 2)
					{
						
						Console.Clear();
						Console.Write("Personnummer: ");
						string personNumber = Console.ReadLine();
						User foundUser = null;
						foreach (var user in bank.Users)
						{
							if (personNumber == user.id)
							{
								foundUser = user;
								break;
							}
						}
						if (foundUser != null)
						{
							LoggedIn(foundUser);
						}
						else
						{
							Console.WriteLine("User not found! Try again.");
							Console.ReadKey();
						}
					}
					else if (response == 3)
                    {
                        Environment.Exit(0);
                    }
                    else
                    {
                        Console.Clear();
                    }
                }
                else
                {
                    Console.Clear();
                }
            }
        }

        static void LoggedIn(User user)
        {
            while (true)
            {
				Console.Clear();
				Console.WriteLine($"Logged in as: {user.name}");
				Console.WriteLine($"You have: {user.account.Count} accounts open.");
				Console.WriteLine("1. Manage Accounts");
				Console.WriteLine("2. Open new account");
				Console.WriteLine("3. Signout");
                if(int.TryParse(Console.ReadLine(), out int response))
                {
					if (response == 1) //Manage Accounts
					{
						Console.Clear();
						if (user.account.Count <= 0) // No Accounts
						{
							Console.WriteLine("No accounts!");
							Console.ReadKey();
							continue;
						}

						Console.WriteLine($"Choose account to manage: ");

						for (int i = 0; i < user.account.Count; i++) //List all accounts
						{
							Console.WriteLine($"{i + 1}. Accountnumber: {user.account[i].AccountNumber} \n      Balance: {user.account[i].Balance}");
						}

						if (int.TryParse(Console.ReadLine(), out int choice) && choice >= 1 && choice <= user.account.Count)
						{
							Account selectedAccount = user.account[choice - 1];
							Console.Clear();
							ManageAccount(selectedAccount);
						}
					}
					else if (response == 2) //Open new account
					{
						int random = rng.Next(1000, 9999);
						bank.OpenAccount(user, random.ToString());
					}
					else if (response == 3) //Back
					{
						Console.Clear();
						break;
					}
					else
					{
						continue;
					}
                }
			}
        }
        static void ManageAccount(Account account)
        {
            while (true)
			{
				Console.Clear();
				Console.WriteLine($"Managing account: {account.AccountNumber}");
				Console.WriteLine("1. Deposit Money");
				Console.WriteLine("2. Withdraw Money");
				Console.WriteLine("3. Back");

				if (int.TryParse(Console.ReadLine(), out int response))
                {
					if (response == 1) //Despoit
					{
						Console.Clear();
						Console.Write($"Amount: ");
						if (int.TryParse(Console.ReadLine(), out int choice))
						{
							account.Deposit(choice);
							Console.ReadKey();
							break;
						}
					}
					else if (response == 2) //Withdraw
					{
						Console.Clear();
						Console.Write($"Amount: ");
						if (int.TryParse(Console.ReadLine(), out int choice))
						{
							account.Withdraw(choice);
							Console.ReadKey();
							break;
						}
					}
					else if (response == 3) //Back
					{
						break;
					}
					else //Loop
					{
						continue;
					}
				}
            }
		}
	}
}