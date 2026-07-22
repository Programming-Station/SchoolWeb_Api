using System;
using System.ComponentModel.DataAnnotations;

namespace School.Models.Finance
{
    public class ExpenseModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Expense Account is required")]
        public int CoaAccountId { get; set; }

        [Required(ErrorMessage = "Amount is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than zero")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Date is required")]
        public DateTime Date { get; set; } = DateTime.Now;

        public int? VendorId { get; set; }

        [MaxLength(100, ErrorMessage = "Reference number cannot exceed 100 characters")]
        public string? ReferenceNumber { get; set; }

        [MaxLength(500, ErrorMessage = "Receipt URL cannot exceed 500 characters")]
        public string? ReceiptUrl { get; set; }

        [MaxLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Status is required")]
        [MaxLength(30)]
        public string Status { get; set; } = "Paid";
    }
}
