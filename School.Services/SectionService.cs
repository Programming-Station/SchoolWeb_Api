using System.Net;
using AutoMapper;
using School.Domain;
using School.Infrastructure.Repositories.IRepositories;
using School.Models.Class;
using School.Services.Interfaces;
using School.Utilities.Resources;
using School_DTOs;
using School_DTOs.Class;

namespace School.Services
{
    public class SectionService : ISectionService
    {
        private readonly ISectionRepository _sectionRepository;
        private readonly IMapper _mapper;

        public SectionService(ISectionRepository sectionRepository, IMapper mapper)
        {
            _sectionRepository = sectionRepository;
            _mapper = mapper;
        }

        public async Task<APIResponse<SectionDto>> AddSectionAsync(SectionModel model)
        {
            var entity = _mapper.Map<Section>(model);
            entity = await _sectionRepository.AddSectionAsync(entity);
            if (entity != null && entity.Id > 0)
            {
                return new APIResponse<SectionDto>
                {
                    Success = true,
                    Data = _mapper.Map<SectionDto>(entity),
                    Message = CommonResource.AddSuccess,
                    StatusCode = HttpStatusCode.Created
                };
            }
            return new APIResponse<SectionDto>
            {
                Success = false,
                Message = CommonResource.AddFailed,
                StatusCode = HttpStatusCode.Forbidden
            };
        }

        public async Task<APIResponse<SectionDto>> GetSectionByIdAsync(int id)
        {
            var result = await _sectionRepository.GetSectionByIdAsync(id);
            if (result != null && result.Id > 0)
            {
                return new APIResponse<SectionDto>
                {
                    Data = _mapper.Map<SectionDto>(result),
                    Message = CommonResource.FetchSuccess,
                    Success = true,
                    StatusCode = HttpStatusCode.OK
                };
            }
            return new APIResponse<SectionDto>
            {
                Message = CommonResource.RecordNotFound,
                StatusCode = HttpStatusCode.OK
            };
        }

        public async Task<APIResponse<IEnumerable<SectionDto>>> GetAllSectionsAsync()
        {
            var result = await _sectionRepository.GetAllSectionsAsync();
            if (result != null && result.Any())
            {
                return new APIResponse<IEnumerable<SectionDto>>
                {
                    Data = _mapper.Map<IEnumerable<SectionDto>>(result),
                    Message = CommonResource.FetchSuccess,
                    Success = true,
                    StatusCode = HttpStatusCode.OK
                };
            }
            return new APIResponse<IEnumerable<SectionDto>>
            {
                Message = CommonResource.RecordNotFound,
                StatusCode = HttpStatusCode.OK
            };
        }

        public async Task<APIResponse<IEnumerable<SectionDto>>> GetSectionsByClassIdAsync(int classId)
        {
            var result = await _sectionRepository.GetSectionsByClassIdAsync(classId);
            if (result != null && result.Any())
            {
                return new APIResponse<IEnumerable<SectionDto>>
                {
                    Data = _mapper.Map<IEnumerable<SectionDto>>(result),
                    Message = CommonResource.FetchSuccess,
                    Success = true,
                    StatusCode = HttpStatusCode.OK
                };
            }
            return new APIResponse<IEnumerable<SectionDto>>
            {
                Message = CommonResource.RecordNotFound,
                StatusCode = HttpStatusCode.OK
            };
        }

        public async Task<APIResponse> UpdateSectionAsync(SectionModel model)
        {
            var entity = _mapper.Map<Section>(model);
            var result = await _sectionRepository.UpdateSectionAsync(entity);
            if (result > 0)
            {
                return new APIResponse
                {
                    Success = true,
                    Message = CommonResource.UpdateSuccess,
                    StatusCode = HttpStatusCode.OK
                };
            }
            return new APIResponse
            {
                Success = false,
                Message = CommonResource.UpdateFailed,
                StatusCode = HttpStatusCode.Forbidden
            };
        }

        public async Task<APIResponse> DeleteSectionAsync(int id)
        {
            var result = await _sectionRepository.DeleteSectionAsync(id);
            if (result > 0)
            {
                return new APIResponse
                {
                    Success = true,
                    Message = CommonResource.DeleteSuccess,
                    StatusCode = HttpStatusCode.OK
                };
            }
            return new APIResponse
            {
                Success = false,
                Message = CommonResource.DeleteFailed,
                StatusCode = HttpStatusCode.Forbidden
            };
        }

        public async Task<APIResponse> ToggleSectionStatusAsync(int id)
        {
            var result = await _sectionRepository.ToggleSectionStatusAsync(id);
            if (result > 0)
            {
                return new APIResponse
                {
                    Success = true,
                    Message = CommonResource.UpdateSuccess,
                    StatusCode = HttpStatusCode.OK
                };
            }
            return new APIResponse
            {
                Success = false,
                Message = CommonResource.UpdateFailed,
                StatusCode = HttpStatusCode.Forbidden
            };
        }
    }
}
