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

        public void OpenAccount(User user, string accountNumber)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Choose account type to open:");
                Console.WriteLine("0. Cancel");
                Console.WriteLine("1. Checking account");
                Console.WriteLine("2. Saving account");
                string response = Console.ReadLine();
                if (response == "0")
                {
                    break;
                }
                else if (response == "1")
                {
                    var account = new CheckingAccount(user, accountNumber, 0);
                    user.Account.Add(account);
                    break;
                }
                else if (response == "2")
                {
                    var account = new SavingsAccount(user, accountNumber, 0, 3);
                    user.Account.Add(account);
                    break;
                }
                
            }
        }
       

       //public void FindAccount(string id)
       //{
       //    foreach (var user in Users)
       //    {
       //        if (user.id == id)
       //            Console.WriteLine("hittade user");
       //
       //        else
       //        {
       //            Console.WriteLine("hittade inte user");
       //        }
       //    }
       //}
    }
}
