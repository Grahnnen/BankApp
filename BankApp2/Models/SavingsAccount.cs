using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankApp2.Users;

namespace BankApp2.Models
{
    public class SavingsAccount : Account
    {
        public decimal Balance { get; set; }
        public decimal InterestRate { get; set; }


        public int FreeWithdrawals { get; set; } = 3;
        public decimal WithdrawalFee { get; set; } = 10m;
        private int withdrawCount = 0;
        public SavingsAccount(User user, string accountNumber,decimal balance, decimal interestRate) : base(user, accountNumber, balance)
        {
            Balance = balance;
            InterestRate = interestRate;
        }

        public void ApplyInterest()
        {
            Balance += Balance * InterestRate;
        }

        public override void Withdraw(decimal amount)
        {
            Console.Clear();
            Console.Write("Enter the amount to withdraw: ");
            if (int.TryParse(Console.ReadLine(), out int amounts))
            {
                if (amount <= 0 || amount > Balance)
                    return;

                withdrawCount++;

                if (withdrawCount > FreeWithdrawals)
                {

                    Balance -= (amount + WithdrawalFee);
                }
                else
                {
                    Balance -= amount;
                }
            }
        }
    }
}




