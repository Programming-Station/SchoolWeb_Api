using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using School.Domain.School;
using static School.Domain.BaseEntity;

namespace School.Domain.Hostel
{
    [Table("MessMenus", Schema = "Hostel")]
    public class MessMenu : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        public int HostelId { get; set; }

        [Required, MaxLength(50)]
        public string DayOfWeek { get; set; } = null!; // Monday, Tuesday, etc.

        [Required, MaxLength(50)]
        public string MealType { get; set; } = null!; // Breakfast, Lunch, Snacks, Dinner

        [Required, MaxLength(2000)]
        public string FoodItems { get; set; } = null!;

        [MaxLength(1000)]
        public string? SpecialItems { get; set; }

        public int SchoolRegistrationId { get; set; }

        [ForeignKey(nameof(HostelId))]
        public virtual Hostel Hostel { get; set; } = null!;

        [ForeignKey(nameof(SchoolRegistrationId))]
        public virtual SchoolRegistration SchoolRegistration { get; set; } = null!;
    }
}
