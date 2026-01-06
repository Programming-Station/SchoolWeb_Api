using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static School.Domain.BaseEntity;
using School.Domain.Auth;

namespace School.Domain
{
    public class Teacher : AuditEntity<int>
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(20)]
        public string? TeacherId { get; set; }

        // Relationship with ApplicationUser (for login)
        [Required, MaxLength(450)]
        public string UserId { get; set; } = null!;

        [MaxLength(15)]
        public string? AlternatePhoneNumber { get; set; }

        [MaxLength(10)]
        public string? Gender { get; set; }

        public DateTime? DateOfBirth { get; set; }

        [MaxLength(500)]
        public string? Address { get; set; }

        [Required]
        public int CityId { get; set; }

        [Required]
        public int StateId { get; set; }

        [MaxLength(10)]
        public string? PinCode { get; set; }

        [MaxLength(200)]
        public string? Qualification { get; set; }

        [MaxLength(200)]
        public string? Specialization { get; set; }

        [MaxLength(100)]
        public string? Experience { get; set; }

        [Required]
        public int FacultyId { get; set; }

        [MaxLength(500)]
        public string? Department { get; set; }

        [Required]
        public int CourseId { get; set; }

        [MaxLength(500)]
        public string? ProfilePhotoUrl { get; set; }

        [MaxLength(1000)]
        public string? Bio { get; set; }

        [MaxLength(50)]
        public string Status { get; set; } = "active"; // active, inactive, on-leave

        public DateTime? JoiningDate { get; set; }

        [MaxLength(500)]
        public string? Remarks { get; set; }

        [ForeignKey(nameof(StateId))]
        public virtual State State { get; set; } = null!;
        [ForeignKey(nameof(CityId))]
        public virtual City City { get; set; } = null!;

        [ForeignKey(nameof(CourseId))]
        public virtual Course Course { get; set; } = null!;

        [ForeignKey(nameof(FacultyId))]
        public virtual Faculty Faculty { get; set; } = null!;

        // Navigation property to ApplicationUser
        [ForeignKey(nameof(UserId))]
        public virtual ApplicationUser User { get; set; } = null!;
    }
}

