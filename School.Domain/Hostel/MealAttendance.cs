using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static School.Domain.BaseEntity;
using School.Domain.School;

namespace School.Domain.Hostel
{
    [Table("MealAttendances", Schema = "Hostel")]
    public class MealAttendance : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        public int StudentId { get; set; }

        [Required, MaxLength(50)]
        public string MealType { get; set; } = null!; // Breakfast, Lunch, Snacks, Dinner

        public DateTime Date { get; set; }

        [Required, MaxLength(50)]
        public string ScannedVia { get; set; } = null!; // QR, Barcode, RFID, Manual

        [MaxLength(100)]
        public string? TokenNumber { get; set; }

        public int SchoolRegistrationId { get; set; }

        [ForeignKey(nameof(StudentId))]
        public virtual global::School.Domain.Student.Student Student { get; set; } = null!;

        [ForeignKey(nameof(SchoolRegistrationId))]
        public virtual SchoolRegistration SchoolRegistration { get; set; } = null!;
    }
}
