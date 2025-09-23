namespace BankApp2
{
    internal class User
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public decimal Balance { get; set; }
        public string Role { get; set; }
        public int FailedAttempts { get; set; } = 0;
        public bool IsLocked { get; set; } = false;


        public class LoginResult
        {
            private User? loggedInUser;

            public bool Success { get; set; }
            public string? Message { get; set; }
            public User? LoggedInUser { get => loggedInUser; set => loggedInUser = value; }
        }
    }
}