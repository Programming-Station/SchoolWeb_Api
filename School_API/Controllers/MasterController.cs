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
        
    }

}
