using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace BankApp2
{
    public class Bank
    {
        public List<User> Users { get; set; } = new List<User>();
        public List<Account> Accounts { get; set; } = new List<Account>();

        public void OpenAccount(User user, string accountNumber)
        {
            //if (FindAccount(accountNumber) != null)
            //{
            //    Console.WriteLine("Konto med detta nummer finns redan");
            //}

            var account = new Account("1", 123);
            Accounts.Add(account);

            if (!Users.Contains(user))
            {
                Users.Add(user);
            }

        }

        public void FindAccount(string id)
        {
            foreach (var user in Users)
            {
                if (user.id == id)
                    Console.WriteLine("hittade user");

                else
                {
                    Console.WriteLine("hittade inte user");
                }
            }
        }



    }
}
