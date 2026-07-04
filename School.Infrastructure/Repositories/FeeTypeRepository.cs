using Microsoft.EntityFrameworkCore;
using School.Domain.FeeManagnment;
using School.Infrastructure.Repositories.IRepositories;
using School.Infrastructure.UnitOfWork;
using School.Infrastructure.UnitOfWork.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace School.Infrastructure.Repositories
{
    
    public class FeeTypeRepository: Repository<FeeType>, IFeeTypeRepository
    {
        private readonly SchoolDbContext _context;
        private readonly IUnitOfWork _unitOfWork;
        public FeeTypeRepository(DbFactory dbFactory, SchoolDbContext context, IUnitOfWork unitOfWork):base(dbFactory)
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }
        public async Task<FeeType> AddFeeTypeAsync(FeeType entity)
        {
            // check for dublicates FeeType Name
            var existingFeeType = await DbSet.FirstOrDefaultAsync(x => x.Name.ToUpper() == entity.Name.ToLower() &&
                                                                  x.SchoolId==entity.SchoolId);
            if(existingFeeType != null)
            {
                existingFeeType.Id = 0;
                return existingFeeType;
            }
            await AddAsync(entity);
            await _unitOfWork.CommitAsync();
            return entity;

        }
        
        public async Task<IEnumerable<FeeType>> GetFeeTypeBySchoolIdAsync(int? schoolId =null)
        {
            return schoolId == null ? await List(expression: x => !x.IsDeleted).ToListAsync() :
                await List(expression: x => !x.IsDeleted && x.SchoolId == schoolId).ToListAsync();
        }

        public async Task<int> UpdateFeeTypeBySchoolIdAsync(FeeType entity)
        {
            Attach(entity, updatedProperties: new Expression<Func<FeeType, object>>[]
            {
                u => u.Name!,
                u=> u.School!,
                u=>u.Name!,
                u=>u.description!,
                u=>u.UpdatedBy!,
            });
            return await _unitOfWork.CommitAsync().ConfigureAwait(false);
        }

        public async Task<int> DeleteFeeTypeBySchoolIdAsync(int id)
        {
            var entity = await FindAsync(expression: x => x.Id == id && !x.IsDeleted);
            var result = await FindAsync(expression: x => x.Id == id);
             if(result != null)
            {
                result.UpdatedDate = DateTime.Now;
                Delete(result);
                return await _unitOfWork.CommitAsync().ConfigureAwait(false);
            }
            else
            {
                return 0;
            }
        }
    }
}
