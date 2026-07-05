$entities = @(
    @{ Name="EmployeeBankDetail"; Props="public string BankName { get; set; } = null!; public string AccountNumber { get; set; } = null!; public string IfscCode { get; set; } = null!; public string? Branch { get; set; }"; },
    @{ Name="EmployeeDetail"; Props="public string? FatherName { get; set; } public string? MotherName { get; set; } public string? SpouseName { get; set; } public string? AadhaarNumber { get; set; } public string? PanNumber { get; set; }"; },
    @{ Name="EmployeeDocument"; Props="public string DocumentName { get; set; } = null!; public string DocumentType { get; set; } = null!; public string FilePath { get; set; } = null!;"; },
    @{ Name="EmployeeEducation"; Props="public string Degree { get; set; } = null!; public string? Board { get; set; } public string University { get; set; } = null!; public string? PassingYear { get; set; } public decimal? Percentage { get; set; }"; },
    @{ Name="EmployeeExperience"; Props="public string Company { get; set; } = null!; public string Designation { get; set; } = null!; public DateTime JoiningDate { get; set; } public DateTime LeavingDate { get; set; } public decimal? Salary { get; set; }"; },
    @{ Name="EmployeeSalaryDetail"; Props="public decimal Basic { get; set; } public decimal HRA { get; set; } public decimal DA { get; set; } public decimal PF { get; set; } public decimal ESI { get; set; } public decimal NetSalary { get; set; }"; }
)

$basePath = "E:\GIT\SchoolSAAS\SchoolWeb_Api"

foreach ($e in $entities) {
    $name = $e.Name
    $props = $e.Props

    $dtoContent = "using System;
namespace School_DTOs.Hr
{
    public class Dto { public int Id { get; set; } public int EmployeeId { get; set; }  }
    public class CreateDto { public int EmployeeId { get; set; }  }
    public class UpdateDto : CreateDto { public int Id { get; set; } }
}"
    Set-Content "$basePath\School_DTOs\Hr\Dto.cs" $dtoContent

    $interfaceContent = "using School_DTOs.Common;
using School_DTOs.Hr;
using System.Threading.Tasks;
using System.Collections.Generic;
namespace School.Services.Interfaces.Hr
{
    public interface IService { Task<APIResponse<List<Dto>>> GetAllByEmployeeIdAsync(int employeeId); Task<APIResponse<Dto>> GetByIdAsync(int id); Task<APIResponse<object>> CreateAsync(CreateDto dto, string username); Task<APIResponse<object>> UpdateAsync(int id, UpdateDto dto, string username); Task<APIResponse<object>> DeleteAsync(int id, string username); }
}"
    Set-Content "$basePath\School.Services\Interfaces\Hr\IService.cs" $interfaceContent

    $controllerContent = "using Microsoft.AspNetCore.Mvc;
using School.Services.Interfaces.Hr;
using School_API.Common.Interface;
using School_DTOs.Common;
using School_DTOs.Hr;
using System.Threading.Tasks;
namespace School_API.Controllers.Hr
{
    [Route("api/[controller]")]
    [ApiController]
    public class Controller : BaseController
    {
        private readonly IService _service;
        public Controller(IService service, ICurrentUserService currentUserService) : base(currentUserService) { _service = service; }
        [HttpGet("employee/{employeeId}")] public async Task<IActionResult> GetAllByEmployeeId(int employeeId) { var result = await _service.GetAllByEmployeeIdAsync(employeeId); if (result.Success) return Ok(result); return StatusCode((int)result.StatusCode, result); }
        [HttpGet("{id}")] public async Task<IActionResult> GetById(int id) { var result = await _service.GetByIdAsync(id); if (result.Success) return Ok(result); return StatusCode((int)result.StatusCode, result); }
        [HttpPost] public async Task<IActionResult> Create([FromBody] CreateDto model) { var result = await _service.CreateAsync(model, UserName); if (result.Success) return StatusCode((int)result.StatusCode, result); return StatusCode((int)result.StatusCode, result); }
        [HttpPut] public async Task<IActionResult> Update([FromBody] UpdateDto model) { var result = await _service.UpdateAsync(model.Id, model, UserName); if (result.Success) return Ok(result); return StatusCode((int)result.StatusCode, result); }
        [HttpDelete("{id}")] public async Task<IActionResult> Delete(int id) { var result = await _service.DeleteAsync(id, UserName); if (result.Success) return Ok(result); return StatusCode((int)result.StatusCode, result); }
    }
}"
    Set-Content "$basePath\School_API\Controllers\Hr\Controller.cs" $controllerContent
}
