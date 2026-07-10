using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static School.Domain.BaseEntity;
using School.Domain.School;

namespace School.Domain.Library
{
    [Table("LibraryMembers")]
    public class LibraryMember : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string MembershipNumber { get; set; } = string.Empty;

        /// <summary>Student | Teacher | Employee | Principal | Admin | External</summary>
        [Required, MaxLength(30)]
        public string MemberType { get; set; } = "Student";

        [MaxLength(200)]
        public string MemberName { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? Email { get; set; }

        [MaxLength(20)]
        public string? Phone { get; set; }

        // FK to student or employee (nullable - one or the other)
        public int? StudentId { get; set; }
        [ForeignKey(nameof(StudentId))]
        public virtual Student.Student? Student { get; set; }

        [MaxLength(450)]
        public string? EmployeeUserId { get; set; }

        public DateTime JoiningDate { get; set; } = DateTime.UtcNow;
        public DateTime ExpiryDate { get; set; } = DateTime.UtcNow.AddYears(1);

        public int BorrowLimit { get; set; } = 3;
        public int CurrentBorrowCount { get; set; } = 0;

        [MaxLength(100)]
        public string? MembershipBarcode { get; set; }

        [MaxLength(200)]
        public string? MembershipQRCode { get; set; }

        [MaxLength(500)]
        public string? PhotoPath { get; set; }

        /// <summary>Active | Inactive | Suspended | Expired</summary>
        [MaxLength(20)]
        public string Status { get; set; } = "Active";

        [MaxLength(500)]
        public string? Remarks { get; set; }

        public int SchoolRegistrationId { get; set; }
        [ForeignKey(nameof(SchoolRegistrationId))]
        public virtual SchoolRegistration? SchoolRegistration { get; set; }
    }
}
