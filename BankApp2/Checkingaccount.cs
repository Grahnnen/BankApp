using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApp2
{
    public class CheckingAccount : Account
    {
        public CheckingAccount(string accountNumber, decimal balance) : base(accountNumber, balance) { }

        public override void Withdraw(decimal amount) { }
     
    }
}
