using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using School.Domain.School;
using static School.Domain.BaseEntity;

namespace School.Domain.Student
{
    public class Student : AuditEntity<int>
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string StudentId { get; set; } = string.Empty;

        [MaxLength(50)]
        public string? EnrollmentNumber { get; set; }

        [Required, MaxLength(20)]
        public string CourseType { get; set; } = null!;

        [Required]
        public int CourseId { get; set; }

        [MaxLength(200)]
        public string? CourseOpted { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(200)]
        public string FathersName { get; set; } = string.Empty;

        [Required]
        [MaxLength(10)]
        public string Gender { get; set; } = string.Empty; // Male, Female

        [MaxLength(100)]
        public string? Nationality { get; set; }

        [MaxLength(100)]
        public string? Occupation { get; set; }
        public string? DateOfBirth { get; set; }

        [MaxLength(200)]
        public string? SchoolCollege { get; set; }

        [MaxLength(1000)]
        public string? QualificationDetails { get; set; }

        [MaxLength(500)]
        public string? PostalAddress { get; set; }
        public int? StateId { get; set; }
        public int? CityId { get; set; }

        [MaxLength(10)]
        public string? PinCode { get; set; }

        [MaxLength(15)]
        public string? MobileNo1 { get; set; }

        [MaxLength(15)]
        public string? MobileNo2 { get; set; }

        public string? Image { get; set; } // Base64 or file path 
        public int? ClassId { get; set; }
        public int StatusId { get; set; } // active, inactive, graduated, transferred

        [MaxLength(1000)]
        public string? Remarks { get; set; }

        [ForeignKey(nameof(StatusId))]
        public virtual Status Status { get; set; }
        [ForeignKey(nameof(CourseId))]
        public virtual Course Course { get; set; }
        [ForeignKey(nameof(ClassId))]
        public virtual Class? Class { get; set; }

        public int SchoolRegistrationId { get; set; }
        [ForeignKey(nameof(SchoolRegistrationId))]
        public virtual SchoolRegistration SchoolRegistration { get; set; } = null!;
    }
}

