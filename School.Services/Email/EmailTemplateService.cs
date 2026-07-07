using AutoMapper;
using School.Domain.Email;
using School.Infrastructure.Repositories.IRepositories;
using School.Models.Email;
using School.Services.Interfaces.Email;
using School.Utilities.Resources;
using School_DTOs;
using School_DTOs.Email;
using System.Net;

namespace School.Services.Email
{
    public class EmailTemplateService : IEmailTemplateService
    {
        private readonly IEmailTemplateRepository _repository;
        private readonly IMapper _mapper;

        public EmailTemplateService(IEmailTemplateRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<APIResponse<EmailTemplateDto>> GetByIdAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null)
                return new APIResponse<EmailTemplateDto>
                {
                    Success = false,
                    Message = CommonResource.RecordNotFound,
                    StatusCode = HttpStatusCode.NotFound
                };

            return new APIResponse<EmailTemplateDto>
            {
                Success = true,
                Data = _mapper.Map<EmailTemplateDto>(entity),
                Message = CommonResource.FetchSuccess,
                StatusCode = HttpStatusCode.OK
            };
        }

        public async Task<APIResponse<IEnumerable<EmailTemplateDto>>> GetAllBySchoolIdAsync(int schoolId)
        {
            var list = await _repository.GetAllBySchoolIdAsync(schoolId);
            return new APIResponse<IEnumerable<EmailTemplateDto>>
            {
                Success = true,
                Data = _mapper.Map<IEnumerable<EmailTemplateDto>>(list),
                Message = CommonResource.FetchSuccess,
                StatusCode = HttpStatusCode.OK
            };
        }

        public async Task<APIResponse<EmailTemplateDto>> AddAsync(EmailTemplateModel model)
        {
            var entity = _mapper.Map<EmailTemplate>(model);
            entity.CreatedDate = DateTime.UtcNow;

            entity = await _repository.AddAsync(entity);

            if (entity.Id == 0)
                return new APIResponse<EmailTemplateDto>
                {
                    Success = false,
                    Data = _mapper.Map<EmailTemplateDto>(entity),
                    Message = string.Format(CommonResource.AlreadyExists, "EmailTemplate", model.TemplateName),
                    StatusCode = HttpStatusCode.Conflict
                };

            return new APIResponse<EmailTemplateDto>
            {
                Success = true,
                Data = _mapper.Map<EmailTemplateDto>(entity),
                Message = CommonResource.AddSuccess,
                StatusCode = HttpStatusCode.Created
            };
        }

        public async Task<APIResponse<EmailTemplateDto>> UpdateAsync(EmailTemplateModel model)
        {
            var entity = _mapper.Map<EmailTemplate>(model);
            entity.UpdatedDate = DateTime.UtcNow;

            var result = await _repository.UpdateAsync(entity);
            return result > 0
                ? new APIResponse<EmailTemplateDto>
                {
                    Success = true,
                    Data = _mapper.Map<EmailTemplateDto>(entity),
                    Message = CommonResource.UpdateSuccess,
                    StatusCode = HttpStatusCode.OK
                }
                : new APIResponse<EmailTemplateDto>
                {
                    Success = false,
                    Message = CommonResource.UpdateFailed,
                    StatusCode = HttpStatusCode.InternalServerError
                };
        }

        public async Task<APIResponse<bool>> DeleteAsync(int id)
        {
            var result = await _repository.DeleteAsync(id);
            return new APIResponse<bool>
            {
                Success = result > 0,
                Data = result > 0,
                Message = result > 0 ? CommonResource.DeleteSuccess : CommonResource.RecordNotFound,
                StatusCode = result > 0 ? HttpStatusCode.OK : HttpStatusCode.NotFound
            };
        }

        public async Task<APIResponse<bool>> ToggleStatusAsync(int id)
        {
            var result = await _repository.ToggleStatusAsync(id);
            return new APIResponse<bool>
            {
                Success = result > 0,
                Data = result > 0,
                Message = result > 0 ? "Status updated successfully." : CommonResource.RecordNotFound,
                StatusCode = result > 0 ? HttpStatusCode.OK : HttpStatusCode.NotFound
            };
        }
    }
}
