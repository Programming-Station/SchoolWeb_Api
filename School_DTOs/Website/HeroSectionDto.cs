namespace School_DTOs.Website
{
    public class HeroSectionDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Subtitle { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Button1Text { get; set; } = null!;
        public string? Button1Link { get; set; }
        public string Button2Text { get; set; } = null!;
        public string? Button2Link { get; set; }
        public string? BackgroundImageUrl { get; set; }
        public bool IsActive { get; set; }
    }
}
