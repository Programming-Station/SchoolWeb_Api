using School_DTOs.Fee;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace School.Services.Interfaces
{
    public interface IFineCalculationService
    {
        // Rules configuration
        Task<(bool Success, string Message, FineRuleDto? Rule)> CreateRuleAsync(FineRuleDto dto, string createdBy, int schoolId);
        Task<IEnumerable<FineRuleDto>> GetAllRulesAsync(int schoolId);
        Task<(bool Success, string Message)> UpdateRuleAsync(FineRuleDto dto);
        Task<(bool Success, string Message)> DeleteRuleAsync(int id);

        // Waiving/Management
        Task<System.Collections.Generic.IEnumerable<School_DTOs.Fee.FeeFineDto>> GetFinesAsync(int schoolId);
        Task<(bool Success, string Message)> WaiveFineAsync(int fineId, string reason, string updatedBy);

        // Calculation job
        Task<(bool Success, string Message)> RunDailyFineCalculationAsync(int schoolId);
    }
}
