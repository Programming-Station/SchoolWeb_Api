using AutoMapper;
using School.Domain.Student;
using School.Domain;
using School.Infrastructure.Repositories.IRepositories;
using School.Models.Student;
using School.Services.Interfaces;
using School.Utilities.Resources;
using School.Utilities.Enums;
using School_DTOs;
using School_DTOs.Student;
using Microsoft.EntityFrameworkCore;
using System.Net;
using School.Infrastructure;

namespace School.Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IMapper _mapper;
        private readonly SchoolDbContext _context;

        public StudentService(IStudentRepository studentRepository, IMapper mapper, SchoolDbContext context)
        {
            _studentRepository = studentRepository;
            _mapper = mapper;
            _context = context;
        }

        public async Task<APIResponse<StudentDto>> CreateStudentAsync(StudentModel model)
        {
            try
            {
                if (model.ClassId.HasValue)
                {
                    var classExists = await _context.Classes
                        .AnyAsync(c => c.Id == model.ClassId.Value && !c.IsDeleted && c.Status.ToLower() == "active");

                    if (!classExists)
                    {
                        return new APIResponse<StudentDto>
                        {
                            Success = false,
                            Message = "Class not found or inactive",
                            StatusCode = HttpStatusCode.BadRequest
                        };
                    }
                }

                string studentId = model.StudentId;
                if (string.IsNullOrWhiteSpace(studentId))
                {
                    studentId = await _studentRepository.GenerateStudentIdAsync();
                    if (string.IsNullOrWhiteSpace(studentId))
                    {
                        return new APIResponse<StudentDto>
                        {
                            Success = false,
                            Message = "Failed to generate Student ID",
                            StatusCode = HttpStatusCode.InternalServerError
                        };
                    }
                }

                var entity = _mapper.Map<Student>(model);
                
                if (entity.CourseId <= 0)
                {
                    if (model.CourseType.HasValue)
                    {
                        var course = await _context.Courses
                            .FirstOrDefaultAsync(c => c.Name.ToLower().Contains(model.CourseType.Value.ToString().ToLower()) && !c.IsDeleted);
                        if (course != null)
                        {
                            entity.CourseId = course.Id;
                        }
                    }
                    if (entity.CourseId <= 0)
                    {
                        var defaultCourse = await _context.Courses
                            .FirstOrDefaultAsync(c => !c.IsDeleted);
                        if (defaultCourse != null)
                        {
                            entity.CourseId = defaultCourse.Id;
                        }
                        else
                        {
                            return new APIResponse<StudentDto>
                            {
                                Success = false,
                                Message = "No course available. Please add a course first.",
                                StatusCode = HttpStatusCode.BadRequest
                            };
                        }
                    }
                }

                entity.StudentId = studentId;
                entity.StatusId = (int)DefaultStatus.Created;
                entity.CreatedBy = model.CreatedBy;
                entity.CreatedDate = DateTime.UtcNow;

                entity = await _studentRepository.AddStudentAsync(entity);

                if (entity != null && entity.Id == 0)
                {
                    return new APIResponse<StudentDto>
                    {
                        Success = false,
                        Data = await MapToDtoAsync(entity),
                        Message = string.Format(CommonResource.AlreadyExists, "Student", $"Student ID: {studentId}"),
                        StatusCode = HttpStatusCode.BadRequest,
                    };
                }
                else if (entity != null && entity.Id > 0)
                {
                    var savedEntity = await _studentRepository.GetStudentByIdAsync(entity.Id);
                    var dto = await MapToDtoAsync(savedEntity);

                    return new APIResponse<StudentDto>
                    {
                        Success = true,
                        Data = dto,
                        Message = CommonResource.AddSuccess,
                        StatusCode = HttpStatusCode.Created
                    };
                }
                else
                {
                    return new APIResponse<StudentDto>
                    {
                        Success = false,
                        Message = CommonResource.AddFailed,
                        StatusCode = HttpStatusCode.Forbidden
                    };
                }
            }
            catch (Exception ex)
            {
                return new APIResponse<StudentDto>
                {
                    Success = false,
                    Message = string.Format(CommonResource.AddFailed, "Student", ex.Message),
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
        }

        public async Task<APIResponse<StudentDto>> GetStudentByIdAsync(int id)
        {
            try
            {
                var result = await _studentRepository.GetStudentByIdAsync(id);

                if (result != null && result.Id > 0)
                {
                    var dto = await MapToDtoAsync(result);
                    return new APIResponse<StudentDto>
                    {
                        Data = dto,
                        Message = CommonResource.FetchSuccess,
                        Success = true,
                        StatusCode = HttpStatusCode.OK
                    };
                }
                else
                {
                    return new APIResponse<StudentDto>
                    {
                        Message = CommonResource.RecordNotFound,
                        StatusCode = HttpStatusCode.NotFound
                    };
                }
            }
            catch (Exception ex)
            {
                return new APIResponse<StudentDto>
                {
                    Success = false,
                    Message = string.Format(CommonResource.FetchFailed, "Student", ex.Message),
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
        }

        public async Task<APIResponse<StudentDto>> GetStudentByStudentIdAsync(string studentId)
        {
            try
            {
                var result = await _studentRepository.GetStudentByStudentIdAsync(studentId);

                if (result != null && result.Id > 0)
                {
                    var dto = await MapToDtoAsync(result);
                    return new APIResponse<StudentDto>
                    {
                        Data = dto,
                        Message = CommonResource.FetchSuccess,
                        Success = true,
                        StatusCode = HttpStatusCode.OK
                    };
                }
                else
                {
                    return new APIResponse<StudentDto>
                    {
                        Message = CommonResource.RecordNotFound,
                        StatusCode = HttpStatusCode.NotFound
                    };
                }
            }
            catch (Exception ex)
            {
                return new APIResponse<StudentDto>
                {
                    Success = false,
                    Message = string.Format(CommonResource.FetchFailed, "Student", ex.Message),
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
        }

        public async Task<PagedResponse<StudentDto>> GetAllStudentsAsync(
            int pageNumber = 1,
            int pageSize = 10,
            string? searchTerm = null,
            string? status = null,
            string? classFilter = null)
        {
            try
            {
                int? statusId = null;
                if (!string.IsNullOrWhiteSpace(status))
                {
                    if (Enum.TryParse<DefaultStatus>(status, true, out var statusEnum))
                    {
                        statusId = (int)statusEnum;
                    }
                }

                var (students, totalCount) = await _studentRepository.GetAllAsync(pageNumber, pageSize, searchTerm, statusId, classFilter);

                var dtos = new List<StudentDto>();
                foreach (var student in students)
                {
                    dtos.Add(await MapToDtoAsync(student));
                }

                return new PagedResponse<StudentDto>
                {
                    Success = true,
                    Message = CommonResource.FetchSuccess,
                    StatusCode = HttpStatusCode.OK,
                    Data = dtos,
                    TotalRecords = totalCount,
                    CurrentPage = pageNumber,
                    PageSize = pageSize
                };
            }
            catch (Exception ex)
            {
                return new PagedResponse<StudentDto>
                {
                    Success = false,
                    Message = string.Format(CommonResource.FetchFailed, "Students", ex.Message),
                    StatusCode = HttpStatusCode.InternalServerError,
                    Data = new List<StudentDto>(),
                    TotalRecords = 0,
                    CurrentPage = pageNumber,
                    PageSize = pageSize
                };
            }
        }

        public async Task<APIResponse> UpdateStudentAsync(StudentModel model)
        {
            try
            {
                if (!model.Id.HasValue || model.Id.Value <= 0)
                {
                    return new APIResponse
                    {
                        Success = false,
                        Message = "Invalid Student ID",
                        StatusCode = HttpStatusCode.BadRequest,
                    };
                }

                var existingEntity = await _studentRepository.GetStudentByIdAsync(model.Id.Value);
                if (existingEntity == null || existingEntity.Id == 0)
                {
                    return new APIResponse
                    {
                        Message = CommonResource.RecordNotFound,
                        StatusCode = HttpStatusCode.NotFound
                    };
                }

                if (model.ClassId.HasValue)
                {
                    var classExists = await _context.Classes
                        .AnyAsync(c => c.Id == model.ClassId.Value && !c.IsDeleted && c.Status.ToLower() == "active");

                    if (!classExists)
                    {
                        return new APIResponse
                        {
                            Success = false,
                            Message = "Class not found or inactive",
                            StatusCode = HttpStatusCode.BadRequest
                        };
                    }
                }

                _mapper.Map(model, existingEntity);

                if (model.Status.HasValue)
                {
                    existingEntity.StatusId = (int)model.Status.Value;
                }

                if (model.CourseType.HasValue && existingEntity.CourseId <= 0)
                {
                    var course = await _context.Courses
                        .FirstOrDefaultAsync(c => c.Name.ToLower().Contains(model.CourseType.Value.ToString().ToLower()) && !c.IsDeleted);
                    if (course != null)
                    {
                        existingEntity.CourseId = course.Id;
                    }
                }

                existingEntity.UpdatedBy = model.UpdatedBy;
                existingEntity.UpdatedDate = DateTime.UtcNow;

                var result = await _studentRepository.UpdateStudentAsync(existingEntity);
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
            catch (Exception ex)
            {
                return new APIResponse
                {
                    Success = false,
                    Message = string.Format(CommonResource.UpdateFailed, "Student", ex.Message),
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
        }

        public async Task<APIResponse> DeleteStudentAsync(int id)
        {
            try
            {
                int changes = await _studentRepository.DeleteStudentAsync(id);
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
            catch (Exception ex)
            {
                return new APIResponse
                {
                    Success = false,
                    Message = string.Format(CommonResource.DeleteFailed, "Student", ex.Message),
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
        }

        public async Task<APIResponse<string>> GenerateStudentIdAsync()
        {
            try
            {
                string studentId = await _studentRepository.GenerateStudentIdAsync();
                if (string.IsNullOrWhiteSpace(studentId))
                {
                    return new APIResponse<string>
                    {
                        Success = false,
                        Message = "Failed to generate Student ID",
                        StatusCode = HttpStatusCode.InternalServerError,
                        Data = ""
                    };
                }

                return new APIResponse<string>
                {
                    Success = true,
                    Message = "Student ID generated successfully",
                    StatusCode = HttpStatusCode.OK,
                    Data = studentId
                };
            }
            catch (Exception ex)
            {
                return new APIResponse<string>
                {
                    Success = false,
                    Message = $"Failed to generate Student ID: {ex.Message}",
                    StatusCode = HttpStatusCode.InternalServerError,
                    Data = ""
                };
            }
        }

        private async Task<StudentDto> MapToDtoAsync(Student entity)
        {
            var dto = _mapper.Map<StudentDto>(entity);

            dto.ClassName = entity.Class?.Name;
            dto.Section = entity.Class?.Section;
            dto.Status = entity.Status?.Name ?? ((DefaultStatus)entity.StatusId).ToString();

            dto.StdId = entity.StudentId;
            dto.Gender = entity.Gender;
            dto.Class = entity.Class?.Name;
            dto.MobileNo = entity.MobileNo1;

            if (!string.IsNullOrEmpty(entity.DateOfBirth))
            {
                if (DateTime.TryParse(entity.DateOfBirth, out var dateOfBirth))
                {
                    dto.BirthDate = dateOfBirth.Day;
                    dto.BirthMonth = dateOfBirth.Month;
                    dto.BirthYear = dateOfBirth.Year;
                }
            }

            dto.TotalFee = 0; // Will be calculated from Fee table in future
            dto.DueFee = 0;   // Will be calculated from Fee table in future

            dto.SchoolCourse = entity.Course?.Name;
            dto.UniversityCourse = entity.Course?.Name; // Adjust based on business logic

            return dto;
        }
    }
}
