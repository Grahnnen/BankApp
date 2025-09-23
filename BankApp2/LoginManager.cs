using System;
using System.Collections.Generic;
using System.Linq;
using static BankApp2.User;

namespace BankApp2
{
    public class LoginManager
    {
        private List<User> users;

        public LoginManager()
        {
            users = new List<User>
            {
                new User("admin", "1234", 0, "Admin"),
                new User("john", "pass", 1000, "User"),
                new User("emma", "12345", 500, "User")
            };
        }

        public LoginResult Login(string username, string password)
        {
            var user = users.FirstOrDefault(u => u.Username == username);

            if (user == null)
                return new LoginResult { Success = false, Message = "User not found" };

            if (user.IsLocked)
                return new LoginResult { Success = false, Message = "Account is locked" };

            if (user.Password == password)
            {
                user.FailedAttempts = 0;
                return new LoginResult { Success = true, Message = $"Welcome {user.Username}", LoggedInUser = user };
            }
            else
            {
                user.FailedAttempts++;
                if (user.Role == "Admin" && user.FailedAttempts >= 3)
                    user.IsLocked = true;

                return new LoginResult
                {
                    Success = false,
                    Message = user.Role == "Admin" && user.IsLocked ? "Admin account locked!" : "Wrong password"
                };
            }
        }
    }
}
