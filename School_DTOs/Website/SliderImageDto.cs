namespace School_DTOs.Website
{
    public class SliderImageDto
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; } = null!;
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? AltText { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; }
    }
}
