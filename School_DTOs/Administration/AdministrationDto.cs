using System;
using System.Collections.Generic;

namespace School_DTOs.Administration
{
    public class SchoolBranchDto : BaseDto
    {
        public string BranchName { get; set; } = null!;
        public string Code { get; set; } = null!;
        public string? Address { get; set; }
        public bool IsActive { get; set; } = true;
        public int SchoolRegistrationId { get; set; }
    }

    public class WorkflowDefinitionDto : BaseDto
    {
        public string Name { get; set; } = null!;
        public string TriggerType { get; set; } = null!;
        public bool IsActive { get; set; } = true;
        public int SchoolRegistrationId { get; set; }
        public List<WorkflowStepDto> Steps { get; set; } = new();
    }

    public class WorkflowStepDto : BaseDto
    {
        public int WorkflowDefinitionId { get; set; }
        public int StepNumber { get; set; }
        public string ApproverRole { get; set; } = null!;
        public int? TimeoutHours { get; set; }
        public int SchoolRegistrationId { get; set; }
    }

    public class WorkflowInstanceDto : BaseDto
    {
        public int WorkflowDefinitionId { get; set; }
        public string WorkflowName { get; set; } = null!;
        public string TargetEntityName { get; set; } = null!;
        public int TargetEntityId { get; set; }
        public int CurrentStepId { get; set; }
        public string Status { get; set; } = "Pending";
        public int SchoolRegistrationId { get; set; }
        public List<ApprovalLogDto> ApprovalLogs { get; set; } = new();
    }

    public class ApprovalLogDto : BaseDto
    {
        public int WorkflowInstanceId { get; set; }
        public string ApprovedByUserId { get; set; } = null!;
        public string ApprovedByUserName { get; set; } = null!;
        public string Action { get; set; } = null!;
        public string? Comments { get; set; }
        public DateTime ActionDate { get; set; }
        public int SchoolRegistrationId { get; set; }
    }

    public class AdminAuditLogDto : BaseDto
    {
        public string UserId { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string TableName { get; set; } = null!;
        public string Action { get; set; } = null!;
        public string? OldValuesJson { get; set; }
        public string? NewValuesJson { get; set; }
        public DateTime Timestamp { get; set; }
        public int SchoolRegistrationId { get; set; }
    }

    public class AuditLogFilterDto
    {
        public string? TableName { get; set; }
        public string? Action { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }

    public class WorkflowInstanceFilterDto
    {
        public string? Status { get; set; }
        public string? TargetEntityName { get; set; }
    }
}
