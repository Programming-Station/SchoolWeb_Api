using System.Net;
using AutoMapper;
using School.Domain.Email;
using School.Infrastructure.Repositories.IRepositories;
using School.Models.Email;
using School.Services.Interfaces.Email;
using School.Utilities.Resources;
using School_DTOs;
using School_DTOs.Email;

namespace School.Services.Email
{
    public class EmailBrandingService : IEmailBrandingService
    {
        private readonly IEmailBrandingRepository _repository;
        private readonly IMapper _mapper;

        public EmailBrandingService(IEmailBrandingRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<APIResponse<EmailBrandingDto>> GetByIdAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null)
                return new APIResponse<EmailBrandingDto>
                {
                    Success = false,
                    Message = CommonResource.RecordNotFound,
                    StatusCode = HttpStatusCode.NotFound
                };

            return new APIResponse<EmailBrandingDto>
            {
                Success = true,
                Data = _mapper.Map<EmailBrandingDto>(entity),
                Message = CommonResource.FetchSuccess,
                StatusCode = HttpStatusCode.OK
            };
        }

        public async Task<APIResponse<EmailBrandingDto>> GetBySchoolIdAsync(int schoolId)
        {
            var entity = await _repository.GetBySchoolIdAsync(schoolId);
            if (entity == null)
                return new APIResponse<EmailBrandingDto>
                {
                    Success = false,
                    Message = CommonResource.RecordNotFound,
                    StatusCode = HttpStatusCode.NotFound
                };

            return new APIResponse<EmailBrandingDto>
            {
                Success = true,
                Data = _mapper.Map<EmailBrandingDto>(entity),
                Message = CommonResource.FetchSuccess,
                StatusCode = HttpStatusCode.OK
            };
        }

        public async Task<APIResponse<EmailBrandingDto>> AddAsync(EmailBrandingModel model)
        {
            var entity = _mapper.Map<EmailBranding>(model);
            entity.CreatedDate = DateTime.UtcNow;

            entity = await _repository.AddAsync(entity);

            if (entity.Id == 0)
                return new APIResponse<EmailBrandingDto>
                {
                    Success = false,
                    Data = _mapper.Map<EmailBrandingDto>(entity),
                    Message = "Branding already exists for this school. Use Update instead.",
                    StatusCode = HttpStatusCode.Conflict
                };

            return new APIResponse<EmailBrandingDto>
            {
                Success = true,
                Data = _mapper.Map<EmailBrandingDto>(entity),
                Message = CommonResource.AddSuccess,
                StatusCode = HttpStatusCode.Created
            };
        }

        public async Task<APIResponse<EmailBrandingDto>> UpdateAsync(EmailBrandingModel model)
        {
            var entity = _mapper.Map<EmailBranding>(model);
            entity.UpdatedDate = DateTime.UtcNow;

            var result = await _repository.UpdateAsync(entity);
            return result > 0
                ? new APIResponse<EmailBrandingDto>
                {
                    Success = true,
                    Data = _mapper.Map<EmailBrandingDto>(entity),
                    Message = CommonResource.UpdateSuccess,
                    StatusCode = HttpStatusCode.OK
                }
                : new APIResponse<EmailBrandingDto>
                {
                    Success = false,
                    Message = CommonResource.UpdateFailed,
                    StatusCode = HttpStatusCode.InternalServerError
                };
        }
    }
}
