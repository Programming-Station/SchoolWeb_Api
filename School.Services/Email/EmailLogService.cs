using AutoMapper;
using School.Infrastructure.Repositories.IRepositories;
using School.Services.Interfaces.Email;
using School.Utilities.Resources;
using School_DTOs;
using School_DTOs.Email;
using System.Net;

namespace School.Services.Email
{
    public class EmailLogService : IEmailLogService
    {
        private readonly IEmailLogRepository _repository;
        private readonly IMapper _mapper;

        public EmailLogService(IEmailLogRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<APIResponse<EmailLogDto>> GetByIdAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null)
                return new APIResponse<EmailLogDto>
                {
                    Success = false,
                    Message = CommonResource.RecordNotFound,
                    StatusCode = HttpStatusCode.NotFound
                };

            return new APIResponse<EmailLogDto>
            {
                Success = true,
                Data = _mapper.Map<EmailLogDto>(entity),
                Message = CommonResource.FetchSuccess,
                StatusCode = HttpStatusCode.OK
            };
        }

        public async Task<APIResponse<object>> GetAllBySchoolIdAsync(int schoolId, int page = 1, int pageSize = 20)
        {
            var list = await _repository.GetAllBySchoolIdAsync(schoolId, page, pageSize);
            var total = await _repository.GetTotalCountAsync(schoolId);

            return new APIResponse<object>
            {
                Success = true,
                Data = new
                {
                    Items = _mapper.Map<IEnumerable<EmailLogDto>>(list),
                    TotalCount = total,
                    Page = page,
                    PageSize = pageSize,
                    TotalPages = (int)Math.Ceiling((double)total / pageSize)
                },
                Message = CommonResource.FetchSuccess,
                StatusCode = HttpStatusCode.OK
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

        public async Task<APIResponse<bool>> DeleteOldLogsAsync(int schoolId, int daysOlderThan = 90)
        {
            var result = await _repository.DeleteOldLogsAsync(schoolId, daysOlderThan);
            return new APIResponse<bool>
            {
                Success = true,
                Data = result > 0,
                Message = $"{result} old email log(s) older than {daysOlderThan} days deleted successfully.",
                StatusCode = HttpStatusCode.OK
            };
        }
    }
}
