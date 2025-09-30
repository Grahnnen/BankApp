using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApp2
{
    public class SavingsAccount : Account
    {
        public decimal Balance { get;  set; }
        public decimal InterestRate { get; set; }


        public SavingsAccount(User user, string accountNumber,decimal balance, decimal interestRate) : base(user, accountNumber, balance)
        {
            Balance = balance;
            InterestRate = interestRate;
        }

        public void ApplyInterest()
        {
            Balance += Balance * InterestRate;
        }

       
       
    }
}
