using System.Collections.Generic;
using System.Threading.Tasks;
using School_DTOs;
using School_DTOs.Administration;

namespace School.Services.Interfaces
{
    public interface IAdministrationService
    {
        // School Branch Management
        Task<APIResponse<List<SchoolBranchDto>>> GetBranchesAsync(int schoolId);
        Task<APIResponse<SchoolBranchDto>> GetBranchByIdAsync(int id, int schoolId);
        Task<APIResponse<SchoolBranchDto>> SaveBranchAsync(SchoolBranchDto dto, int schoolId);
        Task<APIResponse<bool>> DeleteBranchAsync(int id, int schoolId);

        // Workflow Engine
        Task<APIResponse<List<WorkflowDefinitionDto>>> GetWorkflowsAsync(int schoolId);
        Task<APIResponse<WorkflowDefinitionDto>> GetWorkflowByIdAsync(int id, int schoolId);
        Task<APIResponse<WorkflowDefinitionDto>> SaveWorkflowAsync(WorkflowDefinitionDto dto, int schoolId);
        Task<APIResponse<bool>> DeleteWorkflowAsync(int id, int schoolId);

        // Workflow Instances & Approval
        Task<APIResponse<List<WorkflowInstanceDto>>> GetWorkflowInstancesAsync(WorkflowInstanceFilterDto filter, int schoolId);
        Task<APIResponse<WorkflowInstanceDto>> StartWorkflowInstanceAsync(int definitionId, string entityName, int entityId, string initiatedBy, int schoolId);
        Task<APIResponse<WorkflowInstanceDto>> SubmitApprovalAsync(int instanceId, string approvedBy, string action, string comments, int schoolId);

        // Audit Logs & Activity Logs
        Task<APIResponse<List<AdminAuditLogDto>>> GetAuditLogsAsync(AuditLogFilterDto filter, int schoolId);
        Task<APIResponse<bool>> LogAuditAsync(AdminAuditLogDto dto, int schoolId);

        // System Settings & Operations
        Task<APIResponse<Dictionary<string, string>>> GetSystemSettingsAsync(int schoolId);
        Task<APIResponse<bool>> SaveSystemSettingsAsync(Dictionary<string, string> settings, int schoolId);
        Task<APIResponse<bool>> TriggerBackupAsync(int schoolId);
        Task<APIResponse<bool>> TriggerRestoreAsync(string backupFile, int schoolId);
        Task<APIResponse<bool>> ImportDataAsync(string entityName, byte[] fileData, int schoolId);
        Task<APIResponse<byte[]>> ExportDataAsync(string entityName, int schoolId);
    }
}
