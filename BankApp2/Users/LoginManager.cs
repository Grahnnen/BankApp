using System;
using System.Collections.Generic;
using System.Linq;

namespace BankApp2.Users
{
    public class LoginManager
    {
        private List<User> users;
        public List<User> Users => users;
        public LoginManager()
        {
            users = new List<User>
            {
                new User("admin", "1234", 0, "Admin"),
                new User("john", "pass", 1000, "User"),
                new User("emma", "12345", 500, "User"),
                new User("emelie", "12345", 500, "User"),
                new User("martin", "12345", 500, "User"),
                new User("robin", "12345", 500, "Admin"),
            };
        }

        public LoginResult Login(string username, string password)
        {
            var user = users.FirstOrDefault(u => u.Username == username);

            if (user == null)
                return new LoginResult { Success = false, Message = "\nUser not found" };

            if (user.IsLocked)
                return new LoginResult { Success = false, Message = "\nAccount is locked" };

            if (user.Password == password)
            {
                user.FailedAttempts = 0;
                return new LoginResult { Success = true, Message = $"\nWelcome {user.Username}", LoggedInUser = user };
            }
            else
            {
                user.FailedAttempts++;
                if (user.FailedAttempts >= 3)
                    user.IsLocked = true;

                return new LoginResult
                {
                    Success = false,
                    Message = user.IsLocked ? "\nAccount locked! (Wrong password 3 times)" : "\nWrong password"
                };
            }
        }

        public User AddUser()
        {
            Console.Clear();
            Console.WriteLine("Adding user");
            Console.Write("Name: ");
            string name = Console.ReadLine();
            Console.Write("Password: ");
            string password = Console.ReadLine();
            Console.Write("Role(Admin/User): ");
            string newRole = Console.ReadLine();
            if(string.IsNullOrEmpty(name) || string.IsNullOrEmpty(password))
            {
                Console.WriteLine("Name and password cannot be empty!");
                Console.ReadKey();
                return null;
            }
            if (newRole == "Admin" || newRole == "User")
            {
                User newUser = new User(name, password, 0, newRole);
                Users.Add(newUser);
                Console.WriteLine("Added user");
                Console.ReadKey();
                return newUser;
            }
            else
            {
                Console.WriteLine("Invalid role!");
                Console.ReadKey();
                return null;
            }
        }
        public User DeleteUser()
        {
            Console.Clear();
            Console.WriteLine("Deleting user");
            Console.Write("Name: ");
            string name = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("Name cannot be empty.");
                Console.ReadKey();
                return null;
            }

            var userToDelete = users.FirstOrDefault(u => string.Equals(u.Username, name, StringComparison.OrdinalIgnoreCase));

            if (userToDelete == null)
            {
                Console.WriteLine("User not found.");
                Console.ReadKey();
                return null;
            }
            users.Remove(userToDelete);
            Console.WriteLine("User deleted.");
            Console.ReadKey();
            return userToDelete;
        }
        public void TryChangePAssword(LoginResult loginResult)
        {
            if (loginResult.LoggedInUser.IsPasswordChangeDue())
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\n⚠️ Its time to change your password!");
                Console.ForegroundColor = ConsoleColor.White;

                Console.Write("Would you like to change it now? (y/n):");
                string answer = Console.ReadLine();

                if (answer == "y")
                {
                    Console.Write("Enter new password: ");
                    string newPassword = Console.ReadLine();
                    loginResult.LoggedInUser.Password = newPassword;
                    loginResult.LoggedInUser.NextPasswordChangeDate = DateTime.Now.AddDays(90); // we set the time for when the password need to be changed next time
                    Console.WriteLine("✅ Password successfully changed!");
                }
            }
        }
    }

}
