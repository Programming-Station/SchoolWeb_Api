using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static School.Domain.BaseEntity;

namespace School.Domain.Location
{
    public class City : AuditEntity<int>
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; }
        [MaxLength(10)]
        public string? CityCode { get; set; }
        public int StateId { get; set; }

        [MaxLength(1000)]
        public string? Description { get; set; }

        public bool IsActive { get; set; } = true;

        [ForeignKey(nameof(StateId))]
        public virtual State State { get; set; }
    }
}

