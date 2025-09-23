
using System.Transactions;

namespace BankApp2
{
    public class Account
    {
        public string AccountNumber { get; set; }
        public decimal Balance { get; set; }


        List<Transaction> transactions = new List<Transaction>();

        public Account (string accountNumber, decimal Balance)
        {
            AccountNumber = accountNumber;
            Balance = Balance;
        }


    }
}
