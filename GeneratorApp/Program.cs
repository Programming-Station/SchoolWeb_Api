using System;
using System.IO;
using System.Linq;

class Program
{
    static void Main()
    {
        var entities = new[] {
            new { Name="Attendance", Props="public DateTime AttendanceDate { get; set; } public TimeSpan? CheckInTime { get; set; } public TimeSpan? CheckOutTime { get; set; } public string Status { get; set; } = \"Present\"; public string? Remarks { get; set; }", Assigns="AttendanceDate = dto.AttendanceDate, CheckInTime = dto.CheckInTime, CheckOutTime = dto.CheckOutTime, Status = dto.Status, Remarks = dto.Remarks", DtoInit="AttendanceDate = x.AttendanceDate, CheckInTime = x.CheckInTime, CheckOutTime = x.CheckOutTime, Status = x.Status, Remarks = x.Remarks", Ns="Attendance", EmployeeIdField="EmployeeId" },
            new { Name="AttendanceLog", Props="public DateTime LogTime { get; set; } public string LogType { get; set; } = null!; public string? Source { get; set; } public string? DeviceId { get; set; }", Assigns="LogTime = dto.LogTime, LogType = dto.LogType, Source = dto.Source, DeviceId = dto.DeviceId", DtoInit="LogTime = x.LogTime, LogType = x.LogType, Source = x.Source, DeviceId = x.DeviceId", Ns="Attendance", EmployeeIdField="EmployeeId" },
            new { Name="LeaveBalance", Props="public int LeaveTypeId { get; set; } public string Year { get; set; } = null!; public decimal TotalLeaves { get; set; } = 0; public decimal UsedLeaves { get; set; } = 0; public decimal AvailableLeaves { get; set; } = 0;", Assigns="LeaveTypeId = dto.LeaveTypeId, Year = dto.Year, TotalLeaves = dto.TotalLeaves, UsedLeaves = dto.UsedLeaves, AvailableLeaves = dto.AvailableLeaves", DtoInit="LeaveTypeId = x.LeaveTypeId, Year = x.Year, TotalLeaves = x.TotalLeaves, UsedLeaves = x.UsedLeaves, AvailableLeaves = x.AvailableLeaves", Ns="LeaveManagement", EmployeeIdField="EmployeeId" },
            new { Name="LeaveRequest", Props="public int LeaveTypeId { get; set; } public DateTime StartDate { get; set; } public DateTime EndDate { get; set; } public decimal TotalDays { get; set; } public string? Reason { get; set; } public string Status { get; set; } = \"Pending\"; public int? ApprovedById { get; set; } public string? Remarks { get; set; }", Assigns="LeaveTypeId = dto.LeaveTypeId, StartDate = dto.StartDate, EndDate = dto.EndDate, TotalDays = dto.TotalDays, Reason = dto.Reason, Status = dto.Status, ApprovedById = dto.ApprovedById, Remarks = dto.Remarks", DtoInit="LeaveTypeId = x.LeaveTypeId, StartDate = x.StartDate, EndDate = x.EndDate, TotalDays = x.TotalDays, Reason = x.Reason, Status = x.Status, ApprovedById = x.ApprovedById, Remarks = x.Remarks", Ns="LeaveManagement", EmployeeIdField="EmployeeId" },
            new { Name="Timesheet", Props="public DateTime StartDate { get; set; } public DateTime EndDate { get; set; } public string Status { get; set; } = \"Draft\"; public int? ApprovedById { get; set; } public decimal TotalHours { get; set; } = 0;", Assigns="StartDate = dto.StartDate, EndDate = dto.EndDate, Status = dto.Status, ApprovedById = dto.ApprovedById, TotalHours = dto.TotalHours", DtoInit="StartDate = x.StartDate, EndDate = x.EndDate, Status = x.Status, ApprovedById = x.ApprovedById, TotalHours = x.TotalHours", Ns="Timesheet", EmployeeIdField="EmployeeId" },
            new { Name="TimesheetEntry", Props="public int TimesheetId { get; set; } public DateTime EntryDate { get; set; } public string TaskName { get; set; } = null!; public string? ProjectName { get; set; } public decimal HoursWorked { get; set; } = 0; public string? Description { get; set; }", Assigns="TimesheetId = dto.TimesheetId, EntryDate = dto.EntryDate, TaskName = dto.TaskName, ProjectName = dto.ProjectName, HoursWorked = dto.HoursWorked, Description = dto.Description", DtoInit="TimesheetId = x.TimesheetId, EntryDate = x.EntryDate, TaskName = x.TaskName, ProjectName = x.ProjectName, HoursWorked = x.HoursWorked, Description = x.Description", Ns="Timesheet", EmployeeIdField="TimesheetId" }
        };

        string basePath = @"E:\GIT\SchoolSAAS\SchoolWeb_Api";

        foreach (var e in entities)
        {
            string name = e.Name;
            
            // Build the update string manually
            var propsArray = e.Assigns.Split(new[] { ", " }, StringSplitOptions.None);
            string updateLines = string.Join(";\n            ", propsArray.Select(x => "entity." + x)) + ";";

            string dtoPath = Path.Combine(basePath, "School_DTOs", "Hr", e.Ns);
            Directory.CreateDirectory(dtoPath);
            File.WriteAllText(Path.Combine(dtoPath, $"{name}Dto.cs"), $@"using System;
namespace School_DTOs.Hr.{e.Ns}
{{
    public class {name}Dto
    {{
        public int Id {{ get; set; }}
        public int {e.EmployeeIdField} {{ get; set; }}
        {e.Props}
    }}

    public class Create{name}Dto
    {{
        public int {e.EmployeeIdField} {{ get; set; }}
        {e.Props}
    }}

    public class Update{name}Dto : Create{name}Dto
    {{
        public int Id {{ get; set; }}
    }}
}}");

            string ifacePath = Path.Combine(basePath, "School.Services", "Interfaces", "Hr", e.Ns);
            Directory.CreateDirectory(ifacePath);
            File.WriteAllText(Path.Combine(ifacePath, $"I{name}Service.cs"), $@"using School_DTOs.Common;
using School_DTOs.Hr.{e.Ns};
using System.Threading.Tasks;
using System.Collections.Generic;

namespace School.Services.Interfaces.Hr.{e.Ns}
{{
    public interface I{name}Service
    {{
        Task<APIResponse<List<{name}Dto>>> GetAllBy{e.EmployeeIdField}Async(int foreignKeyId);
        Task<APIResponse<{name}Dto>> GetByIdAsync(int id);
        Task<APIResponse<object>> CreateAsync(Create{name}Dto dto, string username);
        Task<APIResponse<object>> UpdateAsync(int id, Update{name}Dto dto, string username);
        Task<APIResponse<object>> DeleteAsync(int id, string username);
    }}
}}");

            string svcPath = Path.Combine(basePath, "School.Services", "Hr", e.Ns);
            Directory.CreateDirectory(svcPath);
            File.WriteAllText(Path.Combine(svcPath, $"{name}Service.cs"), $@"using Microsoft.EntityFrameworkCore;
using School.Domain.Hr.{e.Ns};
using School.Infrastructure.Repositories.IRepositories;
using School.Infrastructure.UnitOfWork.Interfaces;
using School.Services.Interfaces.Hr.{e.Ns};
using School_DTOs.Common;
using School_DTOs.Hr.{e.Ns};
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace School.Services.Hr.{e.Ns}
{{
    public class {name}Service : I{name}Service
    {{
        private readonly IRepository<{name}> _repository;
        private readonly IUnitOfWork _unitOfWork;

        public {name}Service(IRepository<{name}> repository, IUnitOfWork unitOfWork)
        {{
            _repository = repository;
            _unitOfWork = unitOfWork;
        }}

        public async Task<APIResponse<List<{name}Dto>>> GetAllBy{e.EmployeeIdField}Async(int foreignKeyId)
        {{
            var data = await _repository.GetAll().Where(x => x.{e.EmployeeIdField} == foreignKeyId).Select(x => new {name}Dto
            {{
                Id = x.Id,
                {e.EmployeeIdField} = x.{e.EmployeeIdField},
                {e.DtoInit}
            }}).ToListAsync();

            return new APIResponse<List<{name}Dto>>(HttpStatusCode.OK, ""Success"", data);
        }}

        public async Task<APIResponse<{name}Dto>> GetByIdAsync(int id)
        {{
            var data = await _repository.GetAll().Where(x => x.Id == id).Select(x => new {name}Dto
            {{
                Id = x.Id,
                {e.EmployeeIdField} = x.{e.EmployeeIdField},
                {e.DtoInit}
            }}).FirstOrDefaultAsync();

            if (data == null) return new APIResponse<{name}Dto>(HttpStatusCode.NotFound, ""Not found"");
            return new APIResponse<{name}Dto>(HttpStatusCode.OK, ""Success"", data);
        }}

        public async Task<APIResponse<object>> CreateAsync(Create{name}Dto dto, string username)
        {{
            var entity = new {name}
            {{
                {e.EmployeeIdField} = dto.{e.EmployeeIdField},
                {e.Assigns},
                CreatedBy = username,
                CreatedDate = DateTime.UtcNow
            }};
            await _repository.AddAsync(entity);
            await _unitOfWork.CommitAsync();
            return new APIResponse<object>(HttpStatusCode.OK, ""Created successfully"");
        }}

        public async Task<APIResponse<object>> UpdateAsync(int id, Update{name}Dto dto, string username)
        {{
            if (id != dto.Id) return new APIResponse<object>(HttpStatusCode.BadRequest, ""Id mismatch"");
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return new APIResponse<object>(HttpStatusCode.NotFound, ""Not found"");

            entity.{e.EmployeeIdField} = dto.{e.EmployeeIdField};
            {updateLines}
            entity.UpdatedBy = username;
            entity.UpdatedDate = DateTime.UtcNow;

            _repository.Update(entity);
            await _unitOfWork.CommitAsync();
            return new APIResponse<object>(HttpStatusCode.OK, ""Updated successfully"");
        }}

        public async Task<APIResponse<object>> DeleteAsync(int id, string username)
        {{
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return new APIResponse<object>(HttpStatusCode.NotFound, ""Not found"");
            _repository.Remove(entity);
            await _unitOfWork.CommitAsync();
            return new APIResponse<object>(HttpStatusCode.OK, ""Deleted successfully"");
        }}
    }}
}}");

            string ctlPath = Path.Combine(basePath, "School_API", "Controllers", "Hr", e.Ns);
            Directory.CreateDirectory(ctlPath);
            File.WriteAllText(Path.Combine(ctlPath, $"{name}Controller.cs"), $@"using Microsoft.AspNetCore.Mvc;
using School.Services.Interfaces.Hr.{e.Ns};
using School_API.Common.Interface;
using School_DTOs.Common;
using School_DTOs.Hr.{e.Ns};
using System.Threading.Tasks;

namespace School_API.Controllers.Hr.{e.Ns}
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

        [HttpGet(""parent/{{foreignKeyId}}"")]
        public async Task<IActionResult> GetAllByForeignKeyId(int foreignKeyId)
        {{
            var result = await _service.GetAllBy{e.EmployeeIdField}Async(foreignKeyId);
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
}}");
        }
    }
}
