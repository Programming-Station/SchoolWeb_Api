using System.Collections.Generic;
using System.Threading.Tasks;
using School.Models.Finance;
using School_DTOs;
using School_DTOs.Finance;

namespace School.Services.Interfaces
{
    public interface IExpenseService
    {
        Task<APIResponse<IEnumerable<ExpenseDto>>> GetAllExpensesAsync(int schoolId);
        Task<APIResponse<ExpenseDto>> GetExpenseByIdAsync(int id, int schoolId);
        Task<APIResponse<ExpenseDto>> CreateExpenseAsync(ExpenseModel model, int schoolId);
        Task<APIResponse<ExpenseDto>> UpdateExpenseAsync(ExpenseModel model, int schoolId);
        Task<APIResponse<bool>> DeleteExpenseAsync(int id, int schoolId);
    }
}
