$reposPath = "e:\GIT\SchoolSAAS\SchoolWeb_Api\School.Infrastructure\Repositories"
$servicesPath = "e:\GIT\SchoolSAAS\SchoolWeb_Api\School.Services"

# IEmployeeRepository
$content = @"
using School.Domain.Hr;
using School.Infrastructure.UnitOfWork.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace School.Infrastructure.Repositories.IRepositories
{
    public interface IEmployeeRepository : IRepository<Employee>
    {
        Task<Employee?> GetEmployeeWithDetailsAsync(int id);
        Task<IEnumerable<Employee>> GetAllEmployeesWithDetailsAsync();
        Task<bool> IsDuplicateEmployeeAsync(string email, string mobile, string? aadhaar, string? pan, int? excludeId = null);
    }
}
"@
Set-Content -Path "$reposPath\IRepositories\IEmployeeRepository.cs" -Value $content

# EmployeeRepository
$content = @"
using Microsoft.EntityFrameworkCore;
using School.Domain.Hr;
using School.Infrastructure.Repositories.IRepositories;
using School.Infrastructure.UnitOfWork;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace School.Infrastructure.Repositories
{
    public class EmployeeRepository : Repository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(DbFactory dbFactory) : base(dbFactory)
        {
        }

        public async Task<Employee?> GetEmployeeWithDetailsAsync(int id)
        {
            return await DbSet
                .Include(e => e.Department)
                .Include(e => e.Designation)
                .Include(e => e.Documents)
                .Include(e => e.BankDetails)
                .Include(e => e.Educations)
                .Include(e => e.Experiences)
                .Include(e => e.SalaryDetails)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<IEnumerable<Employee>> GetAllEmployeesWithDetailsAsync()
        {
            return await DbSet
                .Include(e => e.Department)
                .Include(e => e.Designation)
                .ToListAsync();
        }

        public async Task<bool> IsDuplicateEmployeeAsync(string email, string mobile, string? aadhaar, string? pan, int? excludeId = null)
        {
            var query = DbSet.AsQueryable();
            if (excludeId.HasValue)
            {
                query = query.Where(e => e.Id != excludeId.Value);
            }

            return await query.AnyAsync(e => 
                e.Email == email || 
                e.MobileNumber == mobile || 
                (!string.IsNullOrEmpty(aadhaar) && e.AadhaarNumber == aadhaar) ||
                (!string.IsNullOrEmpty(pan) && e.PANNumber == pan)
            );
        }
    }
}
"@
Set-Content -Path "$reposPath\EmployeeRepository.cs" -Value $content

# IEmployeeService
$content = @"
using School_DTOs;
using School_DTOs.Hr;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace School.Services.Interfaces
{
    public interface IEmployeeService
    {
        Task<APIResponse<EmployeeDto>> CreateEmployeeAsync(CreateEmployeeDto model, string username);
        Task<APIResponse<EmployeeDto>> UpdateEmployeeAsync(UpdateEmployeeDto model, string username);
        Task<APIResponse<EmployeeDto>> GetEmployeeByIdAsync(int id);
        Task<APIResponse<IEnumerable<EmployeeDto>>> GetAllEmployeesAsync();
        Task<APIResponse<bool>> DeleteEmployeeAsync(int id, string username);
        Task<APIResponse<bool>> ToggleEmployeeStatusAsync(int id, string username);
    }
}
"@
Set-Content -Path "$servicesPath\Interfaces\IEmployeeService.cs" -Value $content

# EmployeeService
$content = @"
using AutoMapper;
using Microsoft.Extensions.Logging;
using School.Domain.Hr;
using School.Infrastructure.Repositories.IRepositories;
using School.Infrastructure.UnitOfWork.Interfaces;
using School.Services.Interfaces;
using School_DTOs;
using School_DTOs.Hr;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace School.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<EmployeeService> _logger;

        public EmployeeService(
            IEmployeeRepository employeeRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<EmployeeService> logger)
        {
            _employeeRepository = employeeRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<APIResponse<EmployeeDto>> CreateEmployeeAsync(CreateEmployeeDto model, string username)
        {
            try
            {
                if (await _employeeRepository.IsDuplicateEmployeeAsync(model.Email, model.MobileNumber, model.AadhaarNumber, model.PANNumber))
                {
                    return new APIResponse<EmployeeDto> { Success = false, StatusCode = HttpStatusCode.Conflict, Message = `"Employee with same Email, Mobile, Aadhaar or PAN already exists.`" };
                }

                var entity = _mapper.Map<Employee>(model);
                entity.CreatedBy = username;
                
                // Auto generate Employee Code logic here
                entity.EmployeeCode = `"EMP`" + DateTime.Now.ToString(`"yyyyMMddHHmmss`");

                await _employeeRepository.AddAsync(entity);
                await _unitOfWork.CommitAsync();

                return new APIResponse<EmployeeDto> { Success = true, StatusCode = HttpStatusCode.Created, Data = _mapper.Map<EmployeeDto>(entity) };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, `"Error creating employee`");
                return new APIResponse<EmployeeDto> { Success = false, StatusCode = HttpStatusCode.InternalServerError, Message = `"An error occurred.`" };
            }
        }

        public async Task<APIResponse<EmployeeDto>> UpdateEmployeeAsync(UpdateEmployeeDto model, string username)
        {
            try
            {
                var existingEntity = await _employeeRepository.GetEmployeeWithDetailsAsync(model.Id);
                if (existingEntity == null)
                {
                    return new APIResponse<EmployeeDto> { Success = false, StatusCode = HttpStatusCode.NotFound, Message = `"Employee not found.`" };
                }

                if (await _employeeRepository.IsDuplicateEmployeeAsync(model.Email, model.MobileNumber, model.AadhaarNumber, model.PANNumber, model.Id))
                {
                    return new APIResponse<EmployeeDto> { Success = false, StatusCode = HttpStatusCode.Conflict, Message = `"Employee with same Email, Mobile, Aadhaar or PAN already exists.`" };
                }

                _mapper.Map(model, existingEntity);
                existingEntity.UpdatedBy = username;
                existingEntity.UpdatedDate = DateTime.Now;

                _employeeRepository.Update(existingEntity);
                await _unitOfWork.CommitAsync();

                return new APIResponse<EmployeeDto> { Success = true, StatusCode = HttpStatusCode.OK, Data = _mapper.Map<EmployeeDto>(existingEntity) };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, `"Error updating employee`");
                return new APIResponse<EmployeeDto> { Success = false, StatusCode = HttpStatusCode.InternalServerError, Message = `"An error occurred.`" };
            }
        }

        public async Task<APIResponse<EmployeeDto>> GetEmployeeByIdAsync(int id)
        {
            var entity = await _employeeRepository.GetEmployeeWithDetailsAsync(id);
            if (entity == null)
                return new APIResponse<EmployeeDto> { Success = false, StatusCode = HttpStatusCode.NotFound, Message = `"Employee not found.`" };

            return new APIResponse<EmployeeDto> { Success = true, StatusCode = HttpStatusCode.OK, Data = _mapper.Map<EmployeeDto>(entity) };
        }

        public async Task<APIResponse<IEnumerable<EmployeeDto>>> GetAllEmployeesAsync()
        {
            var entities = await _employeeRepository.GetAllEmployeesWithDetailsAsync();
            return new APIResponse<IEnumerable<EmployeeDto>> { Success = true, StatusCode = HttpStatusCode.OK, Data = _mapper.Map<IEnumerable<EmployeeDto>>(entities) };
        }

        public async Task<APIResponse<bool>> DeleteEmployeeAsync(int id, string username)
        {
            var entity = await _employeeRepository.GetByIdAsync(id);
            if (entity == null)
                return new APIResponse<bool> { Success = false, StatusCode = HttpStatusCode.NotFound, Message = `"Employee not found.`" };

            entity.IsDeleted = true;
            entity.UpdatedBy = username;
            entity.UpdatedDate = DateTime.Now;

            _employeeRepository.Update(entity);
            await _unitOfWork.CommitAsync();

            return new APIResponse<bool> { Success = true, StatusCode = HttpStatusCode.OK, Data = true, Message = `"Employee deleted successfully.`" };
        }

        public async Task<APIResponse<bool>> ToggleEmployeeStatusAsync(int id, string username)
        {
            var entity = await _employeeRepository.GetByIdAsync(id);
            if (entity == null)
                return new APIResponse<bool> { Success = false, StatusCode = HttpStatusCode.NotFound, Message = `"Employee not found.`" };

            entity.Status = entity.Status == `"active`" ? `"inactive`" : `"active`";
            entity.UpdatedBy = username;
            entity.UpdatedDate = DateTime.Now;

            _employeeRepository.Update(entity);
            await _unitOfWork.CommitAsync();

            return new APIResponse<bool> { Success = true, StatusCode = HttpStatusCode.OK, Data = true };
        }
    }
}
"@
Set-Content -Path "$servicesPath\EmployeeService.cs" -Value $content
