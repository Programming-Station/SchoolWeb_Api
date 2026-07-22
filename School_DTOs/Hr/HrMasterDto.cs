namespace School_DTOs.Hr
{
    public class HrMasterDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Code { get; set; }
        public string? Description { get; set; }
        public string Status { get; set; } = null!;
    }

    public class CreateHrMasterDto
    {
        public string Name { get; set; } = null!;
        public string? Code { get; set; }
        public string? Description { get; set; }
        public string Status { get; set; } = "active";
    }

    public class UpdateHrMasterDto : CreateHrMasterDto
    {
        public int Id { get; set; }
    }

    public class BulkStatusChangeDto
    {
        public System.Collections.Generic.IEnumerable<int> Ids { get; set; } = null!;
        public string Status { get; set; } = null!;
    }
}
