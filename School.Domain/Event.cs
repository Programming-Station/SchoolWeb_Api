using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using School.Domain.School;
using static School.Domain.BaseEntity;

namespace School.Domain
{
    public class Event : AuditEntity<int>
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(200)]
        public string Title { get; set; } = null!;

        [MaxLength(2000)]
        public string? Description { get; set; }

        [Required]
        public DateTime EventDate { get; set; }

        [MaxLength(100)]
        public string? Location { get; set; }

        [MaxLength(200)]
        public string? ImagePath { get; set; }

        public bool IsActive { get; set; } = true;

        public bool IsUpcoming { get; set; } = true; // Event is upcoming if EventDate >= Today

        public int SchoolRegistrationId { get; set; }
        [ForeignKey(nameof(SchoolRegistrationId))]
        public virtual SchoolRegistration SchoolRegistration { get; set; } = null!;
    }
}

