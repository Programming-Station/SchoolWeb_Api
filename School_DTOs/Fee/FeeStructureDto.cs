using System.Collections.Generic;

namespace School_DTOs.Fee
{
    public class FeeStructureDto : BaseDto
    {
        public FeeStructureDto()
        {
            FeeStructureItems = new List<FeeStructureItemDto>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int CampusId { get; set; }
        public string CampusName { get; set; } = string.Empty;
        public int ProgramId { get; set; }
        public string ProgramName { get; set; } = string.Empty;
        public int BatchId { get; set; }
        public string BatchName { get; set; } = string.Empty;
        public bool IsActive { get; set; }

        public List<FeeStructureItemDto> FeeStructureItems { get; set; }
    }

    public class FeeStructureItemDto
    {
        public int Id { get; set; }
        public int FeeStructureId { get; set; }
        public int FeeTypeId { get; set; }
        public string FeeTypeName { get; set; } = string.Empty;
        public decimal Amount { get; set; }
    }
}
