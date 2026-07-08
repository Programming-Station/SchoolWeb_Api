using System;

namespace School_DTOs.Academic
{
    public class BranchDto : BaseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public int ProgramId { get; set; }
        public string ProgramName { get; set; } = string.Empty;
        public string Status { get; set; } = "active";
    }
}
