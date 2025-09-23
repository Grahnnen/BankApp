namespace BankApp2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Bank bank = new Bank();
            User user1 = new User("001009", "Robin");
            bank.OpenAccount(user1, "2");

            while (true)
            {
                Console.WriteLine("Hello and welcome to UmeBank!");
                Console.WriteLine("1. Create user");
                Console.WriteLine("2. Login");
                Console.WriteLine("3. Exit");
                if (int.TryParse(Console.ReadLine(), out int response))
                {
                    if (response == 1)
                    {
                        while (true)
                        {
                            Console.Write("Personnummer: ");
                            string personNumber = Console.ReadLine();
                            foreach (var users in bank.Users)
                            {
                                if (users.id == personNumber)
                                {
                                    Console.Clear();

                                    Console.WriteLine("User with that id already exists! Please Login Instead.");
                                    continue;
                                }
                                else
                                {
                                    Console.Write("Namn: ");
                                    string username = Console.ReadLine();
                                    User user = new User(personNumber, username);
                                    Console.Clear();
                                    Console.WriteLine($"Now logged in as: {user.name}");

                                }
                            }
                            break;
                        }

                    }
                    else if (response == 2)
                    {
                        while (true)
                        {
                            Console.Clear();
                            Console.Write("Personnummer: ");
                            string personNumber = Console.ReadLine();
                            foreach (var users in bank.Users)
                            {
                                if (personNumber == users.id)
                                {
                                    Console.WriteLine($"Logged in as: {users.name}");
                                    break;
                                }
                                else
                                {
                                    Console.WriteLine("User not found! Try again.");
                                    continue;
                                }
                            }
                            break;
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
    }
}