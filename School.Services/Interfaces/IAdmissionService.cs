using School.Models.Academic;
using School.Models.Fee;
using School.Models.School;
using School.Models.Student;
using School_DTOs;
using School_DTOs.Academic;
using School_DTOs.Fee;
using School_DTOs.School;
using School_DTOs.Student;

namespace School.Services.Interfaces
{
    public interface IAdmissionService
    {
        // Campus CRUD
        Task<APIResponse<CampusDto>> CreateCampusAsync(CampusModel model, string username);
        Task<APIResponse<CampusDto>> GetCampusByIdAsync(int id);
        Task<APIResponse<IEnumerable<CampusDto>>> GetAllCampusesAsync(bool? isActive = null);
        Task<APIResponse> UpdateCampusAsync(CampusModel model, string username);
        Task<APIResponse> DeleteCampusAsync(int id);

        // Education Level CRUD
        Task<APIResponse<EducationLevelDto>> CreateEducationLevelAsync(EducationLevelModel model, string username);
        Task<APIResponse<EducationLevelDto>> GetEducationLevelByIdAsync(int id);
        Task<APIResponse<IEnumerable<EducationLevelDto>>> GetAllEducationLevelsAsync(bool? isActive = null);
        Task<APIResponse> UpdateEducationLevelAsync(EducationLevelModel model, string username);
        Task<APIResponse> DeleteEducationLevelAsync(int id);

        // Program CRUD
        Task<APIResponse<ProgramDto>> CreateProgramAsync(ProgramModel model, string username);
        Task<APIResponse<ProgramDto>> GetProgramByIdAsync(int id);
        Task<APIResponse<IEnumerable<ProgramDto>>> GetAllProgramsAsync(int? educationLevelId = null, bool? isActive = null);
        Task<APIResponse> UpdateProgramAsync(ProgramModel model, string username);
        Task<APIResponse> DeleteProgramAsync(int id);

        // Branch CRUD
        Task<APIResponse<BranchDto>> CreateBranchAsync(BranchModel model, string username);
        Task<APIResponse<BranchDto>> GetBranchByIdAsync(int id);
        Task<APIResponse<IEnumerable<BranchDto>>> GetAllBranchesAsync(int? programId = null, bool? isActive = null);
        Task<APIResponse> UpdateBranchAsync(BranchModel model, string username);
        Task<APIResponse> DeleteBranchAsync(int id);

        // Year/Semester CRUD
        Task<APIResponse<YearSemesterDto>> CreateYearSemesterAsync(YearSemesterModel model, string username);
        Task<APIResponse<YearSemesterDto>> GetYearSemesterByIdAsync(int id);
        Task<APIResponse<IEnumerable<YearSemesterDto>>> GetAllYearSemestersAsync(bool? isActive = null);
        Task<APIResponse> UpdateYearSemesterAsync(YearSemesterModel model, string username);
        Task<APIResponse> DeleteYearSemesterAsync(int id);

        // Batch CRUD
        Task<APIResponse<BatchDto>> CreateBatchAsync(BatchModel model, string username);
        Task<APIResponse<BatchDto>> GetBatchByIdAsync(int id);
        Task<APIResponse<IEnumerable<BatchDto>>> GetAllBatchesAsync(int? programId = null, bool? isActive = null);
        Task<APIResponse> UpdateBatchAsync(BatchModel model, string username);
        Task<APIResponse> DeleteBatchAsync(int id);

        // Config CRUD
        Task<APIResponse<AdmissionFormConfigDto>> CreateFormConfigAsync(AdmissionFormConfigModel model, string username);
        Task<APIResponse<AdmissionFormConfigDto>> GetFormConfigByIdAsync(int id);
        Task<APIResponse<AdmissionFormConfigDto>> GetFormConfigAsync(int campusId, int educationLevelId, int? programId);
        Task<APIResponse<IEnumerable<AdmissionFormConfigDto>>> GetAllFormConfigsAsync();
        Task<APIResponse> UpdateFormConfigAsync(AdmissionFormConfigModel model, string username);
        Task<APIResponse> DeleteFormConfigAsync(int id);

        // Rule CRUD
        Task<APIResponse<AdmissionRuleDto>> CreateRuleAsync(AdmissionRuleModel model, string username);
        Task<APIResponse<AdmissionRuleDto>> GetRuleByIdAsync(int id);
        Task<APIResponse<IEnumerable<AdmissionRuleDto>>> GetAllRulesAsync();
        Task<APIResponse> UpdateRuleAsync(AdmissionRuleModel model, string username);
        Task<APIResponse> DeleteRuleAsync(int id);

        // Fee Structure CRUD
        Task<APIResponse<FeeStructureDto>> CreateFeeStructureAsync(FeeStructureModel model, string username);
        Task<APIResponse<FeeStructureDto>> GetFeeStructureByIdAsync(int id);
        Task<APIResponse<FeeStructureDto>> GetFeeStructureAsync(int campusId, int programId, int batchId);
        Task<APIResponse<IEnumerable<FeeStructureDto>>> GetAllFeeStructuresAsync();
        Task<APIResponse> UpdateFeeStructureAsync(FeeStructureModel model, string username);
        Task<APIResponse> DeleteFeeStructureAsync(int id);

        // Core Admission Applications
        Task<APIResponse<AdmissionApplicationDto>> SaveDraftAsync(AdmissionApplicationModel model, string username, int tenantId);
        Task<APIResponse<AdmissionApplicationDto>> SubmitApplicationAsync(AdmissionApplicationModel model, string username, int tenantId);
        Task<APIResponse<AdmissionApplicationDto>> GetApplicationByIdAsync(int id);
        Task<PagedResponse<AdmissionApplicationDto>> GetApplicationsListAsync(int pageNumber = 1, int pageSize = 10, string? searchTerm = null, string? status = null, int? campusId = null, int? programId = null);
        Task<APIResponse> VerifyDocumentAsync(int applicationId, string documentName, string status, string? notes, string username);
        Task<APIResponse> AssignFeeAsync(int applicationId, string assignedFeesJson, string username);
        Task<APIResponse> UpdateApplicationStatusAsync(UpdateAdmissionApplicationStatusDto dto, string username);
        Task<APIResponse<IEnumerable<AdmissionAuditLogDto>>> GetAuditLogsAsync(int applicationId);
        Task<APIResponse<object>> GetDashboardStatsAsync(int tenantId);
        Task<APIResponse<string>> GenerateBarcodeBase64Async(string data);
        Task<APIResponse<string>> GenerateQrCodeBase64Async(string url);
    }
}
