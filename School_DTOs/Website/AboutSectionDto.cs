namespace School_DTOs.Website
{
    public class AboutSectionDto
    {
        public int Id { get; set; }
        public string Description { get; set; } = null!;
        public string CompanyNumber { get; set; } = null!;
        public string RegisteredDate { get; set; } = null!;
        public string Location { get; set; } = null!;
        public bool IsActive { get; set; }
    }
}
