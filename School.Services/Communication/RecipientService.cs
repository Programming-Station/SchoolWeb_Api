using System.Net;
using AutoMapper;
using Microsoft.Extensions.Logging;
using School.Domain.Communication.Recipients;
using School.Infrastructure.Repositories.IRepositories;
using School.Infrastructure.UnitOfWork.Interfaces;
using School.Services.Interfaces;
using School_DTOs;
using School_DTOs.Communication.Recipients;

namespace School.Services.Communication
{
    public class RecipientService : IRecipientService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRecipientRepository _recipientRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<RecipientService> _logger;

        public RecipientService(
            IUnitOfWork unitOfWork,
            IRecipientRepository recipientRepository,
            IMapper mapper,
            ILogger<RecipientService> logger)
        {
            _unitOfWork = unitOfWork;
            _recipientRepository = recipientRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<APIResponse<IEnumerable<RecipientDto>>> GetAllRecipientsAsync()
        {
            try
            {
                var tenantId = 1; // Real implementation will get from token/context
                var recipients = await _recipientRepository.GetRecipientsByTenantAsync(tenantId);
                var dtos = _mapper.Map<IEnumerable<RecipientDto>>(recipients);
                return new APIResponse<IEnumerable<RecipientDto>>
                {
                    Success = true,
                    Data = dtos,
                    Message = "Recipients fetched successfully",
                    StatusCode = HttpStatusCode.OK
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching recipients");
                return new APIResponse<IEnumerable<RecipientDto>>
                {
                    Success = false,
                    Message = "Failed to fetch recipients",
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
        }

        public async Task<APIResponse<RecipientDto>> GetRecipientByIdAsync(int id)
        {
            try
            {
                var recipient = await _recipientRepository.FindAsync(r => r.Id == id);
                if (recipient == null)
                {
                    return new APIResponse<RecipientDto>
                    {
                        Success = false,
                        Message = "Recipient not found",
                        StatusCode = HttpStatusCode.NotFound
                    };
                }

                var dto = _mapper.Map<RecipientDto>(recipient);
                return new APIResponse<RecipientDto>
                {
                    Success = true,
                    Data = dto,
                    Message = "Recipient fetched successfully",
                    StatusCode = HttpStatusCode.OK
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching recipient {Id}", id);
                return new APIResponse<RecipientDto>
                {
                    Success = false,
                    Message = "Failed to fetch recipient",
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
        }

        public async Task<APIResponse<RecipientDto>> CreateRecipientAsync(RecipientCreateDto dto)
        {
            try
            {
                var recipient = new Recipient
                {
                    RecipientType = dto.RecipientType,
                    FullName = dto.FullName,
                    DisplayName = dto.DisplayName,
                    Gender = dto.Gender,
                    DateOfBirth = dto.DateOfBirth,
                    Email = dto.Email,
                    Mobile = dto.Mobile,
                    WhatsAppNumber = dto.WhatsAppNumber,
                    PreferredChannel = dto.PreferredChannel,
                    RecipientCode = dto.RecipientCode,
                    SchoolRegistrationId = 1 // from tenant context
                };

                await _recipientRepository.AddAsync(recipient);
                await _unitOfWork.CommitAsync();

                var createdDto = _mapper.Map<RecipientDto>(recipient);
                return new APIResponse<RecipientDto>
                {
                    Success = true,
                    Data = createdDto,
                    Message = "Recipient created successfully",
                    StatusCode = HttpStatusCode.Created
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating recipient");
                return new APIResponse<RecipientDto>
                {
                    Success = false,
                    Message = "Failed to create recipient",
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
        }

        public async Task<APIResponse<bool>> DeleteRecipientAsync(int id)
        {
            try
            {
                var recipient = await _recipientRepository.FindAsync(r => r.Id == id);
                if (recipient == null)
                {
                    return new APIResponse<bool>
                    {
                        Success = false,
                        Message = "Recipient not found",
                        StatusCode = HttpStatusCode.NotFound
                    };
                }

                recipient.IsDeleted = true;
                _recipientRepository.Update(recipient);
                await _unitOfWork.CommitAsync();

                return new APIResponse<bool>
                {
                    Success = true,
                    Data = true,
                    Message = "Recipient deleted successfully",
                    StatusCode = HttpStatusCode.OK
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting recipient {Id}", id);
                return new APIResponse<bool>
                {
                    Success = false,
                    Message = "Failed to delete recipient",
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
        }

        public async Task<APIResponse<IEnumerable<RecipientGroupDto>>> GetAllGroupsAsync()
        {
            return new APIResponse<IEnumerable<RecipientGroupDto>>
            {
                Success = true,
                Data = new List<RecipientGroupDto>(),
                Message = "Groups fetched successfully",
                StatusCode = HttpStatusCode.OK
            };
        }
    }
}
