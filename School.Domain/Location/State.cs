using System.ComponentModel.DataAnnotations;
using static School.Domain.BaseEntity;

namespace School.Domain.Location
{
    public class State : AuditEntity<int>
    {
        [Key]
        public int Id { get; set; }
        [Required, MaxLength(100)]
        public string Name { get; set; }
        [MaxLength(10)]
        public string? StateCode { get; set; }

        [MaxLength(1000)]
        public string? Description { get; set; }
        public int CountryId { get; set; }
        [System.ComponentModel.DataAnnotations.Schema.ForeignKey(nameof(CountryId))]
        public virtual Country Country { get; set; } = null!;

        public bool IsActive { get; set; } = true;
        public virtual ICollection<City> Cities { get; set; } = new List<City>();
    }
}
