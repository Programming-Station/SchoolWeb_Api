using Microsoft.EntityFrameworkCore;
using School.Domain;
using School.Infrastructure.Repositories.IRepositories;
using School.Infrastructure.UnitOfWork.Interfaces;
using School.Services.Interfaces.Hr;
using School_DTOs.Common;
using School_DTOs.Hr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace School.Services.Hr
{
    public class EmployeeSalaryDetailService : IEmployeeSalaryDetailService
    {
        private readonly IRepository<EmployeeSalaryDetail> _repository;
        private readonly IUnitOfWork _unitOfWork;

        public EmployeeSalaryDetailService(IRepository<EmployeeSalaryDetail> repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<APIResponse<List<EmployeeSalaryDetailDto>>> GetAllByEmployeeIdAsync(int employeeId)
        {
            var data = await _repository.GetAll().Where(x => x.EmployeeId == employeeId).Select(x => new EmployeeSalaryDetailDto
            {
                Id = x.Id,
                EmployeeId = x.EmployeeId,
                Basic = x.Basic, HRA = x.HRA, DA = x.DA, PF = x.PF, ESI = x.ESI, NetSalary = x.NetSalary
            }).ToListAsync();

            return new APIResponse<List<EmployeeSalaryDetailDto>>(HttpStatusCode.OK, "Success", data);
        }

        public async Task<APIResponse<EmployeeSalaryDetailDto>> GetByIdAsync(int id)
        {
            var data = await _repository.GetAll().Where(x => x.Id == id).Select(x => new EmployeeSalaryDetailDto
            {
                Id = x.Id,
                EmployeeId = x.EmployeeId,
                Basic = x.Basic, HRA = x.HRA, DA = x.DA, PF = x.PF, ESI = x.ESI, NetSalary = x.NetSalary
            }).FirstOrDefaultAsync();

            if (data == null) return new APIResponse<EmployeeSalaryDetailDto>(HttpStatusCode.NotFound, "Not found");
            return new APIResponse<EmployeeSalaryDetailDto>(HttpStatusCode.OK, "Success", data);
        }

        public async Task<APIResponse<object>> CreateAsync(CreateEmployeeSalaryDetailDto dto, string username)
        {
            var entity = new EmployeeSalaryDetail
            {
                EmployeeId = dto.EmployeeId,
                Basic = dto.Basic, HRA = dto.HRA, DA = dto.DA, PF = dto.PF, ESI = dto.ESI, NetSalary = dto.NetSalary,
                CreatedBy = username,
                CreatedDate = DateTime.UtcNow
            };
            await _repository.AddAsync(entity);
            await _unitOfWork.CommitAsync();
            return new APIResponse<object>(HttpStatusCode.OK, "Created successfully");
        }

        public async Task<APIResponse<object>> UpdateAsync(int id, UpdateEmployeeSalaryDetailDto dto, string username)
        {
            if (id != dto.Id) return new APIResponse<object>(HttpStatusCode.BadRequest, "Id mismatch");
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return new APIResponse<object>(HttpStatusCode.NotFound, "Not found");

            entity.EmployeeId = dto.EmployeeId;
            entity.Basic = dto.Basic;
            entity.HRA = dto.HRA;
            entity.DA = dto.DA;
            entity.PF = dto.PF;
            entity.ESI = dto.ESI;
            entity.NetSalary = dto.NetSalary;
            entity.UpdatedBy = username;
            entity.UpdatedDate = DateTime.UtcNow;

            _repository.Update(entity);
            await _unitOfWork.CommitAsync();
            return new APIResponse<object>(HttpStatusCode.OK, "Updated successfully");
        }

        public async Task<APIResponse<object>> DeleteAsync(int id, string username)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return new APIResponse<object>(HttpStatusCode.NotFound, "Not found");
            _repository.Remove(entity);
            await _unitOfWork.CommitAsync();
            return new APIResponse<object>(HttpStatusCode.OK, "Deleted successfully");
        }
    }
}