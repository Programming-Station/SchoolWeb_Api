using System.Collections.Generic;
using System.Threading.Tasks;
using School.Models.Finance;
using School_DTOs;
using School_DTOs.Finance;

namespace School.Services.Interfaces
{
    public interface IIncomeService
    {
        Task<APIResponse<IEnumerable<IncomeDto>>> GetAllIncomesAsync(int schoolId);
        Task<APIResponse<IncomeDto>> GetIncomeByIdAsync(int id, int schoolId);
        Task<APIResponse<IncomeDto>> CreateIncomeAsync(IncomeModel model, int schoolId);
        Task<APIResponse<IncomeDto>> UpdateIncomeAsync(IncomeModel model, int schoolId);
        Task<APIResponse<bool>> DeleteIncomeAsync(int id, int schoolId);
    }
}
