using System;

namespace BankApp2.Models
{
    public class Transaction
    {
        public string Id { get; }
        public string AccountNumber { get; }
        public decimal Amount { get; }
        public DateTime DateTime { get; }
        public string Type { get; }
        public string? TargetAccount { get; }
		public string Status { get; set; }
		public DateTime ScheduledCompletionTime { get; set; }

		public Transaction(string accountNumber, decimal amount, string type, string? targetAccount = null)
        {
            Id = Guid.NewGuid().ToString();
            AccountNumber = accountNumber;
            Amount = amount;
            DateTime = DateTime.Now;
            Type = type;
            TargetAccount = targetAccount;
			Status = "Pending";
			ScheduledCompletionTime = DateTime.Now.AddSeconds(20);
		}

        public override string ToString()
        {
			if (!string.IsNullOrEmpty(TargetAccount))
				return $"{DateTime:G} | {Type} | {Amount:C} | To: {TargetAccount}";
			return $"{DateTime:G} | {Type} | {Amount:C}";
		}
    }
}