using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using School.Models.City;
using School.Models.State;

namespace School.Models.AffiliationCollege
{
    public class AffiliatedModel
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(200)]
        public string CollegeName { get; set; }

        [MaxLength(50)]
        public string CollegeCode { get; set; }

        [MaxLength(200)]
        public string UniversityName { get; set; }

        [MaxLength(50)]
        public string? UniversityCode { get; set; }

        [ForeignKey("State")]
        public int StateId { get; set; }
        public StateModel State { get; set; }

        [ForeignKey("City")]
        public int CityId { get; set; }
        public CityModel City { get; set; }


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
        public DateTime? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
