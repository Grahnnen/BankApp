using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApp2.Models
{
    public class Transaction
    {
        private string id;
        private string accountNumber;
        private decimal amount;
        private DateTime dateTime;
        private string type;
        private List<string> transactions = new List<string>();

        public void CreateTransaction(Account fromAccount, Account toAccount, decimal amount)
        {
            transactions.Add($"From account: {fromAccount.AccountNumber}" +
                $" to account: {toAccount.AccountNumber} - Amount: {amount}");
        }
        public void ListTransactions()
        {
            Console.Clear();
            foreach (var transaction in transactions)
            {
                Console.WriteLine(transaction);
            }
            Console.ReadKey();
        }
    }
}
