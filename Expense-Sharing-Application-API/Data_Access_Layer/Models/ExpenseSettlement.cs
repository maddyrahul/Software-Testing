using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Access_Layer.Models
{
    public class ExpenseSettlement
    {
        public int ExpenseSettlementId { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }

        public int ExpenseId { get; set; }
        public Expense? Expense { get; set; }

        public int PaidById { get; set; }
        public User? PaidBy { get; set; }

        public int ReceivedById { get; set; }
        public User? ReceivedBy { get; set; }
    }
}
