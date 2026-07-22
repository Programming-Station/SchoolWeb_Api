using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static School.Domain.BaseEntity;

namespace School.Domain.Communication
{
    [Table("GroupChatRooms", Schema = "Communication")]
    public class GroupChatRoom : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; } = null!;

        [Required, MaxLength(20)]
        public string Type { get; set; } = "Group"; // Group, Department, Class, Broadcast

        [MaxLength(100)]
        public string? TargetReferenceId { get; set; } // ClassId or DepartmentId if mapped auto-generated channel

        public int SchoolRegistrationId { get; set; }
    }
}
