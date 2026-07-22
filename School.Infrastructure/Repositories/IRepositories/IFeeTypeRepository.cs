using School.Domain.FeeManagnment;

namespace School.Infrastructure.Repositories.IRepositories
{
    public interface IFeeTypeRepository
    {
        Task<FeeType> AddFeeTypeAsync(FeeType entity);
        Task<IEnumerable<FeeType>> GetFeeTypeBySchoolIdAsync(int? schoolId = null);
    }
}
