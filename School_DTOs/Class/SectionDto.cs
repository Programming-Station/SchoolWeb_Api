namespace School_DTOs.Class
{
    public class SectionDto : BaseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int ClassId { get; set; }
        public string ClassName { get; set; } = string.Empty;
        public string Status { get; set; } = "active";
    }
}
