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
            if (decimal.TryParse(Console.ReadLine(), out decimal amounts))
              
            {
                if (amounts <= 0)
                {
                    Console.WriteLine("Withdrawal failed: amount must be greater than 0.");
                    Console.WriteLine("\nPress any key to continue...");
                    Console.ReadKey();
                    return;
                }

                decimal totalAmount = amounts;

                if (withdrawCount >= FreeWithdrawals)
                {
                    totalAmount += WithdrawalFee;
                    Console.WriteLine($"A fee of {WithdrawalFee} kr will be applied to this withdrawal.");
                }
                if (totalAmount > Balance)
                {
                    Console.WriteLine("Withdrawal failed: insufficient funds.");
                    Console.WriteLine("\nPress any key to continue...");
                    Console.ReadKey();
                    return;
                }

                withdrawCount++;
                Balance -= totalAmount;

                int remainingFree = FreeWithdrawals - withdrawCount;

                if (remainingFree > 0)
                {
                    Console.WriteLine($"Withdrawal successful. You have {remainingFree} free withdrawal(s) left.");
                }
                else if (remainingFree == 0)
                {
                    Console.WriteLine("Withdrawal successful. That was your last free withdrawal.");
                    Console.WriteLine("Future withdrawals will have a 10 kr fee.");
                }
                else
                {
                    Console.WriteLine($"Withdrawal successful. Fee of {WithdrawalFee} kr was applied.");
                }

                Console.WriteLine($"New balance: {Balance} kr");
                Owner.transactions.Add(new Transaction(AccountNumber, amounts, "Withdraw"));
            }
            else
            {
                Console.WriteLine("Invalid amount.");
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();

        }

        public override void Withdraw(decimal amount)
        {
            if (amount <= 0)
            {
                Console.WriteLine("Withdrawal failed: amount must be greater than 0.");
                return;
            }

            decimal totalAmount = amount;


            if (withdrawCount >= FreeWithdrawals)
            {
                totalAmount += WithdrawalFee;
            }

            if (totalAmount > Balance)
            {
                Console.WriteLine("Withdrawal failed: insufficient funds (including fee).");
                return;
            }

            withdrawCount++;
            Balance -= totalAmount;
            Owner.transactions.Add(new Transaction(AccountNumber, amount, "Withdraw"));

            if (withdrawCount > FreeWithdrawals)
            {
                Console.WriteLine($"Fee of {WithdrawalFee} kr was applied.");
            }
        }
    }
}






