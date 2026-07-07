using School.Domain.FeeManagnment;
using System;
using System.Collections.Generic;
using System.Text;

namespace School.Infrastructure.Repositories.IRepositories
{
    public interface IFeeTypeRepository
    {
        Task<FeeType> AddFeeTypeAsync(FeeType entity);
        Task<IEnumerable<FeeType>> GetFeeTypeBySchoolIdAsync(int? schoolId = null);
    }
}
