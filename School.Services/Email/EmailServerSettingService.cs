using System.Net;
using AutoMapper;
using School.Domain.Email;
using School.Infrastructure.Repositories.IRepositories;
using School.Models.Email;
using School.Services.Interfaces.Email;
using School.Utilities.Resources;
using School.Utilities.Security;
using School_DTOs;
using School_DTOs.Email;

namespace School.Services.Email
{
    public class EmailServerSettingService : IEmailServerSettingService
    {
        private readonly IEmailServerSettingRepository _repository;
        private readonly IMapper _mapper;
        private readonly IEncryptionService _encryptionService;

        public EmailServerSettingService(
            IEmailServerSettingRepository repository,
            IMapper mapper,
            IEncryptionService encryptionService)
        {
            _repository = repository;
            _mapper = mapper;
            _encryptionService = encryptionService;
        }

        public async Task<APIResponse<EmailServerSettingDto>> GetByIdAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null)
                return new APIResponse<EmailServerSettingDto>
                {
                    Success = false,
                    Message = CommonResource.RecordNotFound,
                    StatusCode = HttpStatusCode.NotFound
                };

            return new APIResponse<EmailServerSettingDto>
            {
                Success = true,
                Data = _mapper.Map<EmailServerSettingDto>(entity),
                Message = CommonResource.FetchSuccess,
                StatusCode = HttpStatusCode.OK
            };
        }

        public async Task<APIResponse<IEnumerable<EmailServerSettingDto>>> GetAllBySchoolIdAsync(int schoolId)
        {
            var list = await _repository.GetAllBySchoolIdAsync(schoolId);
            return new APIResponse<IEnumerable<EmailServerSettingDto>>
            {
                Success = true,
                Data = _mapper.Map<IEnumerable<EmailServerSettingDto>>(list),
                Message = CommonResource.FetchSuccess,
                StatusCode = HttpStatusCode.OK
            };
        }

        public async Task<APIResponse<EmailServerSettingDto>> AddAsync(EmailServerSettingModel model)
        {
            var entity = _mapper.Map<EmailServerSetting>(model);
            entity.CreatedDate = DateTime.UtcNow;

            // Encrypt password before persisting
            if (!string.IsNullOrEmpty(model.Password))
                entity.Password = _encryptionService.Encrypt(model.Password);

            entity = await _repository.AddAsync(entity);

            if (entity.Id == 0)
                return new APIResponse<EmailServerSettingDto>
                {
                    Success = false,
                    Data = _mapper.Map<EmailServerSettingDto>(entity),
                    Message = string.Format(CommonResource.AlreadyExists, "EmailServerSetting", model.FromEmail),
                    StatusCode = HttpStatusCode.Conflict
                };

            return new APIResponse<EmailServerSettingDto>
            {
                Success = true,
                Data = _mapper.Map<EmailServerSettingDto>(entity),
                Message = CommonResource.AddSuccess,
                StatusCode = HttpStatusCode.Created
            };
        }

        public async Task<APIResponse<EmailServerSettingDto>> UpdateAsync(EmailServerSettingModel model)
        {
            var entity = _mapper.Map<EmailServerSetting>(model);
            entity.UpdatedDate = DateTime.UtcNow;

            var result = await _repository.UpdateAsync(entity);
            return result > 0
                ? new APIResponse<EmailServerSettingDto>
                {
                    Success = true,
                    Data = _mapper.Map<EmailServerSettingDto>(entity),
                    Message = CommonResource.UpdateSuccess,
                    StatusCode = HttpStatusCode.OK
                }
                : new APIResponse<EmailServerSettingDto>
                {
                    Success = false,
                    Message = CommonResource.UpdateFailed,
                    StatusCode = HttpStatusCode.InternalServerError
                };
        }

        public async Task<APIResponse<bool>> UpdatePasswordAsync(int id, string newPassword, string updatedBy)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null)
                return new APIResponse<bool>
                {
                    Success = false,
                    Message = CommonResource.RecordNotFound,
                    StatusCode = HttpStatusCode.NotFound
                };

            existing.Password = _encryptionService.Encrypt(newPassword);
            existing.UpdatedBy = updatedBy;
            existing.UpdatedDate = DateTime.UtcNow;

            var result = await _repository.UpdateAsync(existing);
            return new APIResponse<bool>
            {
                Success = result > 0,
                Data = result > 0,
                Message = result > 0 ? CommonResource.UpdateSuccess : CommonResource.UpdateFailed,
                StatusCode = result > 0 ? HttpStatusCode.OK : HttpStatusCode.InternalServerError
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
