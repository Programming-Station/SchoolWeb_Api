using System.ComponentModel.DataAnnotations;

namespace School.Models.Event
{
    public class EventModel
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "Event title is required")]
        [MaxLength(200, ErrorMessage = "Event title cannot exceed 200 characters")]
        public string Title { get; set; } = null!;

        [MaxLength(2000, ErrorMessage = "Description cannot exceed 2000 characters")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Event date is required")]
        public DateTime EventDate { get; set; }

        [MaxLength(100, ErrorMessage = "Location cannot exceed 100 characters")]
        public string? Location { get; set; }

        [MaxLength(200, ErrorMessage = "Image path cannot exceed 200 characters")]
        public string? ImagePath { get; set; }

        public bool IsActive { get; set; } = true;
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
    }
}

