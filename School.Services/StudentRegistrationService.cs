using AutoMapper;
using School.Domain.Student;
using School.Infrastructure.Repositories.IRepositories;
using School.Models.Student;
using School.Services.Interfaces;
using School.Utilities.Resources;
using School_DTOs;
using School_DTOs.Student;
using System.Net;

namespace School.Services
{
    public class StudentRegistrationService : IStudentRegistrationService
    {
        private readonly IStudentRegistrationRepository _studentRegistrationRepository;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;

        public StudentRegistrationService(IStudentRegistrationRepository studentRegistrationRepository, IMapper mapper, IEmailService emailService)
        {
            _studentRegistrationRepository = studentRegistrationRepository;
            _mapper = mapper;
            _emailService = emailService;
        }

        public async Task<APIResponse<StudentRegistrationDto>> AddStudentRegistrationAsync(StudentRegistrationModel model)
        {
            var existsByMobile = await _studentRegistrationRepository.ExistsByMobileAsync(model.Mobile);
            if (existsByMobile)
            {
                return new APIResponse<StudentRegistrationDto>
                {
                    Success = false,
                    Message = string.Format(CommonResource.AlreadyExists, "Student Registration", $"Mobile: {model.Mobile}"),
                    StatusCode = HttpStatusCode.BadRequest,
                };
            }

            var existsByAadhaar = await _studentRegistrationRepository.ExistsByAadhaarAsync(model.AadhaarNumber);
            if (existsByAadhaar)
            {
                return new APIResponse<StudentRegistrationDto>
                {
                    Success = false,
                    Message = string.Format(CommonResource.AlreadyExists, "Student Registration", $"Aadhaar: {model.AadhaarNumber}"),
                    StatusCode = HttpStatusCode.BadRequest,
                };
            }

            var entity = _mapper.Map<StudentRegistration>(model);
            entity.DateOfBirth = model.DateOfBirth.ToString("yyyy-MM-dd");
            entity.RegistrationStatus = model.RegistrationStatus ?? "pending";
            entity.PaymentStatus = model.PaymentStatus ?? "pending";

            if (model.EducationalDetails != null && model.EducationalDetails.Any() && entity.EducationalDetails == null)
            {
                foreach (var eduDetail in model.EducationalDetails)
                {
                    if (!string.IsNullOrWhiteSpace(eduDetail.ExamName))
                    {
                        entity.EducationalDetails.Add(new EducationalDetail
                        {
                            ExamName = eduDetail.ExamName,
                            PassingYear = eduDetail.PassingYear,
                            InstituteName = eduDetail.InstituteName,
                            InstituteAddress = eduDetail.InstituteAddress,
                            TotalMarks = eduDetail.TotalMarks,
                            ObtainedMarks = eduDetail.ObtainedMarks,
                            Certificate = eduDetail.Certificate
                        });
                    }
                }
            }

            if (model.ExperienceCertificates != null && model.ExperienceCertificates.Any() && entity.ExperienceCertificates == null)
            {
                foreach (var expCert in model.ExperienceCertificates)
                {
                    if (!string.IsNullOrWhiteSpace(expCert.Experience) ||
                        !string.IsNullOrWhiteSpace(expCert.HospitalLabName) ||
                        expCert.FromDate.HasValue)
                    {
                        entity.ExperienceCertificates.Add(new StudentExperienceCertificate
                        {
                            Experience = expCert.Experience,
                            HospitalLabName = expCert.HospitalLabName,
                            FromDate = expCert.FromDate,
                            ToDate = expCert.ToDate,
                            TotalDuration = expCert.TotalDuration,
                            Certificate = expCert.Certificate,
                            CreatedDate = DateTime.Now
                        });
                    }
                }
            }

            entity = await _studentRegistrationRepository.AddStudentRegistrationAsync(entity);

            if (entity != null && entity.Id == 0)
            {
                return new APIResponse<StudentRegistrationDto>
                {
                    Success = false,
                    Data = _mapper.Map<StudentRegistrationDto>(entity),
                    Message = string.Format(CommonResource.AlreadyExists, typeof(StudentRegistration).Name, model.FullName),
                    StatusCode = HttpStatusCode.BadRequest,
                };
            }
            else if (entity != null && entity.Id > 0)
            {
                return new APIResponse<StudentRegistrationDto>
                {
                    Success = true,
                    Data = _mapper.Map<StudentRegistrationDto>(entity),
                    Message = CommonResource.AddSuccess,
                    StatusCode = HttpStatusCode.Created
                };
            }
            else
            {
                return new APIResponse<StudentRegistrationDto>
                {
                    Success = false,
                    Message = CommonResource.AddFailed,
                    StatusCode = HttpStatusCode.Forbidden
                };
            }
        }

        public async Task<APIResponse<StudentRegistrationDto>> GetStudentRegistrationByIdAsync(int id)
        {
            var result = await _studentRegistrationRepository.GetStudentRegistrationByIdAsync(id);

            if (result != null && result.Id > 0)
            {
                return new APIResponse<StudentRegistrationDto>
                {
                    Data = _mapper.Map<StudentRegistrationDto>(result),
                    Message = CommonResource.FetchSuccess,
                    Success = true,
                    StatusCode = HttpStatusCode.OK
                };
            }
            else
            {
                return new APIResponse<StudentRegistrationDto>
                {
                    Message = CommonResource.RecordNotFound,
                    StatusCode = HttpStatusCode.NotFound
                };
            }
        }

        public async Task<PagedResponse<StudentRegistrationDto>> GetAllAsync(int pageNumber = 1, int pageSize = 10, string? searchTerm = null, string? status = null)
        {
            var result = await _studentRegistrationRepository.GetAllAsync(pageNumber, pageSize, searchTerm, status);
            var totalCount = await _studentRegistrationRepository.GetTotalCountAsync(searchTerm, status);

            if (result != null && result.Any())
            {
                return new PagedResponse<StudentRegistrationDto>
                {
                    Data = _mapper.Map<IEnumerable<StudentRegistrationDto>>(result),
                    Message = CommonResource.FetchSuccess,
                    Success = true,
                    StatusCode = HttpStatusCode.OK,
                    TotalRecords = totalCount,
                    PageSize = pageSize,
                    CurrentPage = pageNumber,
                };
            }
            else
            {
                return new PagedResponse<StudentRegistrationDto>
                {
                    Message = CommonResource.RecordNotFound,
                    StatusCode = HttpStatusCode.OK,
                };
            }
        }

        public async Task<APIResponse> UpdateStudentRegistrationAsync(StudentRegistrationModel model)
        {
            if (model.Id > 0)
            {
                var existsByMobile = await _studentRegistrationRepository.ExistsByMobileAsync(model.Mobile, model.Id);
                if (existsByMobile)
                {
                    return new APIResponse
                    {
                        Success = false,
                        Message = string.Format(CommonResource.AlreadyExists, "Student Registration", $"Mobile: {model.Mobile}"),
                        StatusCode = HttpStatusCode.BadRequest,
                    };
                }

                var existsByAadhaar = await _studentRegistrationRepository.ExistsByAadhaarAsync(model.AadhaarNumber, model.Id);
                if (existsByAadhaar)
                {
                    return new APIResponse
                    {
                        Success = false,
                        Message = string.Format(CommonResource.AlreadyExists, "Student Registration", $"Aadhaar: {model.AadhaarNumber}"),
                        StatusCode = HttpStatusCode.BadRequest,
                    };
                }
            }

            var entity = _mapper.Map<StudentRegistration>(model);
            entity.DateOfBirth = model.DateOfBirth.ToString("yyyy-MM-dd");

            var result = await _studentRegistrationRepository.UpdateStudentRegistrationAsync(entity);
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
                    StatusCode = HttpStatusCode.BadRequest,
                };
            }
        }

        public async Task<APIResponse> DeleteStudentRegistrationAsync(int id)
        {
            int changes = await _studentRegistrationRepository.DeleteStudentRegistrationAsync(id);
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

        public async Task<APIResponse<StudentRegistrationDto>> GetByMobileAsync(string mobile)
        {
            var result = await _studentRegistrationRepository.GetByMobileAsync(mobile);

            if (result != null && result.Id > 0)
            {
                return new APIResponse<StudentRegistrationDto>
                {
                    Data = _mapper.Map<StudentRegistrationDto>(result),
                    Message = CommonResource.FetchSuccess,
                    Success = true,
                    StatusCode = HttpStatusCode.OK
                };
            }
            else
            {
                return new APIResponse<StudentRegistrationDto>
                {
                    Message = CommonResource.RecordNotFound,
                    StatusCode = HttpStatusCode.NotFound
                };
            }
        }

        public async Task<APIResponse<StudentRegistrationDto>> GetByAadhaarAsync(string aadhaarNumber)
        {
            var result = await _studentRegistrationRepository.GetByAadhaarAsync(aadhaarNumber);

            if (result != null && result.Id > 0)
            {
                return new APIResponse<StudentRegistrationDto>
                {
                    Data = _mapper.Map<StudentRegistrationDto>(result),
                    Message = CommonResource.FetchSuccess,
                    Success = true,
                    StatusCode = HttpStatusCode.OK
                };
            }
            else
            {
                return new APIResponse<StudentRegistrationDto>
                {
                    Message = CommonResource.RecordNotFound,
                    StatusCode = HttpStatusCode.NotFound
                };
            }
        }

        public async Task<APIResponse> UpdateStatusAsync(UpdateStudentRegistrationStatusDto dto)
        {
            var entity = await _studentRegistrationRepository.GetStudentRegistrationByIdAsync(dto.Id);

            if (entity == null || entity.Id == 0)
            {
                return new APIResponse
                {
                    Message = CommonResource.RecordNotFound,
                    StatusCode = HttpStatusCode.NotFound
                };
            }

            entity.RegistrationStatus = dto.RegistrationStatus;
            entity.Remarks = dto.Remarks;
            entity.UpdatedDate = DateTime.Now;

            var result = await _studentRegistrationRepository.UpdateStudentRegistrationAsync(entity);
            if (result > 0)
            {
                if (!string.IsNullOrEmpty(entity.Email))
                {
                    var statusLower = dto.RegistrationStatus.ToLower();
                    if (statusLower == "rejected" || statusLower == "reject")
                    {
                        var placeholders = new Dictionary<string, string>
                        {
                            { "StudentName", entity.FullName },
                            { "Reason", dto.Remarks ?? "Your application does not meet the criteria." },
                            { "EditLink", $"http://localhost:4200/student-registration/edit/{entity.Id}" }
                        };
                        await _emailService.SendGenericTemplateAsync(entity.Email, "RegistrationRejected", placeholders);
                    }
                }

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
                    StatusCode = HttpStatusCode.BadRequest,
                };
            }
        }
    }
}
