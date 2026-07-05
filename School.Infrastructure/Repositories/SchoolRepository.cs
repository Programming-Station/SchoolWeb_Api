using Azure.Core;
using Microsoft.EntityFrameworkCore;
using School.Domain.School;
using School.Infrastructure.Repositories.IRepositories;
using School.Infrastructure.UnitOfWork;
using School.Infrastructure.UnitOfWork.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace School.Infrastructure.Repositories
{
    public class SchoolRepository:Repository<schoolRegistion>, ISchoolRepository
    {
        private readonly SchoolDbContext _context;
        private readonly IUnitOfWork _unitOfWork;
        public SchoolRepository(DbFactory dbFactory, SchoolDbContext context, IUnitOfWork unitOfWork):base(dbFactory)
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }

        public async Task<schoolRegistion> AddSchoolAsync(schoolRegistion entity)
        {
            entity = await AddAsync(entity);
            await _unitOfWork.CommitAsync();
            return entity;
          
        }

        public async Task<int> DeleteSchoolAsync(int id)
        {
            var result = await FindAsync(x => x.Id == id);
            if (result != null)
            {
                result.UpdatedDate = DateTime.Now;
                result.UpdatedBy = "Superadmin";
                this.Delete(result);
                return await _unitOfWork.CommitAsync().ConfigureAwait(false);
            }
           else
               return 0;
           
        }

        public async Task<IEnumerable<schoolRegistion>> GetAllSchoolsAsync()
        {
            return await List(expression: x => !x.IsDeleted).ToListAsync();
        }

        public async Task<schoolRegistion?> GetSchoolByIdAsync(int id)
        {
            return await FindAsync(expression: x => x.Id == id) ?? new schoolRegistion();
        }

        public async Task<int> UpdateSchoolAsync(schoolRegistion entity)
        {
            Attach(entity, updatedProperties: new Expression<Func<schoolRegistion, object>>[]
            {
                  u => u.SchoolName,
                  u => u.SchoolCode,
                  u => u.EstablishedYear,
                  u => u.Address,
                  u => u.Pincode,
                  u => u.StateId,
                  u => u.CityId,
                  u => u.Logo,
                  u => u.IsActive
            });
            return await _unitOfWork.CommitAsync().ConfigureAwait(false);
        }
    }
}
