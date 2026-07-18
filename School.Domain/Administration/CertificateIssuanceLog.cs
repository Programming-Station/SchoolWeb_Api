using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static School.Domain.BaseEntity;

namespace School.Domain.Administration
{
    /// <summary>
    /// Logs every certificate issued (TC, Bonafide, Character, Migration, etc.)
    /// for audit trail and re-print tracking.
    /// </summary>
    [Table("CertificateIssuanceLogs", Schema = "Administration")]
    public class CertificateIssuanceLog : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string CertificateNumber { get; set; } = null!; // e.g. TC-2026-0001

        [Required, MaxLength(50)]
        public string CertificateType { get; set; } = null!; // TC, Bonafide, Character, Migration, StudyCertificate, Custom

        public int StudentId { get; set; }

        public int AcademicYearId { get; set; }

        [MaxLength(200)]
        public string? StudentName { get; set; }

        [MaxLength(50)]
        public string? AdmissionNumber { get; set; }

        [MaxLength(100)]
        public string? ClassName { get; set; }

        public DateTime IssuedDate { get; set; }

        [MaxLength(450)]
        public string? IssuedByUserId { get; set; }

        [MaxLength(200)]
        public string? IssuedByName { get; set; }

        [MaxLength(500)]
        public string? Reason { get; set; }

        [MaxLength(500)]
        public string? PdfUrl { get; set; } // Stored PDF path

        [MaxLength(100)]
        public string? QrVerificationCode { get; set; } // For QR-based verification

        public int PrintCount { get; set; } = 1;

        public DateTime? LastPrintedDate { get; set; }

        [Required, MaxLength(20)]
        public string Status { get; set; } = "Issued"; // Draft, Issued, Revoked, Cancelled

        [MaxLength(500)]
        public string? RevocationReason { get; set; }

        public int SchoolRegistrationId { get; set; }
    }
}
