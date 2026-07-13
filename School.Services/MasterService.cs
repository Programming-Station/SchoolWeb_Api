using AutoMapper;
using School.Infrastructure;
using School.Infrastructure.Seeds;
using School.Services.Interfaces;
using School.Utilities.Enums;
using School.Utilities.Resources;
using School_DTOs;
using School_DTOs.Account;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System.Net;
using System.Text.Json;

namespace School.Services
{
    public class MasterService : IMasterService
    {
        private readonly IMapper _mapper;
        private readonly SchoolDbContext _dbContext;
        private readonly IDistributedCache _cache;
        public MasterService(IMapper mapper, SchoolDbContext dbContext, IDistributedCache cache)
        {
            _mapper = mapper;
            _dbContext = dbContext;
            _cache = cache;
        }
        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetCitiesAsync(int id)
        {
            var entity = await _dbContext.Cities.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.Name, Id = x.Id, Code = x.CityCode }).ToListAsync();
            if (entity != null)
                return new APIResponse<IEnumerable<DropdownDto>>
                {
                    Data = entity,
                    Message = CommonResource.FetchSuccess,
                    Success = true,
                    StatusCode = HttpStatusCode.OK
                };
            else
                return new APIResponse<IEnumerable<DropdownDto>>
                {
                    Message = CommonResource.RecordNotFound,
                    Success = false,
                    StatusCode = HttpStatusCode.OK
                };
        }
        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetStatesAsync(int id)
        {
            string key = $"States_{id}";

            if (!TryGetFromCache<IEnumerable<DropdownDto>>(key, out var result) || result == null)
            {
                var entity = await _dbContext.States.Where(x => !x.IsDeleted && x.CountryId == id).Select(x => new DropdownDto { Name = x.Name, Id = x.Id, Code = x.StateCode }).ToListAsync();
                if (entity == null || !entity.Any())
                {
                    return new APIResponse<IEnumerable<DropdownDto>>
                    {
                        Message = CommonResource.RecordNotFound,
                        Success = false,
                        StatusCode = HttpStatusCode.OK
                    };
                }

                _cache.SetString(key, JsonSerializer.Serialize(entity));
                result = entity;
            }

            return new APIResponse<IEnumerable<DropdownDto>>
            {
                Data = result,
                Message = CommonResource.FetchSuccess,
                Success = true,
                StatusCode = HttpStatusCode.OK
            };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetStatusAsync()
        {
            var enity = await _dbContext.Statuses.Select(x => new DropdownDto { Name = x.Name, Id = x.Id }).ToListAsync();
            if (enity != null)
                return new APIResponse<IEnumerable<DropdownDto>>
                {
                    Data = _mapper.Map<List<DropdownDto>>(enity),
                    Message = CommonResource.FetchSuccess,
                    Success = true,
                    StatusCode = HttpStatusCode.OK
                };
            else
                return new APIResponse<IEnumerable<DropdownDto>>
                {
                    Message = CommonResource.RecordNotFound,
                    Success = false,
                    StatusCode = HttpStatusCode.OK
                };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetCoursesAsync(int courseTypeId)
        {
            string key = "Courses";

            if (!TryGetFromCache<IEnumerable<DropdownDto>>(key, out var result) || result == null)
            {
                var entity = await _dbContext.Courses.Where(x => !x.IsDeleted && (int)x.CourseType! == courseTypeId).Select(x => new DropdownDto { Name = x.Name, Id = x.Id, Code = x.CourseCode }).ToListAsync();
                if (entity == null || !entity.Any())
                {
                    return new APIResponse<IEnumerable<DropdownDto>>
                    {
                        Message = CommonResource.RecordNotFound,
                        Success = false,
                        StatusCode = HttpStatusCode.OK
                    };
                }

                _cache.SetString(key, JsonSerializer.Serialize(entity));
            }

            return new APIResponse<IEnumerable<DropdownDto>>
            {
                Data = result,
                Message = CommonResource.FetchSuccess,
                Success = true,
                StatusCode = HttpStatusCode.OK
            };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetAcademicYearAsync()
        {
            string key = "AcademicYear";

            if (!TryGetFromCache<IEnumerable<DropdownDto>>(key, out var result) || result == null)
            {
                var entity = await _dbContext.AcademicYears.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.YearName, Id = x.Id, Code = x.YearName }).ToListAsync();
                if (entity == null || !entity.Any())
                {
                    return new APIResponse<IEnumerable<DropdownDto>>
                    {
                        Message = CommonResource.RecordNotFound,
                        Success = false,
                        StatusCode = HttpStatusCode.OK
                    };
                }

                _cache.SetString(key, JsonSerializer.Serialize(entity));
            }

            return new APIResponse<IEnumerable<DropdownDto>>
            {
                Data = result,
                Message = CommonResource.FetchSuccess,
                Success = true,
                StatusCode = HttpStatusCode.OK
            };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetAffiliationBoardsAsync()
        {
            var data = await _dbContext.AffiliationBoards
                .Where(x => x.IsActive && !x.IsDeleted)
                .Select(x => new DropdownDto { Id = x.Id, Name = x.Name })
                .ToListAsync();

            return new APIResponse<IEnumerable<DropdownDto>>
            {
                Data = data,
                Success = true,
                StatusCode = HttpStatusCode.OK
            };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetSchoolTypesAsync()
        {
            var data = await _dbContext.SchoolTypes
                .Where(x => x.IsActive && !x.IsDeleted)
                .Select(x => new DropdownDto { Id = x.Id, Name = x.Name })
                .ToListAsync();

            return new APIResponse<IEnumerable<DropdownDto>>
            {
                Data = data,
                Success = true,
                StatusCode = HttpStatusCode.OK
            };
        }
        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetSchoolMediumsAsync()
        {
            var data = await _dbContext.SchoolMediums
                .Where(x => x.IsActive && !x.IsDeleted)
                .Select(x => new DropdownDto { Id = x.Id, Name = x.Name })
                .ToListAsync();

            return new APIResponse<IEnumerable<DropdownDto>>
            {
                Data = data,
                Success = true,
                StatusCode = HttpStatusCode.OK
            };
        }

        private bool TryGetFromCache<T>(string key, out T? value)
        {
            var cachedData = _cache.GetString(key);
            if (!string.IsNullOrEmpty(cachedData))
            {
                value = JsonSerializer.Deserialize<T>(cachedData);
                return true;
            }

            value = default;
            return false;
        }

        public async Task<APIResponse<IEnumerable<RoleDto>>> GetRolesAsync()
        {
            var roles = await _dbContext.Roles.Where(x => x.Name != Roles.SuperAdmin.ToString()).Select(x => new RoleDto
            {
                Id = x.Id,
                Name = x.Name!,
                NormalizedName = x.NormalizedName!

            }).ToListAsync();
            if (roles != null)
            {
                return new APIResponse<IEnumerable<RoleDto>>
                {
                    Data = roles,
                    Message = CommonResource.FetchSuccess,
                    Success = true,
                    StatusCode = HttpStatusCode.OK
                };
            }
            else
            {
                return new APIResponse<IEnumerable<RoleDto>>
                {
                    Message = CommonResource.RecordNotFound,
                    Success = false,
                    StatusCode = HttpStatusCode.OK
                };
            }
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetCountriesAsync()
        {
            var entity = await _dbContext.Countries.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.Name, Id = x.Id, Code = x.CountryCode }).ToListAsync();
            if (entity != null)
                return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
            else
                return new APIResponse<IEnumerable<DropdownDto>> { Message = CommonResource.RecordNotFound, Success = false, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetModulesAsync()
        {
            var entity = await _dbContext.Modules.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.Name, Id = x.Id }).ToListAsync();
            if (entity != null)
                return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
            else
                return new APIResponse<IEnumerable<DropdownDto>> { Message = CommonResource.RecordNotFound, Success = false, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetMenusAsync()
        {
            var entity = await _dbContext.Menus.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.MenuName, Id = x.Id }).ToListAsync();
            if (entity != null)
                return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
            else
                return new APIResponse<IEnumerable<DropdownDto>> { Message = CommonResource.RecordNotFound, Success = false, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetSubMenusAsync(int menuId)
        {
            var entity = await _dbContext.SubMenus.Where(x => !x.IsDeleted && x.MenuId == menuId).Select(x => new DropdownDto { Name = x.SubMenuName, Id = x.Id }).ToListAsync();
            if (entity != null)
                return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
            else
                return new APIResponse<IEnumerable<DropdownDto>> { Message = CommonResource.RecordNotFound, Success = false, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetClassesAsync()
        {
            var entity = await _dbContext.Classes.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.Name, Id = x.Id }).ToListAsync();
            if (entity != null)
                return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
            else
                return new APIResponse<IEnumerable<DropdownDto>> { Message = CommonResource.RecordNotFound, Success = false, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetDepartmentsAsync()
        {
            var entity = await _dbContext.Departments.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.Name, Id = x.Id, Code = x.Code }).ToListAsync();
            if (entity != null)
                return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
            else
                return new APIResponse<IEnumerable<DropdownDto>> { Message = CommonResource.RecordNotFound, Success = false, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetFeeTypesAsync()
        {
            var entity = await _dbContext.FeeTypes.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.Name, Id = x.Id }).ToListAsync();
            if (entity != null)
                return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
            else
                return new APIResponse<IEnumerable<DropdownDto>> { Message = CommonResource.RecordNotFound, Success = false, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetFacultiesAsync()
        {
            var entity = await _dbContext.Faculties.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.Name, Id = x.Id, Code = x.Code }).ToListAsync();
            if (entity != null)
                return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
            else
                return new APIResponse<IEnumerable<DropdownDto>> { Message = CommonResource.RecordNotFound, Success = false, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetCategoryModulesAsync()
        {
            var entity = await _dbContext.CategoryModules.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.Name, Id = x.Id }).ToListAsync();
            if (entity != null)
                return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
            else
                return new APIResponse<IEnumerable<DropdownDto>> { Message = CommonResource.RecordNotFound, Success = false, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetAffiliatedsAsync()
        {
            var entity = await _dbContext.Affiliateds.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.CollegeName, Id = x.Id, Code = x.CollegeCode }).ToListAsync();
            if (entity != null)
                return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
            else
                return new APIResponse<IEnumerable<DropdownDto>> { Message = CommonResource.RecordNotFound, Success = false, StatusCode = HttpStatusCode.OK };
        }

        // â”€â”€â”€ HR Master dropdowns â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetDesignationsAsync()
        {
            var entity = await _dbContext.Designations.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.Name, Id = x.Id, Code = x.Code }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetEmployeesAsync()
        {
            var entity = await _dbContext.Employees.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.FirstName + " " + x.LastName, Id = x.Id, Code = x.EmployeeCode }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetEmployeeCategoriesAsync()
        {
            var entity = await _dbContext.EmployeeCategories.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.Name, Id = x.Id, Code = x.Code }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetEmployeeTypesAsync()
        {
            var entity = await _dbContext.EmployeeTypes.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.Name, Id = x.Id, Code = x.Code }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetEmploymentStatusesAsync()
        {
            var entity = await _dbContext.EmploymentStatuses.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.Name, Id = x.Id, Code = x.Code }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetSalaryGradesAsync()
        {
            var entity = await _dbContext.SalaryGrades.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.Name, Id = x.Id, Code = x.Code }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetBloodGroupMastersAsync()
        {
            var entity = await _dbContext.BloodGroupMasters.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.Name, Id = x.Id }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetQualificationMastersAsync()
        {
            var entity = await _dbContext.QualificationMasters.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.Name, Id = x.Id }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetReligionMastersAsync()
        {
            var entity = await _dbContext.ReligionMasters.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.Name, Id = x.Id }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetSpecializationsAsync()
        {
            var entity = await _dbContext.Specializations.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.Name, Id = x.Id, Code = x.Code }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetShiftMastersAsync()
        {
            var entity = await _dbContext.ShiftMasters.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.Name, Id = x.Id, Code = x.Code }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetHolidayMastersAsync()
        {
            var entity = await _dbContext.HolidayMasters.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.Name, Id = x.Id, Code = x.Code }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetWeekOffsAsync()
        {
            var entity = await _dbContext.WeekOffs.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.Name, Id = x.Id, Code = x.Code }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetNoticePeriodsAsync()
        {
            var entity = await _dbContext.NoticePeriods.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.Name, Id = x.Id, Code = x.Code }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetLeaveTypesAsync()
        {
            var entity = await _dbContext.LeaveTypes.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.Name, Id = x.Id, Code = x.Code }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetLeaveSettingsAsync()
        {
            var entity = await _dbContext.LeaveSettings.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.Name, Id = x.Id, Code = x.Code }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetJobPostingsAsync()
        {
            var entity = await _dbContext.JobPostings.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.Title, Id = x.Id }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetCandidatesAsync()
        {
            var entity = await _dbContext.Candidates.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.FullName, Id = x.Id }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetJobApplicationsAsync()
        {
            var entity = await _dbContext.JobApplications.Where(x => !x.IsDeleted)
                .Select(x => new DropdownDto { Name = x.Candidate.FullName + " - " + x.JobPosting.Title, Id = x.Id }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetPerformanceReviewsAsync()
        {
            var entity = await _dbContext.PerformanceReviews.Where(x => !x.IsDeleted)
                .Select(x => new DropdownDto { Name = x.Employee.FirstName + " " + x.Employee.LastName + " (" + x.ReviewDate.Year + ")", Id = x.Id }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetKpiMetricsAsync()
        {
            var entity = await _dbContext.KpiMetrics.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.Name, Id = x.Id }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetTrainingProgramsAsync()
        {
            var entity = await _dbContext.TrainingPrograms.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.Title, Id = x.Id }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetTrainingEnrollmentsAsync()
        {
            var entity = await _dbContext.TrainingEnrollments.Where(x => !x.IsDeleted)
                .Select(x => new DropdownDto { Name = x.Employee.FirstName + " " + x.Employee.LastName + " - " + x.TrainingProgram.Title, Id = x.Id }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetSchoolAssetsAsync()
        {
            var entity = await _dbContext.SchoolAssets.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.Name, Id = x.Id, Code = x.AssetCode }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetAssetAssignmentsAsync()
        {
            var entity = await _dbContext.AssetAssignments.Where(x => !x.IsDeleted)
                .Select(x => new DropdownDto { Name = x.SchoolAsset.Name + " â†’ " + x.Employee.FirstName + " " + x.Employee.LastName, Id = x.Id }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        // â”€â”€â”€ Academic â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetSubjectsAsync()
        {
            var entity = await _dbContext.Subjects.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.Name, Id = x.Id, Code = x.Code }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        // â”€â”€â”€ Payroll â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetSalaryComponentsAsync()
        {
            var entity = await _dbContext.SalaryComponents.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.Name, Id = x.Id }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        // â”€â”€â”€ Transport â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetTransportRoutesAsync()
        {
            var entity = await _dbContext.TransportRoutes.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.RouteName, Id = x.Id }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetVehiclesAsync()
        {
            var entity = await _dbContext.Vehicles.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.Name, Id = x.Id, Code = x.RegistrationNumber }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }
    
public async Task<APIResponse<IEnumerable<DropdownDto>>> GetStudentsAsync()
        {
            var entity = await _dbContext.Students.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.Name, Id = x.Id }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetSchoolRegistrationsAsync()
        {
            var entity = await _dbContext.SchoolRegistrations.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.SchoolName, Id = x.Id }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetSchoolProfileSettingsAsync()
        {
            var entity = await _dbContext.SchoolProfileSettings.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.BankAccountName, Id = x.Id }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetSchoolSubscriptionsAsync()
        {
            var entity = await _dbContext.SchoolSubscriptions.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.PaymentStatus, Id = x.Id }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetSchoolOwnersAsync()
        {
            var entity = await _dbContext.SchoolOwners.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.ApplicationUserId, Id = x.Id }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetEducationalDetailsAsync()
        {
            var entity = await _dbContext.EducationalDetails.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.ExamName, Id = x.Id }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetEmailServerSettingsAsync()
        {
            var entity = await _dbContext.EmailServerSettings.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.FromEmail, Id = x.Id }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetEmailTemplatesAsync()
        {
            var entity = await _dbContext.EmailTemplates.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.TemplateName, Id = x.Id }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetEmailBrandingsAsync()
        {
            var entity = await _dbContext.EmailBrandings.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.ThemeColor, Id = x.Id }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetExamsAsync()
        {
            var entity = await _dbContext.Exams.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.Name, Id = x.Id }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetExamResultsAsync()
        {
            var entity = await _dbContext.ExamResults.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.Grade, Id = x.Id }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetEventsAsync()
        {
            var entity = await _dbContext.Events.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.Title, Id = x.Id }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetEmployeeDocumentsAsync()
        {
            var entity = await _dbContext.EmployeeDocuments.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.DocumentName, Id = x.Id }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetEmployeeBankDetailsAsync()
        {
            var entity = await _dbContext.EmployeeBankDetails.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.BankName, Id = x.Id }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetEmployeeEducationsAsync()
        {
            var entity = await _dbContext.EmployeeEducations.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.Degree, Id = x.Id }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetEmployeeExperiencesAsync()
        {
            var entity = await _dbContext.EmployeeExperiences.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.Company, Id = x.Id }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetEmployeeSalaryDetailsAsync()
        {
            var entity = await _dbContext.EmployeeSalaryDetails.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.Id.ToString(), Id = x.Id }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetEmployeeDetailsAsync()
        {
            var entity = await _dbContext.EmployeeDetails.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.FatherName, Id = x.Id }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetLeaveRequestsAsync()
        {
            var entity = await _dbContext.LeaveRequests.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.Reason, Id = x.Id }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetLeaveBalancesAsync()
        {
            var entity = await _dbContext.LeaveBalances.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.Year, Id = x.Id }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetAttendancesAsync()
        {
            var entity = await _dbContext.Attendances.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.Status, Id = x.Id }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetAttendanceLogsAsync()
        {
            var entity = await _dbContext.AttendanceLogs.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.LogType, Id = x.Id }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetTimesheetsAsync()
        {
            var entity = await _dbContext.Timesheets.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.Status, Id = x.Id }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetTimesheetEntriesAsync()
        {
            var entity = await _dbContext.TimesheetEntries.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.TaskName, Id = x.Id }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetPayrollRunsAsync()
        {
            var entity = await _dbContext.PayrollRuns.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.Month, Id = x.Id }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetPayGroupsAsync()
        {
            var entity = await _dbContext.PayGroups.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.Name, Id = x.Id }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetSalaryStructuresAsync()
        {
            var entity = await _dbContext.SalaryStructures.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.Name, Id = x.Id }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetSalaryStructureItemsAsync()
        {
            var entity = await _dbContext.SalaryStructureItems.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.CalculationType, Id = x.Id }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetEmployeeSalaryAllocationsAsync()
        {
            var entity = await _dbContext.EmployeeSalaryAllocations.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.Remarks, Id = x.Id }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetEmployeeLoansAsync()
        {
            var entity = await _dbContext.EmployeeLoans.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.Purpose, Id = x.Id }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetLoanRepaymentSchedulesAsync()
        {
            var entity = await _dbContext.LoanRepaymentSchedules.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.Status, Id = x.Id }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetSalaryAdvancesAsync()
        {
            var entity = await _dbContext.SalaryAdvances.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.Status, Id = x.Id }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetEmployeeBonusesAsync()
        {
            var entity = await _dbContext.EmployeeBonuses.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.BonusType, Id = x.Id }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetReimbursementClaimsAsync()
        {
            var entity = await _dbContext.ReimbursementClaims.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.ClaimType, Id = x.Id }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetSalaryArrearsAsync()
        {
            var entity = await _dbContext.SalaryArrears.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.ArrearMonth, Id = x.Id }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetStatutoryComplianceConfigsAsync()
        {
            var entity = await _dbContext.StatutoryComplianceConfigs.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.ProfessionalTaxSlabJson, Id = x.Id }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetPayrollRunDetailsAsync()
        {
            var entity = await _dbContext.PayrollRunDetails.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.Id.ToString(), Id = x.Id }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetTransportAllocationsAsync()
        {
            var entity = await _dbContext.TransportAllocations.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.SeatNumber, Id = x.Id }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetTransportStopsAsync()
        {
            var entity = await _dbContext.TransportStops.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.StopName, Id = x.Id }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetRouteStopMappingsAsync()
        {
            var entity = await _dbContext.RouteStopMappings.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.Id.ToString(), Id = x.Id }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetConductorsAsync()
        {
            var entity = await _dbContext.Conductors.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.LicenseNumber, Id = x.Id }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetRouteAssignmentsAsync()
        {
            var entity = await _dbContext.RouteAssignments.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.Status, Id = x.Id }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetTransportTripsAsync()
        {
            var entity = await _dbContext.TransportTrips.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.Status, Id = x.Id }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetVehicleMaintenancesAsync()
        {
            var entity = await _dbContext.VehicleMaintenances.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.VendorName, Id = x.Id }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetVehicleIncidentsAsync()
        {
            var entity = await _dbContext.VehicleIncidents.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.Description, Id = x.Id }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetTransportInventoriesAsync()
        {
            var entity = await _dbContext.TransportInventories.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.ItemName, Id = x.Id, Code = x.SerialNumber }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetBooksAsync()
        {
            var entity = await _dbContext.Books.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.Title, Id = x.Id, Code = x.Barcode }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetBookIssueLogsAsync()
        {
            var entity = await _dbContext.BookIssueLogs.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.Status, Id = x.Id }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetBookCategoriesAsync()
        {
            var entity = await _dbContext.BookCategories.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.Name, Id = x.Id }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetBookAuthorsAsync()
        {
            var entity = await _dbContext.BookAuthors.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.Name, Id = x.Id }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetBookPublishersAsync()
        {
            var entity = await _dbContext.BookPublishers.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.Name, Id = x.Id }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetBookVendorsAsync()
        {
            var entity = await _dbContext.BookVendors.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.Name, Id = x.Id }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetLibraryMembersAsync()
        {
            var entity = await _dbContext.LibraryMembers.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.MembershipNumber, Id = x.Id }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetBookReservationsAsync()
        {
            var entity = await _dbContext.BookReservations.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.Status, Id = x.Id }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetLibraryFineRulesAsync()
        {
            var entity = await _dbContext.LibraryFineRules.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.Id.ToString(), Id = x.Id }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetDigitalResourcesAsync()
        {
            var entity = await _dbContext.DigitalResources.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.Title, Id = x.Id }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetCampusesAsync()
        {
            var entity = await _dbContext.Campuses.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.Name, Id = x.Id, Code = x.Code }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetEducationLevelsAsync()
        {
            var entity = await _dbContext.EducationLevels.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.Name, Id = x.Id, Code = x.Code }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetYearSemestersAsync()
        {
            var entity = await _dbContext.YearSemesters.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.Name, Id = x.Id, Code = x.Code }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetProgramsAsync()
        {
            var entity = await _dbContext.Programs.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.Name, Id = x.Id, Code = x.Code }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetBranchesAsync()
        {
            var entity = await _dbContext.Branches.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.Name, Id = x.Id, Code = x.Code }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetBatchesAsync()
        {
            var entity = await _dbContext.Batches.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.Name, Id = x.Id, Code = x.Code }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetAdmissionFormConfigsAsync()
        {
            var entity = await _dbContext.AdmissionFormConfigs.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.FormStepsJson, Id = x.Id }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetAdmissionRulesAsync()
        {
            var entity = await _dbContext.AdmissionRules.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.RuleName, Id = x.Id }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetFeeStructuresAsync()
        {
            var entity = await _dbContext.FeeStructures.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.Name, Id = x.Id }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetFeeStructureItemsAsync()
        {
            var entity = await _dbContext.FeeStructureItems.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.Id.ToString(), Id = x.Id }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetAdmissionApplicationsAsync()
        {
            var entity = await _dbContext.AdmissionApplications.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.FullName, Id = x.Id }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetParentStudentMappingsAsync()
        {
            var entity = await _dbContext.ParentStudentMappings.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.ParentUserId, Id = x.Id }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetSubjectEnrollmentsAsync()
        {
            var entity = await _dbContext.SubjectEnrollments.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.Status, Id = x.Id }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetStudentAttendancesAsync()
        {
            var entity = await _dbContext.StudentAttendances.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.Status, Id = x.Id }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetTimetablePeriodsAsync()
        {
            var entity = await _dbContext.TimetablePeriods.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.StartTime, Id = x.Id }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetHomeworksAsync()
        {
            var entity = await _dbContext.Homeworks.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.Title, Id = x.Id }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetHomeworkSubmissionsAsync()
        {
            var entity = await _dbContext.HomeworkSubmissions.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.Id.ToString(), Id = x.Id }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetAssignmentsAsync()
        {
            var entity = await _dbContext.Assignments.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.Title, Id = x.Id }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetAssignmentSubmissionsAsync()
        {
            var entity = await _dbContext.AssignmentSubmissions.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.Id.ToString(), Id = x.Id }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetOnlineClassesAsync()
        {
            var entity = await _dbContext.OnlineClasses.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.Title, Id = x.Id }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetSyllabusChaptersAsync()
        {
            var entity = await _dbContext.SyllabusChapters.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.Id.ToString(), Id = x.Id }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetLessonPlansAsync()
        {
            var entity = await _dbContext.LessonPlans.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.Id.ToString(), Id = x.Id }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetFeeInstallmentsAsync()
        {
            var entity = await _dbContext.FeeInstallments.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.InstallmentName, Id = x.Id }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetFeePaymentsAsync()
        {
            var entity = await _dbContext.FeePayments.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.PaymentMode, Id = x.Id }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetFeeFinesAsync()
        {
            var entity = await _dbContext.FeeFines.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.Id.ToString(), Id = x.Id }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetStudentScholarshipsAsync()
        {
            var entity = await _dbContext.StudentScholarships.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.Id.ToString(), Id = x.Id }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetFeeRefundsAsync()
        {
            var entity = await _dbContext.FeeRefunds.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.Id.ToString(), Id = x.Id }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetPaymentGatewaysAsync()
        {
            var entity = await _dbContext.PaymentGateways.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.GatewayName, Id = x.Id }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetOnlinePaymentOrdersAsync()
        {
            var entity = await _dbContext.OnlinePaymentOrders.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.Id.ToString(), Id = x.Id }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetFineRulesAsync()
        {
            var entity = await _dbContext.FineRules.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.FineType, Id = x.Id }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetExamSchedulesAsync()
        {
            var entity = await _dbContext.ExamSchedules.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.Id.ToString(), Id = x.Id }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetGradeConfigsAsync()
        {
            var entity = await _dbContext.GradeConfigs.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.Id.ToString(), Id = x.Id }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetReportCardsAsync()
        {
            var entity = await _dbContext.ReportCards.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.Id.ToString(), Id = x.Id }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetStudentPromotionsAsync()
        {
            var entity = await _dbContext.StudentPromotions.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.Id.ToString(), Id = x.Id }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetHostelsAsync()
        {
            var entity = await _dbContext.Hostels.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.Name, Id = x.Id, Code = x.Code }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetBuildingsAsync()
        {
            var entity = await _dbContext.Buildings.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.Name, Id = x.Id, Code = x.Code }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetFloorsAsync()
        {
            var entity = await _dbContext.Floors.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.Description, Id = x.Id }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetRoomCategoriesAsync()
        {
            var entity = await _dbContext.RoomCategories.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.Name, Id = x.Id }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetRoomsAsync()
        {
            var entity = await _dbContext.Rooms.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.RoomNumber, Id = x.Id, Code = x.RoomNumber }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetBedsAsync()
        {
            var entity = await _dbContext.Beds.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.BedNumber, Id = x.Id, Code = x.Barcode }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetHostelWardensAsync()
        {
            var entity = await _dbContext.HostelWardens.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.RoleType, Id = x.Id }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetHostelAdmissionsAsync()
        {
            var entity = await _dbContext.HostelAdmissions.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.AdmissionNumber, Id = x.Id }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetRoomTransferHistoriesAsync()
        {
            var entity = await _dbContext.RoomTransferHistories.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.Reason, Id = x.Id }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetBedReservationsAsync()
        {
            var entity = await _dbContext.BedReservations.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.Status, Id = x.Id }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetHostelFeeAllocationsAsync()
        {
            var entity = await _dbContext.HostelFeeAllocations.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.FeePlanName, Id = x.Id }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetHostelFeePaymentsAsync()
        {
            var entity = await _dbContext.HostelFeePayments.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.PaymentMode, Id = x.Id }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetMessMenusAsync()
        {
            var entity = await _dbContext.MessMenus.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.DayOfWeek, Id = x.Id }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetMealAttendancesAsync()
        {
            var entity = await _dbContext.MealAttendances.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.MealType, Id = x.Id }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetHostelVisitorsAsync()
        {
            var entity = await _dbContext.HostelVisitors.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.VisitorName, Id = x.Id }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetHostelGatePassesAsync()
        {
            var entity = await _dbContext.HostelGatePasses.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.Reason, Id = x.Id }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetHostelAttendancesAsync()
        {
            var entity = await _dbContext.HostelAttendances.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.AttendanceType, Id = x.Id }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetHostelComplaintsAsync()
        {
            var entity = await _dbContext.HostelComplaints.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.Category, Id = x.Id }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetHostelMaintenancesAsync()
        {
            var entity = await _dbContext.HostelMaintenances.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.Description, Id = x.Id }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetLaundryTransactionsAsync()
        {
            var entity = await _dbContext.LaundryTransactions.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.TokenNumber, Id = x.Id }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetHostelInventoriesAsync()
        {
            var entity = await _dbContext.HostelInventories.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.AssetName, Id = x.Id, Code = x.Barcode }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetHostelMedicalLogsAsync()
        {
            var entity = await _dbContext.HostelMedicalLogs.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.IncidentDescription, Id = x.Id }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetHostelDisciplinesAsync()
        {
            var entity = await _dbContext.HostelDisciplines.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.Offense, Id = x.Id }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetCoaAccountsAsync()
        {
            var entity = await _dbContext.CoaAccounts.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.Name, Id = x.Id, Code = x.Code }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetJournalEntriesAsync()
        {
            var entity = await _dbContext.JournalEntries.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.VoucherNo, Id = x.Id }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetJournalEntryLinesAsync()
        {
            var entity = await _dbContext.JournalEntryLines.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.Description, Id = x.Id }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetCashBankTransactionsAsync()
        {
            var entity = await _dbContext.CashBankTransactions.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.TransactionType, Id = x.Id }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetBudgetPlansAsync()
        {
            var entity = await _dbContext.BudgetPlans.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.FinancialYear, Id = x.Id }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetTaxConfigsAsync()
        {
            var entity = await _dbContext.TaxConfigs.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.TaxName, Id = x.Id }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetFinancialYearsAsync()
        {
            var entity = await _dbContext.FinancialYears.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.YearName, Id = x.Id }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetCostCentersAsync()
        {
            var entity = await _dbContext.CostCenters.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.Name, Id = x.Id, Code = x.Code }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetChequeBooksAsync()
        {
            var entity = await _dbContext.ChequeBooks.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.SeriesPrefix, Id = x.Id }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetItemCategoriesAsync()
        {
            var entity = await _dbContext.ItemCategories.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.Name, Id = x.Id }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetInventoryItemsAsync()
        {
            var entity = await _dbContext.InventoryItems.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.Name, Id = x.Id }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetVendorsAsync()
        {
            var entity = await _dbContext.Vendors.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.Name, Id = x.Id, Code = x.Code }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetPurchaseRequisitionsAsync()
        {
            var entity = await _dbContext.PurchaseRequisitions.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.RequisitionNo, Id = x.Id }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetPurchaseRequisitionItemsAsync()
        {
            var entity = await _dbContext.PurchaseRequisitionItems.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.Id.ToString(), Id = x.Id }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetPurchaseOrdersAsync()
        {
            var entity = await _dbContext.PurchaseOrders.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.PoNumber, Id = x.Id }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetPurchaseOrderItemsAsync()
        {
            var entity = await _dbContext.PurchaseOrderItems.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.Id.ToString(), Id = x.Id }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetGoodsReceiptNotesAsync()
        {
            var entity = await _dbContext.GoodsReceiptNotes.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.GrnNumber, Id = x.Id }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetGoodsReceiptNoteItemsAsync()
        {
            var entity = await _dbContext.GoodsReceiptNoteItems.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.Id.ToString(), Id = x.Id }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetStockTransactionsAsync()
        {
            var entity = await _dbContext.StockTransactions.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.TransactionType, Id = x.Id }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetWarehousesAsync()
        {
            var entity = await _dbContext.Warehouses.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.Name, Id = x.Id, Code = x.Code }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetWarehouseBinsAsync()
        {
            var entity = await _dbContext.WarehouseBins.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.Zone, Id = x.Id }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetStoresAsync()
        {
            var entity = await _dbContext.Stores.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.Name, Id = x.Id, Code = x.Code }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetRequestForQuotationsAsync()
        {
            var entity = await _dbContext.RequestForQuotations.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.RfqNo, Id = x.Id }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetVendorQuotationsAsync()
        {
            var entity = await _dbContext.VendorQuotations.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.Status, Id = x.Id }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetPurchaseReturnsAsync()
        {
            var entity = await _dbContext.PurchaseReturns.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.ReturnNo, Id = x.Id }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetStockIssuesAsync()
        {
            var entity = await _dbContext.StockIssues.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.IssueNo, Id = x.Id }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetQualityInspectionsAsync()
        {
            var entity = await _dbContext.QualityInspections.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.InspectionReport, Id = x.Id }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetNoticeBoardsAsync()
        {
            var entity = await _dbContext.NoticeBoards.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.Title, Id = x.Id }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetCircularsAsync()
        {
            var entity = await _dbContext.Circulars.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.CircularNo, Id = x.Id }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetPushNotificationsAsync()
        {
            var entity = await _dbContext.PushNotifications.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.Title, Id = x.Id }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetParentTeacherChatsAsync()
        {
            var entity = await _dbContext.ParentTeacherChats.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.SenderUserId, Id = x.Id }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetFeedbackSurveysAsync()
        {
            var entity = await _dbContext.FeedbackSurveys.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.Title, Id = x.Id }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetSurveyQuestionsAsync()
        {
            var entity = await _dbContext.SurveyQuestions.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.QuestionText, Id = x.Id }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetSurveyResponsesAsync()
        {
            var entity = await _dbContext.SurveyResponses.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.RespondentUserId, Id = x.Id }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetDashboardConfigsAsync()
        {
            var entity = await _dbContext.DashboardConfigs.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.Name, Id = x.Id }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetDashboardWidgetsAsync()
        {
            var entity = await _dbContext.DashboardWidgets.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.Title, Id = x.Id }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetReportTemplatesAsync()
        {
            var entity = await _dbContext.ReportTemplates.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.Name, Id = x.Id }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetAnalyticsKpisAsync()
        {
            var entity = await _dbContext.AnalyticsKpis.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.Name, Id = x.Id, Code = x.Code }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetSchoolBranchesAsync()
        {
            var entity = await _dbContext.SchoolBranches.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.BranchName, Id = x.Id, Code = x.Code }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetWorkflowDefinitionsAsync()
        {
            var entity = await _dbContext.WorkflowDefinitions.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.Name, Id = x.Id }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetWorkflowStepsAsync()
        {
            var entity = await _dbContext.WorkflowSteps.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.ApproverRole, Id = x.Id }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetWorkflowInstancesAsync()
        {
            var entity = await _dbContext.WorkflowInstances.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.TargetEntityName, Id = x.Id }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetApprovalLogsAsync()
        {
            var entity = await _dbContext.ApprovalLogs.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.ApprovedByUserId, Id = x.Id }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetAiPredictionsAsync()
        {
            var entity = await _dbContext.AiPredictions.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.PredictionType, Id = x.Id }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetAiGenerationsAsync()
        {
            var entity = await _dbContext.AiGenerations.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.GenerationType, Id = x.Id }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetAiChatSessionsAsync()
        {
            var entity = await _dbContext.AiChatSessions.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.UserId, Id = x.Id }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetAiChatMessagesAsync()
        {
            var entity = await _dbContext.AiChatMessages.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.Sender, Id = x.Id }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }
    }
}

