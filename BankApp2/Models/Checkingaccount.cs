using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankApp2.Users;

namespace BankApp2.Models
{
    public class CheckingAccount : Account
    {
        public CheckingAccount(User user, string accountNumber, decimal balance) : base(user, accountNumber, balance) { }
     
    }
}
