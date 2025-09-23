namespace BankApp2
{
    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public decimal Balance { get; set; }
        public string Role { get; set; }
        public int FailedAttempts { get; set; } = 0;
        public bool IsLocked { get; set; } = false;

            public User(string username, string password, decimal balance, string role)
            {
                Username = username;
                Password = password;
                Balance = balance;
                Role = role;
        }
    }
}