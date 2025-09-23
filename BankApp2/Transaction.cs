using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApp2
{
    internal class Transaction
    {
        private string id;
        private string accountNumber;
        private decimal amount;
        private DateTime dateTime;
        private string type;

        public Transaction(string id, string accountNumber, decimal amount, DateTime dateTime, string type)
        {
            this.id = id;
            this.accountNumber = accountNumber;
            this.amount = amount;
            this.dateTime = dateTime;
            this.type = type;
        }
    }
}
