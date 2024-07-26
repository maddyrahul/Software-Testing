using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Data_Access_Layer.Models
{
    public class Expense
    {
        public int ExpenseId { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        [StringLength(100, ErrorMessage = "Description cannot exceed 100 characters.")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Amount is required.")]
        [Range(0.01, 1000000, ErrorMessage = "Amount must be between 0.01 and 1,000,000.")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Date is required.")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        public int PaidById { get; set; }

        [JsonIgnore]
        public User? PaidBy { get; set; }

        public int GroupId { get; set; }

        [JsonIgnore]
        public Group? Group { get; set; }
    }
}
