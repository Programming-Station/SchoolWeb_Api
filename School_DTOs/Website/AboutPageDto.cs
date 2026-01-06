namespace School_DTOs.Website
{
    public class AboutPageDto
    {
        public int Id { get; set; }
        public string MainTitle { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Mission { get; set; } = null!;
        public string Vision { get; set; } = null!;
        public string? Values { get; set; }
        public bool IsActive { get; set; }
    }
}
