using AutoMapper;
using School.Domain;
using School.Infrastructure.Repositories.IRepositories;
using School.Models.Faculty;
using School.Services.Interfaces;
using School.Utilities.Resources;
using School_DTOs;
using School_DTOs.Faculty;
using System.Net;

namespace School.Services
{
    public class FacultyService : IFacultyService
    {
        private readonly IFacultyRepository _facultyRepository;
        private readonly IMapper _mapper;

        public FacultyService(IFacultyRepository facultyRepository, IMapper mapper)
        {
            _facultyRepository = facultyRepository;
            _mapper = mapper;
        }

        public async Task<APIResponse<FacultyDto>> AddFacultyAsync(FacultyModel model)
        {
            var entity = _mapper.Map<Faculty>(model);
            entity = await _facultyRepository.AddFacultyAsync(entity);
            
            if (entity != null && entity.Id == 0)
            {
                return new APIResponse<FacultyDto>
                {
                    Success = false,
                    Data = _mapper.Map<FacultyDto>(entity),
                    Message = string.Format(CommonResource.AlreadyExists, "Faculty", model.Name),
                    StatusCode = HttpStatusCode.BadRequest,
                };
            }
            else if (entity != null && entity.Id > 0)
            {
                return new APIResponse<FacultyDto>
                {
                    Success = true,
                    Data = _mapper.Map<FacultyDto>(entity),
                    Message = CommonResource.AddSuccess,
                    StatusCode = HttpStatusCode.Created
                };
            }
            else
            {
                return new APIResponse<FacultyDto>
                {
                    Success = false,
                    Message = CommonResource.AddFailed,
                    StatusCode = HttpStatusCode.Forbidden
                };
            }
        }

        public async Task<APIResponse<FacultyDto>> GetFacultyByIdAsync(int id)
        {
            var result = await _facultyRepository.GetFacultyByIdAsync(id);

            if (result != null && result.Id > 0)
            {
                return new APIResponse<FacultyDto>
                {
                    Data = _mapper.Map<FacultyDto>(result),
                    Message = CommonResource.FetchSuccess,
                    Success = true,
                    StatusCode = HttpStatusCode.OK
                };
            }
            else
            {
                return new APIResponse<FacultyDto>
                {
                    Message = CommonResource.RecordNotFound,
                    StatusCode = HttpStatusCode.OK
                };
            }
        }

        public async Task<APIResponse<IEnumerable<FacultyDto>>> GetAllFacultiesAsync()
        {
            var result = await _facultyRepository.GetAllFacultiesAsync();
            
            if (result != null && result.Any())
            {
                return new APIResponse<IEnumerable<FacultyDto>>
                {
                    Data = _mapper.Map<IEnumerable<FacultyDto>>(result),
                    Message = CommonResource.FetchSuccess,
                    Success = true,
                    StatusCode = HttpStatusCode.OK,
                };
            }
            else
            {
                return new APIResponse<IEnumerable<FacultyDto>>
                {
                    Message = CommonResource.RecordNotFound,
                    StatusCode = HttpStatusCode.OK,
                };
            }
        }

        public async Task<APIResponse> UpdateFacultyAsync(FacultyModel model)
        {
            var entity = _mapper.Map<Faculty>(model);
            var result = await _facultyRepository.UpdateFacultyAsync(entity);
            
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

        public async Task<APIResponse> DeleteFacultyAsync(int id)
        {
            int changes = await _facultyRepository.DeleteFacultyAsync(id);
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

        public async Task<APIResponse> ToggleFacultyStatusAsync(int id)
        {
            int changes = await _facultyRepository.ToggleFacultyStatusAsync(id);
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

