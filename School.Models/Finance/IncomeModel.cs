using System;
using System.ComponentModel.DataAnnotations;

namespace School.Models.Finance
{
    public class IncomeModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Income Account is required")]
        public int CoaAccountId { get; set; }

        [Required(ErrorMessage = "Amount is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than zero")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Date is required")]
        public DateTime Date { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Received From name is required")]
        [MaxLength(150, ErrorMessage = "Name cannot exceed 150 characters")]
        public string ReceivedFrom { get; set; } = null!;

        [MaxLength(100, ErrorMessage = "Reference number cannot exceed 100 characters")]
        public string? ReferenceNumber { get; set; }

        [MaxLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Status is required")]
        [MaxLength(30)]
        public string Status { get; set; } = "Cleared";
    }
}
