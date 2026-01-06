using System.ComponentModel.DataAnnotations; 
using static School.Domain.BaseEntity;

namespace School.Domain
{
    public class AcademicYear : AuditEntity<int>
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(50)] 
        public string YearName { get; set; } = null!; // e.g., "2024-2025"

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        public bool IsActive { get; set; } = true;

        public bool IsCurrent { get; set; } = false; // Only one can be current
    }
}

