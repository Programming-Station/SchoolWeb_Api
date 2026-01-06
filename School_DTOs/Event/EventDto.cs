namespace School_DTOs.Event
{
    public class EventDto : BaseDto
    {
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public string EventDate { get; set; } = null!;
        public string? Location { get; set; }
        public string? ImagePath { get; set; }
        public bool IsActive { get; set; }
        public bool IsUpcoming { get; set; }
    }
}

