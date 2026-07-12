using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static School.Domain.BaseEntity;

namespace School.Domain.Administration
{
    [Table("SchoolBranches", Schema = "Administration")]
    public class SchoolBranch : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(150)]
        public string BranchName { get; set; } = null!;

        [Required, MaxLength(20)]
        public string Code { get; set; } = null!;

        [MaxLength(500)]
        public string? Address { get; set; }

        public bool IsActive { get; set; } = true;

        public int SchoolRegistrationId { get; set; }
    }
}
