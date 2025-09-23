namespace BankApp2
{
    public class LoginResult
    {
        private User? loggedInUser;

        public bool Success { get; set; }
        public string? Message { get; set; }
        public User? LoggedInUser { get => loggedInUser; set => loggedInUser = value; }
    }
}