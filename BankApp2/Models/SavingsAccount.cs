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
        public SavingsAccount(User user, string accountNumber, decimal balance, decimal interestRate) : base(user, accountNumber, balance)
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
            while (true)
            {
                try
                {
                    if (int.TryParse(Console.ReadLine(), out int amounts))

                    {
                        if (amounts <= 0 || amounts > Balance)
                        {

                            Console.WriteLine($"Withdrawal failed: ({Balance}) sek on your account");
                            Console.ReadKey();
                            return;
                        }


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
						Console.ReadKey();
						break;
                    }
                    else
                    {
                        Console.WriteLine("\ninvalid input, enter a interger value!");
                        Console.ReadKey();
                    }
                }

                catch (Exception ex)
                {
                    Console.WriteLine($"An error has occured: {ex.Message}");
                    Console.ReadKey();
                }
            }
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

            if (withdrawCount > FreeWithdrawals)
            {
                Console.WriteLine($"Fee of {WithdrawalFee} kr was applied.");
            }

        }

        public void ShowInterest()
        {
            Console.Clear();
            Console.WriteLine($"Account: {AccountNumber}");
            Console.WriteLine($"Current Balance: {Balance:F2} kr"); // F2 Gives us two decimals
            Console.WriteLine($"Interest Rate: {InterestRate:P2} per month"); //P2 gives us percentage with two decimals

            Console.Write("\nEnter number of months to calculate interest: ");
            if (!int.TryParse(Console.ReadLine(), out int months) || months <= 0)
            {
                Console.WriteLine("Invalid input. Please enter a positive number of months.");
            }
            else
            {
                decimal futureBalance = Balance * (decimal)Math.Pow((double)(1 + InterestRate), months); // Calculate future balance using compound interest: Balance × (1 + rate)^months
                decimal totalInterest = futureBalance - Balance;

                Console.WriteLine($"\nInterest earned after {months} month(s): {totalInterest:F2} kr");
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
    }
}