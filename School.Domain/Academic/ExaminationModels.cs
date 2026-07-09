using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using School.Domain.School;
using static School.Domain.BaseEntity;

namespace School.Domain.Academic
{
    #nullable disable

    /// <summary>5.1 — Exam Schedule: which subject on which date/time</summary>
    public class ExamSchedule : AuditEntity<int>, ITenantEntity
    {
        [Key] public int Id { get; set; }

        [Required] public int ExamId { get; set; }
        [ForeignKey(nameof(ExamId))] public virtual Exam Exam { get; set; }

        [Required] public int SubjectId { get; set; }
        [ForeignKey(nameof(SubjectId))] public virtual Subject Subject { get; set; }

        public int? ClassId { get; set; }
        [ForeignKey(nameof(ClassId))] public virtual Class Class { get; set; }

        [Required] public DateTime ExamDate { get; set; }
        [Required, MaxLength(10)] public string StartTime { get; set; } = "09:00";
        [Required, MaxLength(10)] public string EndTime { get; set; } = "12:00";

        [Column(TypeName = "decimal(5,2)")] public decimal MaxMarks { get; set; } = 100;
        [Column(TypeName = "decimal(5,2)")] public decimal PassingMarks { get; set; } = 33;

        [MaxLength(100)] public string RoomNo { get; set; }
        [MaxLength(500)] public string Instructions { get; set; }

        [Required] public int SchoolRegistrationId { get; set; }
        [ForeignKey(nameof(SchoolRegistrationId))] public virtual SchoolRegistration SchoolRegistration { get; set; }
    }

    /// <summary>5.3 — Grade Configuration per school (A+ = 90-100, etc.)</summary>
    public class GradeConfig : AuditEntity<int>, ITenantEntity
    {
        [Key] public int Id { get; set; }
        [Required, MaxLength(10)] public string GradeName { get; set; }  // A+, A, B+...
        [Column(TypeName = "decimal(5,2)")] public decimal MinPercent { get; set; }
        [Column(TypeName = "decimal(5,2)")] public decimal MaxPercent { get; set; }
        [Column(TypeName = "decimal(5,2)")] public decimal GradePoint { get; set; } // 4.0, 3.7...
        [MaxLength(20)] public string Remark { get; set; } // Excellent, Good...
        public bool IsPass { get; set; } = true;
        public int DisplayOrder { get; set; }
        [Required] public int SchoolRegistrationId { get; set; }
        [ForeignKey(nameof(SchoolRegistrationId))] public virtual SchoolRegistration SchoolRegistration { get; set; }
    }

    /// <summary>5.4 — Report Card header per student per exam</summary>
    public class ReportCard : AuditEntity<int>, ITenantEntity
    {
        [Key] public int Id { get; set; }

        [Required] public int StudentId { get; set; }
        [ForeignKey(nameof(StudentId))] public virtual Student.Student Student { get; set; }

        [Required] public int ExamId { get; set; }
        [ForeignKey(nameof(ExamId))] public virtual Exam Exam { get; set; }

        public int? ClassId { get; set; }
        [ForeignKey(nameof(ClassId))] public virtual Class Class { get; set; }

        [Column(TypeName = "decimal(5,2)")] public decimal TotalMarksObtained { get; set; }
        [Column(TypeName = "decimal(5,2)")] public decimal TotalMaxMarks { get; set; }
        [Column(TypeName = "decimal(5,2)")] public decimal Percentage { get; set; }
        [MaxLength(10)] public string Grade { get; set; }
        [MaxLength(10)] public string GradePoint { get; set; }
        public int Rank { get; set; }

        /// <summary>Pass / Fail / Compartment / Absent</summary>
        [MaxLength(20)] public string Status { get; set; } = "Pass";

        /// <summary>Draft / Published</summary>
        [MaxLength(20)] public string PublishStatus { get; set; } = "Draft";
        public DateTime? PublishedDate { get; set; }

        [MaxLength(500)] public string Remarks { get; set; }

        /// <summary>Path to generated PDF in wwwroot/uploads/reportcards/</summary>
        [MaxLength(500)] public string PdfPath { get; set; }

        [Required] public int SchoolRegistrationId { get; set; }
        [ForeignKey(nameof(SchoolRegistrationId))] public virtual SchoolRegistration SchoolRegistration { get; set; }
    }

    /// <summary>5.6 — Student Promotion record</summary>
    public class StudentPromotion : AuditEntity<int>, ITenantEntity
    {
        [Key] public int Id { get; set; }

        [Required] public int StudentId { get; set; }
        [ForeignKey(nameof(StudentId))] public virtual Student.Student Student { get; set; }

        public int? FromClassId { get; set; }
        [ForeignKey(nameof(FromClassId))] public virtual Class FromClass { get; set; }

        public int? ToClassId { get; set; }
        [ForeignKey(nameof(ToClassId))] public virtual Class ToClass { get; set; }

        public int? FromAcademicYearId { get; set; }
        [ForeignKey(nameof(FromAcademicYearId))] public virtual AcademicYear FromAcademicYear { get; set; }

        public int? ToAcademicYearId { get; set; }
        [ForeignKey(nameof(ToAcademicYearId))] public virtual AcademicYear ToAcademicYear { get; set; }

        public int? ExamId { get; set; }
        [ForeignKey(nameof(ExamId))] public virtual Exam Exam { get; set; }

        /// <summary>Promoted / Detained / Transferred / Graduated / DroppedOut</summary>
        [Required, MaxLength(30)] public string Status { get; set; } = "Promoted";

        [MaxLength(500)] public string Remarks { get; set; }
        public DateTime PromotionDate { get; set; } = DateTime.Today;

        [Required] public int SchoolRegistrationId { get; set; }
        [ForeignKey(nameof(SchoolRegistrationId))] public virtual SchoolRegistration SchoolRegistration { get; set; }
    }
}
