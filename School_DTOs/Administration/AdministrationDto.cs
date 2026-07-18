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

    // ════════════════════════════════════════════════════════════════════════
    // COMPLAINT DTOs
    // ════════════════════════════════════════════════════════════════════════
    public class ComplaintDto : BaseDto
    {
        public string ComplaintNumber { get; set; } = null!;
        public string Subject { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Category { get; set; } = "General";
        public string Priority { get; set; } = "Medium";
        public string Status { get; set; } = "Open";
        public string RaisedByUserId { get; set; } = null!;
        public string RaisedByRole { get; set; } = "Student";
        public int? StudentId { get; set; }
        public string? StudentName { get; set; }
        public int? EmployeeId { get; set; }
        public string? EmployeeName { get; set; }
        public string? AssignedToUserId { get; set; }
        public string? AssignedToName { get; set; }
        public string? AttachmentUrl { get; set; }
        public string? ResolutionDetails { get; set; }
        public DateTime? ResolvedDate { get; set; }
        public string? EscalationNotes { get; set; }
        public int? FeedbackRating { get; set; }
        public string? FeedbackComments { get; set; }
        public bool IsAnonymous { get; set; }
    }

    public class CreateComplaintDto
    {
        public string Subject { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Category { get; set; } = "General";
        public string Priority { get; set; } = "Medium";
        public string RaisedByRole { get; set; } = "Student";
        public int? StudentId { get; set; }
        public int? EmployeeId { get; set; }
        public string? AttachmentUrl { get; set; }
        public bool IsAnonymous { get; set; }
    }

    public class ComplaintFilterDto
    {
        public string? Status { get; set; }
        public string? Category { get; set; }
        public string? Priority { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }

    // ════════════════════════════════════════════════════════════════════════
    // VISITOR DTOs
    // ════════════════════════════════════════════════════════════════════════
    public class VisitorDto : BaseDto
    {
        public string VisitorNumber { get; set; } = null!;
        public string VisitorName { get; set; } = null!;
        public string? Organization { get; set; }
        public string ContactNumber { get; set; } = null!;
        public string? Email { get; set; }
        public string IdProofType { get; set; } = "Aadhaar";
        public string? IdProofNumber { get; set; }
        public string? PhotoUrl { get; set; }
        public string? IdProofUrl { get; set; }
        public string Purpose { get; set; } = null!;
        public string? PersonToMeet { get; set; }
        public string? Department { get; set; }
        public int? StudentId { get; set; }
        public string? StudentName { get; set; }
        public int NumberOfVisitors { get; set; }
        public DateTime CheckInTime { get; set; }
        public DateTime? CheckOutTime { get; set; }
        public string? VisitorBadgeNumber { get; set; }
        public string? Remarks { get; set; }
        public string? QrCode { get; set; }
        public string Status { get; set; } = "CheckedIn";
    }

    public class CreateVisitorDto
    {
        public string VisitorName { get; set; } = null!;
        public string? Organization { get; set; }
        public string ContactNumber { get; set; } = null!;
        public string? Email { get; set; }
        public string IdProofType { get; set; } = "Aadhaar";
        public string? IdProofNumber { get; set; }
        public string? PhotoUrl { get; set; }
        public string? IdProofUrl { get; set; }
        public string Purpose { get; set; } = null!;
        public string? PersonToMeet { get; set; }
        public string? Department { get; set; }
        public int? StudentId { get; set; }
        public int NumberOfVisitors { get; set; } = 1;
        public string? Remarks { get; set; }
    }

    public class VisitorFilterDto
    {
        public string? Status { get; set; }
        public string? Purpose { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }

    // ════════════════════════════════════════════════════════════════════════
    // CERTIFICATE ISSUANCE DTOs
    // ════════════════════════════════════════════════════════════════════════
    public class CertificateIssuanceLogDto : BaseDto
    {
        public string CertificateNumber { get; set; } = null!;
        public string CertificateType { get; set; } = null!;
        public int StudentId { get; set; }
        public string? StudentName { get; set; }
        public string? AdmissionNumber { get; set; }
        public string? ClassName { get; set; }
        public int AcademicYearId { get; set; }
        public string? AcademicYearName { get; set; }
        public DateTime IssuedDate { get; set; }
        public string? IssuedByName { get; set; }
        public string? Reason { get; set; }
        public string? PdfUrl { get; set; }
        public string? QrVerificationCode { get; set; }
        public int PrintCount { get; set; }
        public DateTime? LastPrintedDate { get; set; }
        public string Status { get; set; } = "Issued";
        public string? RevocationReason { get; set; }
    }

    public class CreateCertificateIssuanceDto
    {
        public string CertificateType { get; set; } = null!;
        public int StudentId { get; set; }
        public int AcademicYearId { get; set; }
        public string? Reason { get; set; }
    }

    public class CertificateFilterDto
    {
        public string? CertificateType { get; set; }
        public string? Status { get; set; }
        public int? StudentId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
