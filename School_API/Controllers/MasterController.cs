using School.Models;
using School.Services.Interfaces;
using School.Utilities.Enums;
using School_API.Common.Interface;
using School_DTOs;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace School_API.Controllers
{
    public class MasterController : BaseController
    {
        private readonly IMasterService _masterService;
        public MasterController(IMasterService masterService, ICurrentUserService currentUser) : base(currentUser)
        {
            _masterService = masterService;
        }

        [HttpPost]
        public async Task<IActionResult> GetDropdownData([FromBody] DropDownModel model)
        {
            APIResponse<IEnumerable<DropdownDto>> res = new APIResponse<IEnumerable<DropdownDto>>();

            if (string.IsNullOrWhiteSpace(model.Table))
                return Ok(new APIResponse<List<DropdownDto>>
                {
                    Success = false,
                    Message = "Table parameter is required.",
                    StatusCode = System.Net.HttpStatusCode.BadRequest

                });

            SourceName dropdownType;

            if (!Enum.TryParse(model.Table, true, out dropdownType))
                return Ok(new APIResponse<List<DropdownDto>>
                {
                    Success = false,
                    Message = "Invalid dropdown type.",
                    StatusCode = System.Net.HttpStatusCode.BadRequest

                });

            switch (dropdownType)
            {
                case SourceName.State:
                    if (model.ParentId == null)
                        return Ok(new APIResponse<List<DropdownDto>>
                        {
                            Success = false,
                            Message = "countryId is required for states.",
                            StatusCode = System.Net.HttpStatusCode.BadRequest

                        });
                    res = await _masterService.GetStatesAsync(model.ParentId.Value);
                    break;

                case SourceName.City:
                    if (model.ParentId == null)
                        return Ok(new APIResponse<List<DropdownDto>>
                        {
                            Success = false,
                            Message = "stateId is required for cities.",
                            StatusCode = System.Net.HttpStatusCode.BadRequest

                        });
                    res = await _masterService.GetCitiesAsync(model.ParentId.Value);
                    break;

                case SourceName.Status:
                    res = await _masterService.GetStatusAsync();
                    break;

                case SourceName.Course:
                    if (model.ParentId == null)
                        return Ok(new APIResponse<List<DropdownDto>>
                        {
                            Success = false,
                            Message = "stateId is required for cities.",
                            StatusCode = System.Net.HttpStatusCode.BadRequest

                        });
                    res = await _masterService.GetCoursesAsync(model.ParentId.Value);
                    break;
                case SourceName.AcademicYear:
                    res = await _masterService.GetAcademicYearAsync();
                    break;
                case SourceName.AffiliationBoard:
                    res = await _masterService.GetAffiliationBoardsAsync();
                    break;
                case SourceName.SchoolType:
                    res = await _masterService.GetSchoolTypesAsync();
                    break;
                case SourceName.SchoolMedium:
                    res = await _masterService.GetSchoolMediumsAsync();
                    break;

                case SourceName.Country:
                    res = await _masterService.GetCountriesAsync();
                    break;
                case SourceName.Module:
                    res = await _masterService.GetModulesAsync();
                    break;
                case SourceName.Menu:
                    res = await _masterService.GetMenusAsync();
                    break;
                case SourceName.SubMenu:
                    if (model.ParentId == null)
                        return Ok(new APIResponse<List<DropdownDto>>
                        {
                            Success = false,
                            Message = "menuId is required for submenus.",
                            StatusCode = System.Net.HttpStatusCode.BadRequest
                        });
                    res = await _masterService.GetSubMenusAsync(model.ParentId.Value);
                    break;
                case SourceName.Class:
                    res = await _masterService.GetClassesAsync();
                    break;
                case SourceName.Department:
                    res = await _masterService.GetDepartmentsAsync();
                    break;
                case SourceName.FeeType:
                    res = await _masterService.GetFeeTypesAsync();
                    break;

                case SourceName.Faculty:
                    res = await _masterService.GetFacultiesAsync();
                    break;
                case SourceName.CategoryModule:
                    res = await _masterService.GetCategoryModulesAsync();
                    break;
                case SourceName.Affiliated:
                    res = await _masterService.GetAffiliatedsAsync();
                    break;

                default:
                    return Ok(new APIResponse<List<DropdownDto>>
                    {
                        Success = false,
                        Message = "Invalid dropdown type.",
                        StatusCode = System.Net.HttpStatusCode.BadRequest

                    });
            }

            return Ok(res);
        } 
        [HttpGet]
        public async Task<IActionResult> GetStates([Required] int countryId)
        {
            var res = await _masterService.GetStatesAsync(countryId);
            return Ok(res);
        }
        [HttpGet]
        public async Task<IActionResult> GetCities([Required] int stateId)
        {
            var res = await _masterService.GetCitiesAsync(stateId);
            return Ok(res);
        }

        [HttpGet]
        public async Task<IActionResult> GetStatues()
        {
            var res = await _masterService.GetStatusAsync();
            return Ok(res);
        }

        [HttpGet]
        public async Task<IActionResult> GetRoles()
        {
            var res = await _masterService.GetRolesAsync();
            return Ok(res);
        }         
        
        [HttpGet]
        public async Task<IActionResult> GetAffiliationBoards()
        {
            var res = await _masterService.GetAffiliationBoardsAsync();
            return Ok(res);
        }

        [HttpGet]
        public async Task<IActionResult> GetSchoolTypes()
        {
            var res = await _masterService.GetSchoolTypesAsync();
            return Ok(res);
        }

        [HttpGet]
        public async Task<IActionResult> GetSchoolMediums()
        {
            var res = await _masterService.GetSchoolMediumsAsync();
            return Ok(res);
        }

        [HttpGet]
        public async Task<IActionResult> GetCountries()
        {
            var res = await _masterService.GetCountriesAsync();
            return Ok(res);
        }

        [HttpGet]
        public async Task<IActionResult> GetModules()
        {
            var res = await _masterService.GetModulesAsync();
            return Ok(res);
        }

        [HttpGet]
        public async Task<IActionResult> GetMenus()
        {
            var res = await _masterService.GetMenusAsync();
            return Ok(res);
        }

        [HttpGet]
        public async Task<IActionResult> GetSubMenus([Required] int menuId)
        {
            var res = await _masterService.GetSubMenusAsync(menuId);
            return Ok(res);
        }

        [HttpGet]
        public async Task<IActionResult> GetClasses()
        {
            var res = await _masterService.GetClassesAsync();
            return Ok(res);
        }

        [HttpGet]
        public async Task<IActionResult> GetDepartments()
        {
            var res = await _masterService.GetDepartmentsAsync();
            return Ok(res);
        }

        [HttpGet]
        public async Task<IActionResult> GetFeeTypes()
        {
            var res = await _masterService.GetFeeTypesAsync();
            return Ok(res);
        }
        [HttpGet]
        public async Task<IActionResult> GetFaculties()
        {
            var res = await _masterService.GetFacultiesAsync();
            return Ok(res);
        }

        [HttpGet]
        public async Task<IActionResult> GetCategoryModules()
        {
            var res = await _masterService.GetCategoryModulesAsync();
            return Ok(res);
        }

        [HttpGet]
        public async Task<IActionResult> GetAffiliateds()
        {
            var res = await _masterService.GetAffiliatedsAsync();
            return Ok(res);
        }
    }

}
