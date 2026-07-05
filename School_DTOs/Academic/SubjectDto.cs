namespace School_DTOs.Academic
{
    public class SubjectDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Code { get; set; }
        public string? Description { get; set; }
        public int DisplayOrder { get; set; }
        public string Status { get; set; } = "active";
    }
    public class CreateSubjectDto
    {
        public string Name { get; set; } = null!;
        public string? Code { get; set; }
        public string? Description { get; set; }
        public int DisplayOrder { get; set; }
        public string Status { get; set; } = "active";
    }
    public class UpdateSubjectDto : CreateSubjectDto { public int Id { get; set; } }
}
