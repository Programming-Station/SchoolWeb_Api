using System.ComponentModel.DataAnnotations;
using static School.Domain.BaseEntity;

namespace School.Domain.Location
{
    public class Country : AuditEntity<int>
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(10)]
        public string? CountryCode { get; set; }

        public string? Currency { get; set; }
        public string? CurrencySymbol { get; set; }

        public virtual ICollection<State> States { get; set; } = new List<State>();
    }
}
