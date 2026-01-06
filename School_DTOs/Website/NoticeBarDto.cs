namespace School_DTOs.Website
{
    public class NoticeBarDto
    {
        public int Id { get; set; }
        public string NoticeText { get; set; } = null!;
        public string ContactInfo { get; set; } = null!;
        public bool IsActive { get; set; }
    }
}
