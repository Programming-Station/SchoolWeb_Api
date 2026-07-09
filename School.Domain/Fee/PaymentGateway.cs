using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using School.Domain.School;
using static School.Domain.BaseEntity;

namespace School.Domain.FeeManagnment
{
    #nullable disable

    /// <summary>6.2 — Payment Gateway Configuration</summary>
    public class PaymentGateway : AuditEntity<int>, ITenantEntity
    {
        [Key] public int Id { get; set; }

        /// <summary>Razorpay / Stripe / Paypal</summary>
        [Required, MaxLength(50)] public string GatewayName { get; set; }

        [Required, MaxLength(255)] public string ApiKey { get; set; }

        [Required, MaxLength(255)] public string SecretKey { get; set; }

        [MaxLength(255)] public string WebhookSecret { get; set; }

        public bool IsActive { get; set; } = true;

        [Required] public int SchoolRegistrationId { get; set; }
        [ForeignKey(nameof(SchoolRegistrationId))] public virtual SchoolRegistration SchoolRegistration { get; set; }
    }

    /// <summary>6.2 — Online Payment Order/Transaction Tracking</summary>
    public class OnlinePaymentOrder : AuditEntity<int>, ITenantEntity
    {
        [Key] public int Id { get; set; }

        [Required, MaxLength(100)] public string OrderId { get; set; } // Gateway Order ID or Session ID

        [Required] public int FeeInstallmentId { get; set; }
        [ForeignKey(nameof(FeeInstallmentId))] public virtual FeeInstallment FeeInstallment { get; set; }

        [Required] public int StudentId { get; set; }
        [ForeignKey(nameof(StudentId))] public virtual Student.Student Student { get; set; }

        [Required, Column(TypeName = "decimal(18,2)")] public decimal Amount { get; set; }

        [Required, MaxLength(10)] public string Currency { get; set; } = "INR";

        /// <summary>Initiated / Completed / Failed</summary>
        [Required, MaxLength(20)] public string Status { get; set; } = "Initiated";

        [MaxLength(100)] public string PaymentId { get; set; } // Gateway Payment ID

        [MaxLength(100)] public string Signature { get; set; } // Gateway Webhook / Payment Signature

        [MaxLength(500)] public string Remarks { get; set; }

        [Required] public int SchoolRegistrationId { get; set; }
        [ForeignKey(nameof(SchoolRegistrationId))] public virtual SchoolRegistration SchoolRegistration { get; set; }
    }
}
