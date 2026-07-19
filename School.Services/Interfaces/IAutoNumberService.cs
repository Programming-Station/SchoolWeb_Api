using System.Threading.Tasks;

namespace School.Services.Interfaces
{
    public interface IAutoNumberService
    {
        Task<string> GenerateNextNumberAsync(string entityType, int schoolId);
    }
}
