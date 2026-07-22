using System.ComponentModel.DataAnnotations;
using School.Models.CustomeVailidation;

namespace School.Models.City
{
    public class CityModel
    {
        public int Id { get; set; }
        [Required, MaxLength(100)]
        [NoScript]
        public string Name { get; set; }
        public string? CityCode { get; set; }
        public int StateId { get; set; }
        [MaxLength(1000)]
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
