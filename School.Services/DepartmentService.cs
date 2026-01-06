using AutoMapper;
using School.Domain;
using School.Infrastructure.Repositories.IRepositories;
using School.Models.Department;
using School.Services.Interfaces;
using School.Utilities.Resources;
using School_DTOs;
using School_DTOs.Department;
using System.Net;

namespace School.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IMapper _mapper;

        public DepartmentService(IDepartmentRepository departmentRepository, IMapper mapper)
        {
            _departmentRepository = departmentRepository;
            _mapper = mapper;
        }

        public async Task<APIResponse<DepartmentDto>> AddDepartmentAsync(DepartmentModel model)
        {
            var entity = _mapper.Map<Department>(model);
            entity = await _departmentRepository.AddDepartmentAsync(entity);
            
            if (entity != null && entity.Id == 0)
            {
                return new APIResponse<DepartmentDto>
                {
                    Success = false,
                    Data = _mapper.Map<DepartmentDto>(entity),
                    Message = string.Format(CommonResource.AlreadyExists, "Department", model.Name),
                    StatusCode = HttpStatusCode.BadRequest,
                };
            }
            else if (entity != null && entity.Id > 0)
            { 
                return new APIResponse<DepartmentDto>
                {
                    Success = true,
                    Data = _mapper.Map<DepartmentDto>(entity),
                    Message = CommonResource.AddSuccess,
                    StatusCode = HttpStatusCode.Created
                };
            }
            else
            {
                return new APIResponse<DepartmentDto>
                {
                    Success = false,
                    Message = CommonResource.AddFailed,
                    StatusCode = HttpStatusCode.Forbidden
                };
            }
        }

        public async Task<APIResponse<DepartmentDto>> GetDepartmentByIdAsync(int id)
        {
            var result = await _departmentRepository.GetDepartmentByIdAsync(id);

            if (result != null && result.Id > 0)
            { 
                return new APIResponse<DepartmentDto>
                {
                    Data = _mapper.Map<DepartmentDto>(result),
                    Message = CommonResource.FetchSuccess,
                    Success = true,
                    StatusCode = HttpStatusCode.OK
                };
            }
            else
            {
                return new APIResponse<DepartmentDto>
                {
                    Message = CommonResource.RecordNotFound,
                    StatusCode = HttpStatusCode.OK
                };
            }
        }

        public async Task<APIResponse<IEnumerable<DepartmentDto>>> GetAllDepartmentsAsync()
        {
            var result = await _departmentRepository.GetAllDepartmentsAsync();
            
            if (result != null && result.Any())
            { 

                return new APIResponse<IEnumerable<DepartmentDto>>
                {
                    Data = _mapper.Map<IEnumerable<DepartmentDto>>(result),
                    Message = CommonResource.FetchSuccess,
                    Success = true,
                    StatusCode = HttpStatusCode.OK,
                };
            }
            else
            {
                return new APIResponse<IEnumerable<DepartmentDto>>
                {
                    Message = CommonResource.RecordNotFound,
                    StatusCode = HttpStatusCode.OK,
                };
            }
        }

        public async Task<APIResponse<IEnumerable<DepartmentDto>>> GetDepartmentsByFacultyIdAsync(int facultyId)
        {
            var result = await _departmentRepository.GetDepartmentsByFacultyIdAsync(facultyId);
            
            if (result != null && result.Any())
            { 

                return new APIResponse<IEnumerable<DepartmentDto>>
                {
                    Data = _mapper.Map<IEnumerable<DepartmentDto>>(result),
                    Message = CommonResource.FetchSuccess,
                    Success = true,
                    StatusCode = HttpStatusCode.OK,
                };
            }
            else
            {
                return new APIResponse<IEnumerable<DepartmentDto>>
                {
                    Data = new List<DepartmentDto>(),
                    Message = CommonResource.RecordNotFound,
                    StatusCode = HttpStatusCode.OK,
                };
            }
        }

        public async Task<APIResponse> UpdateDepartmentAsync(DepartmentModel model)
        {
            var entity = _mapper.Map<Department>(model);
            var result = await _departmentRepository.UpdateDepartmentAsync(entity);
            
            if (result > 0)
            {
                return new APIResponse
                {
                    StatusCode = HttpStatusCode.OK,
                    Success = true,
                    Message = CommonResource.UpdateSuccess
                };
            }
            else
            {
                return new APIResponse
                {
                    Message = CommonResource.UpdateFailed,
                    StatusCode = HttpStatusCode.OK,
                };
            }
        }

        public async Task<APIResponse> DeleteDepartmentAsync(int id)
        {
            int changes = await _departmentRepository.DeleteDepartmentAsync(id);
            if (changes > 0)
                return new APIResponse
                {
                    Success = true,
                    Message = CommonResource.DeleteSuccess,
                    StatusCode = HttpStatusCode.OK,
                };
            else
                return new APIResponse
                {
                    Message = CommonResource.DeleteFailed,
                    StatusCode = HttpStatusCode.BadRequest,
                };
        }

        public async Task<APIResponse> ToggleDepartmentStatusAsync(int id)
        {
            int changes = await _departmentRepository.ToggleDepartmentStatusAsync(id);
            if (changes > 0)
                return new APIResponse
                {
                    Success = true,
                    Message = CommonResource.UpdateSuccess,
                    StatusCode = HttpStatusCode.OK,
                };
            else
                return new APIResponse
                {
                    Message = CommonResource.UpdateFailed,
                    StatusCode = HttpStatusCode.BadRequest,
                };
        } 
    }
}

