using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static School.Domain.BaseEntity;
using School.Domain.School;
namespace School.Domain.Payroll
{
    public class PayrollRun : AuditEntity<int>, ITenantEntity
    {
        [Key] public int Id{get;set;}
        public int EmployeeId{get;set;}
        [ForeignKey(nameof(EmployeeId))] public virtual Hr.Employee Employee{get;set;}=null!;
        [MaxLength(20)] public string Month{get;set;}=null!;  // e.g. "2024-06"
        [Column(TypeName=""decimal(10,2)"")] 
        public decimal GrossSalary{get;set;}
        [Column(TypeName=""decimal(10,2)"")] 
        public decimal TotalDeductions{get;set;}
        [Column(TypeName=""decimal(10,2)"")] 
        public decimal NetSalary{get;set;}
        [MaxLength(20)] public string Status{get;set;}="Draft"; // Draft, Processed, Paid
        public int SchoolRegistrationId{get;set;}
        [ForeignKey(nameof(SchoolRegistrationId))] public virtual SchoolRegistration SchoolRegistration{get;set;}=null!;
    }
}
