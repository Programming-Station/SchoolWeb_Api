using System.ComponentModel.DataAnnotations;

namespace School.Models.Fee
{
    public class FeeTypeModel
    {

        public int Id { get; set; }
        [Required, MaxLength(100)]
        public string Name { get; set; }
        [MaxLength(50)]
        public string Description { get; set; }
        public bool IsActive { get; set; } = true;
        public string CreatedBy { get; set; }
    }
}
