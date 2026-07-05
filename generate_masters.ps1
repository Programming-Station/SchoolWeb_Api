$models = @(
    "EmployeeCategory", "EmployeeType", "EmploymentStatus", "SalaryGrade", 
    "ShiftMaster", "HolidayMaster", "WeekOff", "NoticePeriod", "LeaveType", "LeaveSetting"
)

$basePath = "e:\GIT\SchoolSAAS\SchoolWeb_Api\School.Domain\Hr"

foreach ($model in $models) {
    $content = @"
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static School.Domain.BaseEntity;
using School.Domain.School;

namespace School.Domain.Hr
{
    public class $model : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(200)]
        public string Name { get; set; } = null!;

        [MaxLength(50)]
        public string? Code { get; set; }

        [MaxLength(1000)]
        public string? Description { get; set; }

        [Required, MaxLength(50)]
        public string Status { get; set; } = `"active`"; 

        public int SchoolRegistrationId { get; set; }
        
        [ForeignKey(nameof(SchoolRegistrationId))]
        public virtual SchoolRegistration SchoolRegistration { get; set; } = null!;
    }
}
"@
    Set-Content -Path "$basePath\$model.cs" -Value $content
}
