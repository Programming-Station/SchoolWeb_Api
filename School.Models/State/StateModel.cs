using System.ComponentModel.DataAnnotations;
using School.Models.City;
using School.Models.CustomeVailidation;

namespace School.Models.State
{
    public class StateModel
    {
        public int Id { get; set; }

        [NoScript]
        [Required, MaxLength(100)]
        public string Name { get; set; }
        public string StateCode { get; set; }
        public ICollection<CityModel> Cities { get; set; } = new List<CityModel>();

        [MaxLength(1000)]
        public string? Description { get; set; }

        public bool IsActive { get; set; } = true;
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
