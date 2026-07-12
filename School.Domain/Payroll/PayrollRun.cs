using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static School.Domain.BaseEntity;
using School.Domain.School;
namespace School.Domain.Payroll
{
    [Table("PayrollRuns", Schema = "Payroll")]
    public class PayrollRun : AuditEntity<int>, ITenantEntity
    {
        [Key] public int Id{get;set;}
        public int EmployeeId{get;set;}
        [ForeignKey(nameof(EmployeeId))] public virtual Hr.Employee Employee{get;set;}=null!;
        
        public int? PayGroupId { get; set; }
        [ForeignKey(nameof(PayGroupId))] public virtual PayGroup? PayGroup { get; set; }

        [Required, MaxLength(20)] public string Month{get;set;}=null!;  // e.g. "2024-06"
        [Column(TypeName="decimal(10,2)")] 
        public decimal GrossSalary{get;set;}
        [Column(TypeName="decimal(10,2)")] 
        public decimal TotalDeductions{get;set;}
        [Column(TypeName="decimal(10,2)")] 
        public decimal NetSalary{get;set;}
        [MaxLength(20)] public string Status{get;set;}="Draft"; // Draft, Processed, Approved, Locked, Paid

        [MaxLength(50)] public string? PaymentMethod { get; set; } // Cash, Cheque, BankTransfer, UPI
        [MaxLength(100)] public string? PaymentRef { get; set; }
        public DateTime? PaidDate { get; set; }
        
        public DateTime? LockedDate { get; set; }
        [MaxLength(450)] public string? LockedBy { get; set; }

        [MaxLength(450)] public string? ApprovedBy { get; set; }
        public DateTime? ApprovedDate { get; set; }

        public int SchoolRegistrationId{get;set;}
        [ForeignKey(nameof(SchoolRegistrationId))] public virtual SchoolRegistration SchoolRegistration{get;set;}=null!;
    }
}

