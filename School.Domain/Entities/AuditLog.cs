using System.ComponentModel.DataAnnotations;

namespace School.Domain.Entities
{
    public class AuditLog
    {
        public int Id { get; set; }
        [Required]
        public string UserId { get; set; } = string.Empty;
        [Required]
        public string Action { get; set; } = string.Empty; // e.g., Create, Update, Delete, Login, Logout
        public string? EntityName { get; set; }
        public string? EntityId { get; set; }
        public string? OldValues { get; set; }
        public string? NewValues { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public string? IPAddress { get; set; }
    }
}
