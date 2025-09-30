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
            //if (FindAccount(accountNumber) != null)
            //{
            //    Console.WriteLine("Konto med detta nummer finns redan");
            //}
            Console.WriteLine("Choose account type to open:");
            Console.WriteLine("1. Checking account");
            Console.WriteLine("2. Saving account");
            Console.WriteLine("3. Cancel");
            string response = Console.ReadLine();
            if(response == "1") 
            {
                var account = new (user, accountNumber, 0);
                user.account.Add(account);
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
