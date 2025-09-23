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


        public class LoginResult
        {
            private User? loggedInUser;

            public bool Success { get; set; }
            public string? Message { get; set; }
            public User? LoggedInUser { get => loggedInUser; set => loggedInUser = value; }

        public string id;
        private string name;
        private List<Account> account = new List<Account>();

        public User(string id, string name)
        {
            this.id = id;
            this.name = name; 
        }
    }
}