using System;

namespace BankApp2.Models
{
    public class Transaction
    {
        public string AccountNumber { get; }
        public decimal Amount { get; }
        public DateTime DateTime { get; }
        public string Type { get; }
        public string? TargetAccount { get; }
		public string Status { get; set; }
		public DateTime ScheduledCompletionTime { get; set; }

		public Transaction(string accountNumber, decimal amount, string type, string? targetAccount = null)
        {
            AccountNumber = accountNumber;
            Amount = amount;
            DateTime = DateTime.Now;
            Type = type;
            TargetAccount = targetAccount;
			Status = "Pending";
			ScheduledCompletionTime = DateTime.Now.AddSeconds(20);
		}

        // Returns a formatted string with transaction details
        public override string ToString()
        {
			if (!string.IsNullOrEmpty(TargetAccount))
				return $"{DateTime:G} | {Type} | {Amount:C} | To: {TargetAccount} | ({Status})";
			return $"{DateTime:G} | {Type} | {Amount:C}";
		}

        // Properties for handling recurring transactions
        public bool IsRecurring { get; set; }
        public int IntervalDays { get; set; }
        public DateTime? NextExecutionDate { get; set; }
    }
}