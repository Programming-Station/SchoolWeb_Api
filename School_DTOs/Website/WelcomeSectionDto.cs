namespace School_DTOs.Website
{
    public class WelcomeSectionDto
    {
        public int Id { get; set; }
        public string TitlePart1 { get; set; } = null!;
        public string TitlePart2 { get; set; } = null!;
        public string Description { get; set; } = null!;
        public bool IsActive { get; set; }
    }
}
