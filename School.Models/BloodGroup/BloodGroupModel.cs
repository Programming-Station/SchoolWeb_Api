using System.ComponentModel.DataAnnotations;

namespace School.Models.BloodGroup
{
    public class BloodGroupModel
    {
        public int BloodGropuId { get; set; }

        [Required(ErrorMessage = "Blood Group is required")]
        public string BloodGroup { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Current strength must be greater than or equal to 0")]
        public int CurrentStrength { get; set; } = 0;
        [Required(ErrorMessage = "Capacity is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Capacity must be greater than 0")]
        public int Capacity { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
