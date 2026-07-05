using System;
using System.IO;

class Program
{
    static void Main()
    {
        var entities = new[] {
            new { Name="EmployeeBankDetail", Props="public string BankName { get; set; } = null!; public string AccountNumber { get; set; } = null!; public string IfscCode { get; set; } = null!; public string? Branch { get; set; }" },
            new { Name="EmployeeDetail", Props="public string? FatherName { get; set; } public string? MotherName { get; set; } public string? SpouseName { get; set; } public string? AadhaarNumber { get; set; } public string? PanNumber { get; set; }" },
            new { Name="EmployeeDocument", Props="public string DocumentName { get; set; } = null!; public string DocumentType { get; set; } = null!; public string FilePath { get; set; } = null!;" },
            new { Name="EmployeeEducation", Props="public string Degree { get; set; } = null!; public string? Board { get; set; } public string University { get; set; } = null!; public string? PassingYear { get; set; } public decimal? Percentage { get; set; }" },
            new { Name="EmployeeExperience", Props="public string Company { get; set; } = null!; public string Designation { get; set; } = null!; public DateTime JoiningDate { get; set; } public DateTime LeavingDate { get; set; } public decimal? Salary { get; set; }" },
            new { Name="EmployeeSalaryDetail", Props="public decimal Basic { get; set; } public decimal HRA { get; set; } public decimal DA { get; set; } public decimal PF { get; set; } public decimal ESI { get; set; } public decimal NetSalary { get; set; }" }
        };

        string basePath = @"E:\GIT\SchoolSAAS\SchoolWeb_Api";

        foreach (var e in entities)
        {
            string name = e.Name;
            string props = e.Props;

            // DTO
            string dtoContent = $@"using System;
namespace School_DTOs.Hr
{{
    public class {name}Dto
    {{
        public int Id {{ get; set; }}
        public int EmployeeId {{ get; set; }}
        {props}
    }}

    public class Create{name}Dto
    {{
        public int EmployeeId {{ get; set; }}
        {props}
    }}

    public class Update{name}Dto : Create{name}Dto
    {{
        public int Id {{ get; set; }}
    }}
}}";
            File.WriteAllText(Path.Combine(basePath, "School_DTOs", "Hr", $"{name}Dto.cs"), dtoContent);

            // Interface
            string interfaceContent = $@"using School_DTOs.Common;
using School_DTOs.Hr;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace School.Services.Interfaces.Hr
{{
    public interface I{name}Service
    {{
        Task<APIResponse<List<{name}Dto>>> GetAllByEmployeeIdAsync(int employeeId);
        Task<APIResponse<{name}Dto>> GetByIdAsync(int id);
        Task<APIResponse<object>> CreateAsync(Create{name}Dto dto, string username);
        Task<APIResponse<object>> UpdateAsync(int id, Update{name}Dto dto, string username);
        Task<APIResponse<object>> DeleteAsync(int id, string username);
    }}
}}";
            File.WriteAllText(Path.Combine(basePath, "School.Services", "Interfaces", "Hr", $"I{name}Service.cs"), interfaceContent);

            // Controller
            string controllerContent = $@"using Microsoft.AspNetCore.Mvc;
using School.Services.Interfaces.Hr;
using School_API.Common.Interface;
using School_DTOs.Common;
using School_DTOs.Hr;
using System.Threading.Tasks;

namespace School_API.Controllers.Hr
{{
    [Route(""api/[controller]"")]
    [ApiController]
    public class {name}Controller : BaseController
    {{
        private readonly I{name}Service _service;

        public {name}Controller(I{name}Service service, ICurrentUserService currentUserService) : base(currentUserService)
        {{
            _service = service;
        }}

        [HttpGet(""employee/{{employeeId}}"")]
        public async Task<IActionResult> GetAllByEmployeeId(int employeeId)
        {{
            var result = await _service.GetAllByEmployeeIdAsync(employeeId);
            if (result.Success) return Ok(result);
            return StatusCode((int)result.StatusCode, result);
        }}

        [HttpGet(""{{id}}"")]
        public async Task<IActionResult> GetById(int id)
        {{
            var result = await _service.GetByIdAsync(id);
            if (result.Success) return Ok(result);
            return StatusCode((int)result.StatusCode, result);
        }}

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Create{name}Dto model)
        {{
            var result = await _service.CreateAsync(model, UserName);
            if (result.Success) return StatusCode((int)result.StatusCode, result);
            return StatusCode((int)result.StatusCode, result);
        }}

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] Update{name}Dto model)
        {{
            var result = await _service.UpdateAsync(model.Id, model, UserName);
            if (result.Success) return Ok(result);
            return StatusCode((int)result.StatusCode, result);
        }}

        [HttpDelete(""{{id}}"")]
        public async Task<IActionResult> Delete(int id)
        {{
            var result = await _service.DeleteAsync(id, UserName);
            if (result.Success) return Ok(result);
            return StatusCode((int)result.StatusCode, result);
        }}
    }}
}}";
            File.WriteAllText(Path.Combine(basePath, "School_API", "Controllers", "Hr", $"{name}Controller.cs"), controllerContent);
        }
    }
}
