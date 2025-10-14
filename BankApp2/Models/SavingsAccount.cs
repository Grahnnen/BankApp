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

        public override void Withdraw()
        {
            Console.Clear();
            Console.Write("Enter the amount to withdraw: ");
            if (int.TryParse(Console.ReadLine(), out int amounts))
              
            {
                if (amounts <= 0 || amounts > Balance)
                    return;

                withdrawCount++;

                int remainingFree = FreeWithdrawals - withdrawCount;

                if (remainingFree > 0)
                {
                    Console.WriteLine($"You have {remainingFree} free withdrawal(s) left.");
                    
                }
                else if (remainingFree == 0)
                {
                    Console.WriteLine(" That was your last free withdrawal. Future withdrawals will incur a 10 kr fee.");
                }


                if (withdrawCount > FreeWithdrawals)
                {

                    Balance -= (amounts + WithdrawalFee);
                   
                }
                else
                {
                    Balance -= amounts;
                }

                Console.WriteLine($"Withdrawal successful. New balance: {Balance} kr");
                Owner.transactions.Add(new Transaction(AccountNumber, amounts, "Withdraw"));

            }
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();

        }
    }
}






