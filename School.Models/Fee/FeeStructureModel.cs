using System.ComponentModel.DataAnnotations;

namespace School.Models.Fee
{
    public class FeeStructureModel
    {
        public FeeStructureModel()
        {
            FeeStructureItems = new List<FeeStructureItemModel>();
        }

        public int? Id { get; set; }

        [Required(ErrorMessage = "Fee Structure Name is required")]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Campus selection is required")]
        public int CampusId { get; set; }

        [Required(ErrorMessage = "Program selection is required")]
        public int ProgramId { get; set; }

        [Required(ErrorMessage = "Batch selection is required")]
        public int BatchId { get; set; }

        public bool IsActive { get; set; } = true;

        public List<FeeStructureItemModel> FeeStructureItems { get; set; }
    }

    public class FeeStructureItemModel
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "Fee Type selection is required")]
        public int FeeTypeId { get; set; }

        [Required(ErrorMessage = "Amount is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Amount must be greater than or equal to 0")]
        public decimal Amount { get; set; }
    }
}
