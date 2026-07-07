using System.ComponentModel.DataAnnotations;

namespace School_DTOs.School
{
    public class SchoolSubscriptionDto : BaseDto
    {
        public int SchoolRegistrationId { get; set; }
        public int SubscriptionPlanId { get; set; }
        public string StartDate { get; set; } = string.Empty;
        public string EndDate { get; set; } = string.Empty;
        public decimal AmountPaid { get; set; }
        public string PaymentStatus { get; set; } = "Pending";
        public string? TransactionId { get; set; }
        public bool IsActive { get; set; }
    }

    public class SchoolSubscriptionModel
    {
        public int? Id { get; set; }

        [Required]
        public int SchoolRegistrationId { get; set; }

        [Required]
        public int SubscriptionPlanId { get; set; }

        [Required]
        public string StartDate { get; set; } = string.Empty;

        [Required]
        public string EndDate { get; set; } = string.Empty;

        [Required]
        public decimal AmountPaid { get; set; }

        [MaxLength(50)]
        public string PaymentStatus { get; set; } = "Pending";

        [MaxLength(100)]
        public string? TransactionId { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
