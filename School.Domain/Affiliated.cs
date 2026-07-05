using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using School.Domain.Location;
using School.Domain.School;
using static School.Domain.BaseEntity;

namespace School.Domain
{
    public class Affiliated : AuditEntity<int>
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(200)]
        public string CollegeName { get; set; } = null!;

        [MaxLength(50)]
        public string? CollegeCode { get; set; }

        [MaxLength(200)]
        public string? UniversityName { get; set; }

        [MaxLength(50)]
        public string? UniversityCode { get; set; }

        [Required]
        public int StateId { get; set; }

        [Required]
        public int CityId { get; set; }

        public string? ImagePath { get; set; }

        [MaxLength(500)]
        public string? Address { get; set; }

        [MaxLength(10)]
        public string? Pincode { get; set; }

        [MaxLength(150)]
        public string? ContactPerson { get; set; }

        [MaxLength(15)]
        public string? MobileNo { get; set; }

        [MaxLength(150)]
        public string? Email { get; set; }

        public bool IsActive { get; set; } = true;

        [ForeignKey(nameof(StateId))]
        public virtual State State { get; set; } = null!; 
        
        public int SchoolRegistrationId { get; set; }
        [ForeignKey(nameof(SchoolRegistrationId))]
        public virtual SchoolRegistration SchoolRegistration { get; set; } = null!;
    }
}
