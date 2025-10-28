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
        private const decimal OverdraftLimit = -5000m;

        public CheckingAccount(User user, string accountNumber, decimal balance) : base(user, accountNumber, balance) { }

        public override void Withdraw()
        {
            Console.Clear();
            try
            {
                Console.Write("Enter the amount to withdraw: ");

                if (!decimal.TryParse(Console.ReadLine(), out decimal amount))
                {
                    Console.WriteLine("Invalid input. Please enter a valid amount.");
                    Console.ReadKey();
                    return;
                }

                if (amount <= 0)
                {
                    Console.WriteLine("Withdrawal failed: amount must be greater than 0.");
                    Console.ReadKey();
                    return;
                }

                if (Balance - amount < OverdraftLimit)
                {
                    Console.WriteLine($"Withdrawal failed: overdraft limit of {OverdraftLimit:C} exceeded.");
                    Console.ReadKey();
                    return;
                }

                Balance -= amount;
                Owner.transactions.Add(new Transaction(accountNumber: AccountNumber, amount: amount, type: "withdrawal"));
                Console.WriteLine($"Withdrawal successful: new balance is {Balance:C}.");
                Console.ReadKey();
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Ett oväntat fel uppstod {ex.Message}");
                Console.ReadKey();
            }
        }
    }
}
