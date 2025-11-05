using BankApp2.Models;
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
            //List of all users and password in the bank
            users = new List<User>
            {
                new User("admin", "1234", "Admin"),
                new User("john", "pass", "User"),
                new User("emma", "12345", "User"),
                new User("emelie", "12345", "User"),
                new User("martin", "12345", "User"),
                new User("robin", "12345", "Admin"),
            };
        }

        //Handles user login
        public LoginResult Login(string username, string password)
        {
            var user = users.FirstOrDefault(u => u.Username == username);

            if (user == null)
                return new LoginResult { Success = false, Message = "\nUser not found" };

            if (user.IsSuspended)
                return new LoginResult { Success = false, Message = "\nYour account is temporarily suspended. Contact the administrator" };


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
        //Adding a new user
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
                User newUser = new User(name, password, newRole);
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
        //Admin delete existing user
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

        //Change password
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
                    loginResult.LoggedInUser.NextPasswordChangeDate = DateTime.Now.AddDays(90); // password must be changed every 90 days
                    Console.WriteLine("✅ Password successfully changed!");
                }
            }
        }

        //Admin suspends user
        public void SuspendUserInteractive(User admin)
        {
            if(admin.Role != "Admin")
            {
                throw new Exception("User needs to be an admin to suspend other users.");
            }
            var allNonAdminUsers = Users.Where(u => u.Role != "Admin").ToList();
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== Suspend/Unsuspend User ===");

                // Showing all the users in the bank
                for (int i = 0; i < allNonAdminUsers.Count; i++)
                {
                    var u = allNonAdminUsers[i];
                    string status = u.IsSuspended ? "⛔ Suspended" : "✅ Active";
                    Console.WriteLine($"{i + 1}. {u.Username} ({status})");
                }

                Console.WriteLine("0. Cancel");
                Console.Write("\nPick a user to toggle the status: ");

                // reading the choice
                if (int.TryParse(Console.ReadLine(), out int choice))
                {
                    if (choice == 0)
                    {
                        Console.WriteLine("Aborting...");
                        break;
                    }
                    else if (choice > 0 && choice <= allNonAdminUsers.Count)
                    {
                        var selectedUser = allNonAdminUsers[choice - 1];
                        selectedUser.ToggleSuspension();
                    }
                    else
                    {
                        Console.WriteLine("Invalid choice!");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid format, a number is expected!");
                }
            }
        }
    }

}
