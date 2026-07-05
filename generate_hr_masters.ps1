$domainPath = "e:\GIT\SchoolSAAS\SchoolWeb_Api\School.Domain\Hr"

# BloodGroupMaster
$content = @"
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static School.Domain.BaseEntity;
using School.Domain.School;

namespace School.Domain.Hr
{
    public class BloodGroupMaster : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; } = null!;

        [Required, MaxLength(50)]
        public string Status { get; set; } = `"active`"; 

        public int SchoolRegistrationId { get; set; }
        
        [ForeignKey(nameof(SchoolRegistrationId))]
        public virtual SchoolRegistration SchoolRegistration { get; set; } = null!;
    }
}
"@
Set-Content -Path "$domainPath\BloodGroupMaster.cs" -Value $content

# ReligionMaster
$content = @"
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static School.Domain.BaseEntity;
using School.Domain.School;

namespace School.Domain.Hr
{
    public class ReligionMaster : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; } = null!;

        [Required, MaxLength(50)]
        public string Status { get; set; } = `"active`"; 

        public int SchoolRegistrationId { get; set; }
        
        [ForeignKey(nameof(SchoolRegistrationId))]
        public virtual SchoolRegistration SchoolRegistration { get; set; } = null!;
    }
}
"@
Set-Content -Path "$domainPath\ReligionMaster.cs" -Value $content

# QualificationMaster
$content = @"
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static School.Domain.BaseEntity;
using School.Domain.School;

namespace School.Domain.Hr
{
    public class QualificationMaster : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(200)]
        public string Name { get; set; } = null!;

        [MaxLength(50)]
        public string? Code { get; set; }

        [Required, MaxLength(50)]
        public string Status { get; set; } = `"active`"; 

        public int SchoolRegistrationId { get; set; }
        
        [ForeignKey(nameof(SchoolRegistrationId))]
        public virtual SchoolRegistration SchoolRegistration { get; set; } = null!;
    }
}
"@
Set-Content -Path "$domainPath\QualificationMaster.cs" -Value $content
