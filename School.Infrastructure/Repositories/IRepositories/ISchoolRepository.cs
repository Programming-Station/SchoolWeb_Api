using School.Domain.School;
using System;
using System.Collections.Generic;
using System.Text;

namespace School.Infrastructure.Repositories.IRepositories
{
    public interface ISchoolRepository
    {
        Task<IEnumerable<schoolRegistion>> GetAllSchoolsAsync();

        Task<schoolRegistion?> GetSchoolByIdAsync(int id);

        Task<schoolRegistion> AddSchoolAsync(schoolRegistion entity);

        Task<int> UpdateSchoolAsync(schoolRegistion entity);

        Task<int> DeleteSchoolAsync(int id);
    }
}
