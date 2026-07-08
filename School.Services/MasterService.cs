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

        // ─── HR Master dropdowns ─────────────────────────────────────────────────

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
                .Select(x => new DropdownDto { Name = x.SchoolAsset.Name + " → " + x.Employee.FirstName + " " + x.Employee.LastName, Id = x.Id }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        // ─── Academic ────────────────────────────────────────────────────────────

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetSubjectsAsync()
        {
            var entity = await _dbContext.Subjects.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.Name, Id = x.Id, Code = x.Code }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        // ─── Payroll ─────────────────────────────────────────────────────────────

        public async Task<APIResponse<IEnumerable<DropdownDto>>> GetSalaryComponentsAsync()
        {
            var entity = await _dbContext.SalaryComponents.Where(x => !x.IsDeleted).Select(x => new DropdownDto { Name = x.Name, Id = x.Id }).ToListAsync();
            return new APIResponse<IEnumerable<DropdownDto>> { Data = entity, Message = CommonResource.FetchSuccess, Success = true, StatusCode = HttpStatusCode.OK };
        }

        // ─── Transport ───────────────────────────────────────────────────────────

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
    }
}
