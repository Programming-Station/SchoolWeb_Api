using School.Domain.Student;
using School.Infrastructure.Repositories.IRepositories;
using School.Infrastructure.UnitOfWork;
using School.Infrastructure.UnitOfWork.Interfaces;
using School.Models.Student;
using School.Utilities.Enums;
using School_DTOs;
using School_DTOs.Student;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace School.Infrastructure.Repositories
{
    public class StudentRepository : Repository<Student>, IStudentRepository
    {
        private readonly SchoolDbContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public StudentRepository(DbFactory dbFactory, SchoolDbContext context, IUnitOfWork unitOfWork) : base(dbFactory)
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }

        public async Task<APIResponse<StudentDto>> AddStudentAsync(StudentModel model)
        {
            try
            {
                // Generate Student ID if not provided
                string studentId = model.StudentId == null ? await GenerateStudentIdAsync() : model.StudentId;

                // Verify class exists if ClassId is provided
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

                var studentEntity = new Student
                {
                    StudentId = studentId,
                    EnrollmentNumber = model.EnrollmentNumber,
                    CourseType = model.CourseType.ToString(),
                    CourseOpted = model.CourseOpted,
                    Name = model.Name,
                    FathersName = model.FathersName,
                    Gender = model.Sex,
                    Nationality = model.Nationality,
                    Occupation = model.Occupation,
                    DateOfBirth = model.DateOfBirth,
                    SchoolCollege = model.SchoolCollege,
                    QualificationDetails = model.QualificationDetails,
                    PostalAddress = model.PostalAddress,
                    PinCode = model.PinCode,
                    MobileNo1 = model.MobileNo1,
                    MobileNo2 = model.MobileNo2,
                    Image = model.Image,
                    ClassId = model.ClassId,
                    StatusId = (int)DefaultStatus.Created,
                    Remarks = model.Remarks,
                    CreatedBy = model.CreatedBy,
                    CreatedDate = DateTime.UtcNow
                };

                Add(studentEntity);
                await _unitOfWork.CommitAsync();

                var dto = await MapToDtoAsync(studentEntity);
                return new APIResponse<StudentDto>
                {
                    Success = true,
                    Message = "Student added successfully",
                    StatusCode = HttpStatusCode.Created,
                    Data = dto
                };
            }
            catch (Exception ex)
            {
                return new APIResponse<StudentDto>
                {
                    Success = false,
                    Message = $"Failed to add student: {ex.Message}",
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
        }

        public async Task<APIResponse<StudentDto>> GetStudentByIdAsync(int id)
        {
            try
            {
                var studentEntity = await _context.Students
                    .Include(s => s.Class)
                    .ThenInclude(c => c!.Course)
                    .FirstOrDefaultAsync(s => s.Id == id && !s.IsDeleted);

                if (studentEntity == null)
                {
                    return new APIResponse<StudentDto>
                    {
                        Success = false,
                        Message = "Student not found",
                        StatusCode = HttpStatusCode.NotFound
                    };
                }

                var dto = await MapToDtoAsync(studentEntity);
                return new APIResponse<StudentDto>
                {
                    Success = true,
                    Message = "Student fetched successfully",
                    StatusCode = HttpStatusCode.OK,
                    Data = dto
                };
            }
            catch (Exception ex)
            {
                return new APIResponse<StudentDto>
                {
                    Success = false,
                    Message = $"Failed to get student: {ex.Message}",
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
        }

        public async Task<APIResponse<StudentDto>> GetStudentByStudentIdAsync(string studentId)
        {
            try
            {
                var studentEntity = await _context.Students
                    .Include(s => s.Class)
                    .ThenInclude(c => c!.Course)
                    .FirstOrDefaultAsync(s => s.StudentId == studentId && !s.IsDeleted);

                if (studentEntity == null)
                {
                    return new APIResponse<StudentDto>
                    {
                        Success = false,
                        Message = "Student not found",
                        StatusCode = HttpStatusCode.NotFound
                    };
                }

                var dto = await MapToDtoAsync(studentEntity);
                return new APIResponse<StudentDto>
                {
                    Success = true,
                    Message = "Student fetched successfully",
                    StatusCode = HttpStatusCode.OK,
                    Data = dto
                };
            }
            catch (Exception ex)
            {
                return new APIResponse<StudentDto>
                {
                    Success = false,
                    Message = $"Failed to get student: {ex.Message}",
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
        }

        public async Task<APIResponse<IEnumerable<StudentDto>>> GetAllStudentsAsync(int pageNumber = 1, int pageSize = 10, string? searchTerm = null, string? status = null)
        {
            try
            {
                var query = _context.Students
                    .Where(s => !s.IsDeleted)
                    .Include(s => s.Class)
                    .ThenInclude(c => c!.Course)
                    .AsQueryable();

                // Apply search filter
                if (!string.IsNullOrWhiteSpace(searchTerm))
                {
                    searchTerm = searchTerm.ToLower();
                    query = query.Where(s =>
                        s.Name.ToLower().Contains(searchTerm) ||
                        s.StudentId.ToLower().Contains(searchTerm) ||
                        s.FathersName.ToLower().Contains(searchTerm) ||
                        (s.MobileNo1 != null && s.MobileNo1.Contains(searchTerm)) ||
                        (s.EnrollmentNumber != null && s.EnrollmentNumber.ToLower().Contains(searchTerm))
                    );
                }

                // Apply status filter
                if (!string.IsNullOrWhiteSpace(status))
                {
                    var statusEnum = Enum.Parse<DefaultStatus>(status);
                    query = query.Where(s => s.StatusId == (int)statusEnum);
                }

                // Get total count before pagination
                var totalCount = await query.CountAsync();

                // Apply pagination
                var students = await query
                    .OrderByDescending(s => s.CreatedDate)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                var dtos = new List<StudentDto>();
                foreach (var studentEntity in students)
                {
                    dtos.Add(await MapToDtoAsync(studentEntity));
                }

                return new APIResponse<IEnumerable<StudentDto>>
                {
                    Success = true,
                    Message = "Students fetched successfully",
                    StatusCode = HttpStatusCode.OK,
                    Data = dtos,
                };
            }
            catch (Exception ex)
            {
                return new APIResponse<IEnumerable<StudentDto>>
                {
                    Success = false,
                    Message = $"Failed to get students: {ex.Message}",
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
        }

        public async Task<APIResponse> UpdateStudentAsync(StudentModel model)
        {
            try
            {
                if (!model.Id.HasValue)
                {
                    return new APIResponse
                    {
                        Success = false,
                        Message = "Student ID is required",
                        StatusCode = HttpStatusCode.BadRequest
                    };
                }

                var studentEntity = await _context.Students
                    .FirstOrDefaultAsync(s => s.Id == model.Id.Value && !s.IsDeleted);

                if (studentEntity == null)
                {
                    return new APIResponse
                    {
                        Success = false,
                        Message = "Student not found",
                        StatusCode = HttpStatusCode.NotFound
                    };
                }

                // Verify class exists if ClassId is provided
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

                // Update properties
                if (!string.IsNullOrWhiteSpace(model.EnrollmentNumber))
                    studentEntity.EnrollmentNumber = model.EnrollmentNumber;
                if (model.CourseType != null)
                    studentEntity.CourseType = Convert.ToString(model.CourseType)!;
                if (model.CourseOpted != null)
                    studentEntity.CourseOpted = model.CourseOpted;
                if (!string.IsNullOrWhiteSpace(model.Name))
                    studentEntity.Name = model.Name;
                if (!string.IsNullOrWhiteSpace(model.FathersName))
                    studentEntity.FathersName = model.FathersName;
                if (!string.IsNullOrWhiteSpace(model.Sex))
                    studentEntity.Gender = model.Sex;
                if (model.Nationality != null)
                    studentEntity.Nationality = model.Nationality;
                if (model.Occupation != null)
                    studentEntity.Occupation = model.Occupation;
                if (!string.IsNullOrEmpty(model.DateOfBirth))
                    studentEntity.DateOfBirth = model.DateOfBirth;
                if (model.SchoolCollege != null)
                    studentEntity.SchoolCollege = model.SchoolCollege;
                if (model.QualificationDetails != null)
                    studentEntity.QualificationDetails = model.QualificationDetails;
                if (model.PostalAddress != null)
                    studentEntity.PostalAddress = model.PostalAddress;
                if (model.CityId != null)
                    studentEntity.CityId = model.CityId;
                if (model.StateId != null)
                    studentEntity.StateId = model.StateId;
                if (model.PinCode != null)
                    studentEntity.PinCode = model.PinCode;
                if (model.MobileNo1 != null)
                    studentEntity.MobileNo1 = model.MobileNo1;
                if (model.MobileNo2 != null)
                    studentEntity.MobileNo2 = model.MobileNo2;
                if (model.Image != null)
                    studentEntity.Image = model.Image;
                if (model.ClassId.HasValue)
                    studentEntity.ClassId = model.ClassId;
                if (model.Status != null)
                    studentEntity.StatusId = ((int)model.Status);
                if (model.Remarks != null)
                    studentEntity.Remarks = model.Remarks;

                studentEntity.UpdatedBy = model.UpdatedBy;
                studentEntity.UpdatedDate = DateTime.UtcNow;

                Update(studentEntity);
                await _unitOfWork.CommitAsync();

                return new APIResponse
                {
                    Success = true,
                    Message = "Student updated successfully",
                    StatusCode = HttpStatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new APIResponse
                {
                    Success = false,
                    Message = $"Failed to update student: {ex.Message}",
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
        }

        public async Task<APIResponse> DeleteStudentAsync(int id)
        {
            try
            {
                var studentEntity = await _context.Students
                    .FirstOrDefaultAsync(s => s.Id == id && !s.IsDeleted);

                if (studentEntity == null)
                {
                    return new APIResponse
                    {
                        Success = false,
                        Message = "Student not found",
                        StatusCode = HttpStatusCode.NotFound
                    };
                }

                studentEntity.IsDeleted = true;
                studentEntity.UpdatedDate = DateTime.UtcNow;
                Delete(studentEntity);
                await _unitOfWork.CommitAsync();

                return new APIResponse
                {
                    Success = true,
                    Message = "Student deleted successfully",
                    StatusCode = HttpStatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new APIResponse
                {
                    Success = false,
                    Message = $"Failed to delete student: {ex.Message}",
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
        }

        public async Task<string> GenerateStudentIdAsync()
        {

            // Get the current year
            var currentYear = DateTime.Now.Year;
            var yearPrefix = currentYear.ToString().Substring(2, 2); // Last 2 digits of year

            // Get the last student ID for this year
            var lastStudent = await _context.Students
                .Where(s => s.StudentId.StartsWith($"STU{yearPrefix}"))
                .OrderByDescending(s => s.StudentId)
                .FirstOrDefaultAsync();

            int nextNumber = 1;
            if (lastStudent != null)
            {
                // Extract the number from the last student ID (e.g., STU24001 -> 1)
                var lastNumberStr = lastStudent.StudentId.Substring(5); // Skip "STU24"
                if (int.TryParse(lastNumberStr, out int lastNumber))
                {
                    nextNumber = lastNumber + 1;
                }
            }

            // Format: STU + Year (2 digits) + Sequential Number (4 digits)
            // Example: STU24001, STU24002, etc.
            var studentId = $"STU{yearPrefix}{nextNumber:D4}";

            // Ensure uniqueness (in case of race condition)
            var exists = await _context.Students.AnyAsync(s => s.StudentId == studentId);
            int retryCount = 0;
            while (exists && retryCount < 10) // Max 10 retries
            {
                nextNumber++;
                studentId = $"STU{yearPrefix}{nextNumber:D4}";
                exists = await _context.Students.AnyAsync(s => s.StudentId == studentId);
                retryCount++;
            }

            if (exists)
            {
                return "";
            }

            return studentId;

        }

        private async Task<StudentDto> MapToDtoAsync(Student studentEntity)
        {
            var classEntity = studentEntity.Class ?? (studentEntity.ClassId.HasValue
                ? await _context.Classes
                    .Include(c => c.Course)
                    .FirstOrDefaultAsync(c => c.Id == studentEntity.ClassId.Value)
                : null);

            return new StudentDto
            {
                Id = studentEntity.Id,
                StudentId = studentEntity.StudentId,
                EnrollmentNumber = studentEntity.EnrollmentNumber,
                CourseType = studentEntity.CourseType,
                CourseOpted = studentEntity.CourseOpted,
                Name = studentEntity.Name,
                FathersName = studentEntity.FathersName,
                Sex = studentEntity.Gender,
                Nationality = studentEntity.Nationality,
                Occupation = studentEntity.Occupation,
                SchoolCollege = studentEntity.SchoolCollege,
                QualificationDetails = studentEntity.QualificationDetails,
                PostalAddress = studentEntity.PostalAddress,
                PinCode = studentEntity.PinCode,
                MobileNo1 = studentEntity.MobileNo1,
                MobileNo2 = studentEntity.MobileNo2,
                Image = studentEntity.Image,
                ClassId = studentEntity.ClassId,
                ClassName = classEntity?.Name,
                Section = classEntity?.Section,
                Remarks = studentEntity.Remarks,
                CreatedDate = studentEntity.CreatedDate.ToString(),
                CreatedBy = studentEntity.CreatedBy,
                UpdatedDate = studentEntity.UpdatedDate.ToString(),
                UpdatedBy = studentEntity.UpdatedBy
            };
        }
    }
}

