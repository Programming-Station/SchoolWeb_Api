using AutoMapper;
using School.Domain;
using School.Domain.Auth;
using School.Infrastructure.Repositories.IRepositories;
using School.Models.Teacher;
using School.Services.Interfaces;
using School.Utilities.Resources;
using School_DTOs; 
using School_DTOs.Teacher;
using System.Net;
using Microsoft.AspNetCore.Identity;
using School.Utilities.Constants;
using School.Utilities.Enums;

namespace School.Services
{
    public class TeacherService : ITeacherService
    {
        private readonly ITeacherRepository _teacherRepository;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public TeacherService(
            ITeacherRepository teacherRepository, 
            IMapper mapper,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _teacherRepository = teacherRepository;
            _mapper = mapper;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<APIResponse<TeacherDto>> AddTeacherAsync(TeacherModel model, string? password = null)
        {
            // Check if user with this email already exists
            var existingUser = await _userManager.FindByEmailAsync(model.Email);
            if (existingUser != null)
            {
                return new APIResponse<TeacherDto>
                {
                    Success = false,
                    Message = string.Format(CommonResource.AlreadyExists, "User", model.Email),
                    StatusCode = HttpStatusCode.BadRequest,
                };
            }

            // Create ApplicationUser for login
            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.PhoneNumber,
                EmailConfirmed = true,
                PhoneNumberConfirmed = !string.IsNullOrEmpty(model.PhoneNumber),
                IsActive = model.Status == "active",
                StatusId = (int)DefaultStatus.Verified,
                IsDefaultPassword = !string.IsNullOrEmpty(password)
            };

            // Generate default password if not provided
            var defaultPassword = password ?? "Password@123";

            // Create user using UserManager
            var createUserResult = await _userManager.CreateAsync(user, defaultPassword);
            if (!createUserResult.Succeeded)
            {
                var errors = string.Join(", ", createUserResult.Errors.Select(e => e.Description));
                return new APIResponse<TeacherDto>
                {
                    Success = false,
                    Message = $"Failed to create user: {errors}",
                    StatusCode = HttpStatusCode.BadRequest,
                };
            }

            // Assign Teacher role to the user
            var teacherRole = await _roleManager.FindByNameAsync(Roles.Teacher.ToString());
            if (teacherRole != null)
            {
                await _userManager.AddToRoleAsync(user, teacherRole.Name!);
            }

            // Create Teacher entity linked to ApplicationUser
            var entity = _mapper.Map<Teacher>(model);
            entity.UserId = user.Id; // Link to ApplicationUser
            entity = await _teacherRepository.AddTeacherAsync(entity);

            if (entity != null && entity.Id > 0)
            {
                var teacherDto = await MapTeacherToDtoAsync(entity);
                return new APIResponse<TeacherDto>
                {
                    Success = true,
                    Data = teacherDto,
                    Message = CommonResource.AddSuccess,
                    StatusCode = HttpStatusCode.Created
                };
            }
            else
            {
                // Rollback: Delete user if teacher creation failed
                await _userManager.DeleteAsync(user);
                return new APIResponse<TeacherDto>
                {
                    Success = false,
                    Message = CommonResource.AddFailed,
                    StatusCode = HttpStatusCode.Forbidden
                };
            }
        }

        private async Task<TeacherDto> MapTeacherToDtoAsync(Teacher teacher)
        {
            var dto = _mapper.Map<TeacherDto>(teacher);
            
            // Populate user properties from ApplicationUser
            if (teacher.User != null)
            {
                dto.FirstName = teacher.User.FirstName;
                dto.LastName = teacher.User.LastName;
                dto.Email = teacher.User.Email ?? "";
                dto.PhoneNumber = teacher.User.PhoneNumber;
            }
            else if (!string.IsNullOrEmpty(teacher.UserId))
            {
                // Load user if not included
                var user = await _userManager.FindByIdAsync(teacher.UserId);
                if (user != null)
                {
                    dto.FirstName = user.FirstName;
                    dto.LastName = user.LastName;
                    dto.Email = user.Email ?? "";
                    dto.PhoneNumber = user.PhoneNumber;
                }
            }

            // Populate related entity names
            if (teacher.City != null)
                dto.CityName = teacher.City.Name;
            if (teacher.State != null)
                dto.StateName = teacher.State.Name;
            if (teacher.Course != null)
                dto.CourseName = teacher.Course.Name;
            if (teacher.Faculty != null)
                dto.Faculty = teacher.Faculty.Name;

            return dto;
        }

        public async Task<APIResponse<TeacherDto>> GetTeacherByIdAsync(int id)
        {
            var result = await _teacherRepository.GetTeacherByIdAsync(id);

            if (result != null && result.Id > 0)
            {
                var teacherDto = await MapTeacherToDtoAsync(result);
                return new APIResponse<TeacherDto>
                {
                    Data = teacherDto,
                    Message = CommonResource.FetchSuccess,
                    Success = true,
                    StatusCode = HttpStatusCode.OK
                };
            }
            else
            {
                return new APIResponse<TeacherDto>
                {
                    Message = CommonResource.RecordNotFound,
                    StatusCode = HttpStatusCode.OK
                };
            }
        }

        public async Task<APIResponse<TeacherDto>> GetTeacherByUserIdAsync(string userId)
        {
            var result = await _teacherRepository.GetTeacherByUserIdAsync(userId);

            if (result != null && result.Id > 0)
            {
                var teacherDto = await MapTeacherToDtoAsync(result);
                return new APIResponse<TeacherDto>
                {
                    Data = teacherDto,
                    Message = CommonResource.FetchSuccess,
                    Success = true,
                    StatusCode = HttpStatusCode.OK
                };
            }
            else
            {
                return new APIResponse<TeacherDto>
                {
                    Message = CommonResource.RecordNotFound,
                    StatusCode = HttpStatusCode.OK
                };
            }
        }

        public async Task<APIResponse<TeacherDto>> GetTeacherByTeacherIdAsync(string teacherId)
        {
            var result = await _teacherRepository.GetTeacherByTeacherIdAsync(teacherId);

            if (result != null && result.Id > 0)
            {
                var teacherDto = await MapTeacherToDtoAsync(result);
                return new APIResponse<TeacherDto>
                {
                    Data = teacherDto,
                    Message = CommonResource.FetchSuccess,
                    Success = true,
                    StatusCode = HttpStatusCode.OK
                };
            }
            else
            {
                return new APIResponse<TeacherDto>
                {
                    Message = CommonResource.RecordNotFound,
                    StatusCode = HttpStatusCode.OK
                };
            }
        }

        public async Task<APIResponse<TeacherDto>> GetTeacherByEmailAsync(string email)
        {
            // Find user by email first
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return new APIResponse<TeacherDto>
                {
                    Message = CommonResource.RecordNotFound,
                    StatusCode = HttpStatusCode.OK
                };
            }

            var result = await _teacherRepository.GetTeacherByUserIdAsync(user.Id);
            if (result != null && result.Id > 0)
            {
                var teacherDto = await MapTeacherToDtoAsync(result);
                return new APIResponse<TeacherDto>
                {
                    Data = teacherDto,
                    Message = CommonResource.FetchSuccess,
                    Success = true,
                    StatusCode = HttpStatusCode.OK
                };
            }
            else
            {
                return new APIResponse<TeacherDto>
                {
                    Message = CommonResource.RecordNotFound,
                    StatusCode = HttpStatusCode.OK
                };
            }
        }

        public async Task<APIResponse<PagedResponse<TeacherDto>>> GetAllTeachersAsync(TeacherFilter filter)
        {
            var (teachers, totalCount) = await _teacherRepository.GetAllTeachersAsync(filter);

            if (teachers != null && teachers.Any())
            {
                var teacherDtos = new List<TeacherDto>();
                foreach (var teacher in teachers)
                {
                    var dto = await MapTeacherToDtoAsync(teacher);
                    teacherDtos.Add(dto);
                }

                var pagedResponse = new PagedResponse<TeacherDto>
                {
                    Data = teacherDtos,
                    TotalRecords = totalCount,
                    CurrentPage = filter.PageIndex,
                    PageSize = filter.PageSize,
                    Success = true,
                    StatusCode = HttpStatusCode.OK,
                    Message = CommonResource.FetchSuccess
                };

                return new APIResponse<PagedResponse<TeacherDto>>
                {
                    Data = pagedResponse,
                    Message = CommonResource.FetchSuccess,
                    Success = true,
                    StatusCode = HttpStatusCode.OK,
                };
            }
            else
            {
                var emptyPagedResponse = new PagedResponse<TeacherDto>
                {
                    Data = new List<TeacherDto>(),
                    TotalRecords = 0,
                    CurrentPage = filter.PageIndex,
                    PageSize = filter.PageSize,
                    Success = true,
                    StatusCode = HttpStatusCode.OK,
                    Message = CommonResource.RecordNotFound
                };

                return new APIResponse<PagedResponse<TeacherDto>>
                {
                    Data = emptyPagedResponse,
                    Message = CommonResource.RecordNotFound,
                    StatusCode = HttpStatusCode.OK,
                };
            }
        }

        public async Task<APIResponse> UpdateTeacherAsync(TeacherModel model)
        {
            // Get existing teacher
            var existingTeacher = await _teacherRepository.GetTeacherByIdAsync(model.Id ?? 0);
            if (existingTeacher == null || existingTeacher.Id == 0)
            {
                return new APIResponse
                {
                    Message = CommonResource.RecordNotFound,
                    StatusCode = HttpStatusCode.NotFound,
                };
            }

            // Update ApplicationUser properties
            var user = await _userManager.FindByIdAsync(existingTeacher.UserId);
            if (user != null)
            {
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Email = model.Email;
                user.UserName = model.Email;
                user.PhoneNumber = model.PhoneNumber;
                user.IsActive = model.Status == "active";

                var updateUserResult = await _userManager.UpdateAsync(user);
                if (!updateUserResult.Succeeded)
                {
                    var errors = string.Join(", ", updateUserResult.Errors.Select(e => e.Description));
                    return new APIResponse
                    {
                        Message = $"Failed to update user: {errors}",
                        StatusCode = HttpStatusCode.BadRequest,
                    };
                }
            }

            // Update Teacher entity
            var entity = _mapper.Map<Teacher>(model);
            entity.UserId = existingTeacher.UserId; // Preserve UserId
            var result = await _teacherRepository.UpdateTeacherAsync(entity);

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

        public async Task<APIResponse> DeleteTeacherAsync(int id)
        {
            // Get teacher to find associated user
            var teacher = await _teacherRepository.GetTeacherByIdAsync(id);
            if (teacher == null || teacher.Id == 0)
            {
                return new APIResponse
                {
                    Message = CommonResource.RecordNotFound,
                    StatusCode = HttpStatusCode.NotFound,
                };
            }

            // Delete teacher record (soft delete)
            int changes = await _teacherRepository.DeleteTeacherAsync(id);
            if (changes > 0)
            {
                // Optionally deactivate user account instead of deleting
                // (Deleting user would remove login capability)
                var user = await _userManager.FindByIdAsync(teacher.UserId);
                if (user != null)
                {
                    user.IsActive = false;
                    await _userManager.UpdateAsync(user);
                }

                return new APIResponse
                {
                    Success = true,
                    Message = CommonResource.DeleteSuccess,
                    StatusCode = HttpStatusCode.OK,
                };
            }
            else
                return new APIResponse
                {
                    Message = CommonResource.DeleteFailed,
                    StatusCode = HttpStatusCode.BadRequest,
                };
        }

        public async Task<APIResponse> ToggleTeacherStatusAsync(int id)
        {
            int changes = await _teacherRepository.ToggleTeacherStatusAsync(id);
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

        public async Task<APIResponse<string>> GenerateTeacherIdAsync()
        {
            // Generate teacher ID in format: TCH + YY + 4-digit number
            var year = DateTime.Now.Year % 100;
            var prefix = $"TCH{year:D2}";

            // Get the last teacher ID with this prefix
            var filter = new TeacherFilter
            {
                PageIndex = 1,
                PageSize = 1,
                OrderBy = "TeacherId",
                SortDirection = "desc"
            };

            var (teachers, _) = await _teacherRepository.GetAllTeachersAsync(filter);
            var lastTeacher = teachers.FirstOrDefault();

            int nextNumber = 1;
            if (lastTeacher != null && !string.IsNullOrEmpty(lastTeacher.TeacherId) &&
                lastTeacher.TeacherId.StartsWith(prefix))
            {
                var lastNumberStr = lastTeacher.TeacherId.Substring(prefix.Length);
                if (int.TryParse(lastNumberStr, out int lastNumber))
                {
                    nextNumber = lastNumber + 1;
                }
            }

            var teacherId = $"{prefix}{nextNumber:D4}";

            return new APIResponse<string>
            {
                Data = teacherId,
                Success = true,
                Message = "Teacher ID generated successfully",
                StatusCode = HttpStatusCode.OK
            };
        }
    }
}

