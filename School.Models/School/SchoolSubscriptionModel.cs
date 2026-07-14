using System;
using System.ComponentModel.DataAnnotations;

namespace School.Models.School
{
    public class SchoolSubscriptionModel
    {
        public int? Id { get; set; }

        [Required]
        public int SchoolRegistrationId { get; set; }

        [Required]
        public int SubscriptionPlanId { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public decimal AmountPaid { get; set; }

        [MaxLength(50)]
        public string PaymentStatus { get; set; } = "Pending";

        [MaxLength(100)]
        public string? TransactionId { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
