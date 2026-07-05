using System;
using System.IO;

class Program
{
    static void Main()
    {
        var entities = new[] {
            new { Name="EmployeeBankDetail", Props="BankName = dto.BankName, AccountNumber = dto.AccountNumber, IfscCode = dto.IfscCode, Branch = dto.Branch", DtoInit="BankName = x.BankName, AccountNumber = x.AccountNumber, IfscCode = x.IfscCode, Branch = x.Branch" },
            new { Name="EmployeeDetail", Props="FatherName = dto.FatherName, MotherName = dto.MotherName, SpouseName = dto.SpouseName, AadhaarNumber = dto.AadhaarNumber, PanNumber = dto.PanNumber", DtoInit="FatherName = x.FatherName, MotherName = x.MotherName, SpouseName = x.SpouseName, AadhaarNumber = x.AadhaarNumber, PanNumber = x.PanNumber" },
            new { Name="EmployeeDocument", Props="DocumentName = dto.DocumentName, DocumentType = dto.DocumentType, FilePath = dto.FilePath", DtoInit="DocumentName = x.DocumentName, DocumentType = x.DocumentType, FilePath = x.FilePath" },
            new { Name="EmployeeEducation", Props="Degree = dto.Degree, Board = dto.Board, University = dto.University, PassingYear = dto.PassingYear, Percentage = dto.Percentage", DtoInit="Degree = x.Degree, Board = x.Board, University = x.University, PassingYear = x.PassingYear, Percentage = x.Percentage" },
            new { Name="EmployeeExperience", Props="Company = dto.Company, Designation = dto.Designation, JoiningDate = dto.JoiningDate, LeavingDate = dto.LeavingDate, Salary = dto.Salary", DtoInit="Company = x.Company, Designation = x.Designation, JoiningDate = x.JoiningDate, LeavingDate = x.LeavingDate, Salary = x.Salary" },
            new { Name="EmployeeSalaryDetail", Props="Basic = dto.Basic, HRA = dto.HRA, DA = dto.DA, PF = dto.PF, ESI = dto.ESI, NetSalary = dto.NetSalary", DtoInit="Basic = x.Basic, HRA = x.HRA, DA = x.DA, PF = x.PF, ESI = x.ESI, NetSalary = x.NetSalary" }
        };

        string basePath = @"E:\GIT\SchoolSAAS\SchoolWeb_Api";

        foreach (var e in entities)
        {
            string name = e.Name;

            string serviceContent = $@"using Microsoft.EntityFrameworkCore;
using School.Domain;
using School.Infrastructure.Repositories.IRepositories;
using School.Infrastructure.UnitOfWork.Interfaces;
using School.Services.Interfaces.Hr;
using School_DTOs.Common;
using School_DTOs.Hr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace School.Services.Hr
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

        public async Task<APIResponse<List<{name}Dto>>> GetAllByEmployeeIdAsync(int employeeId)
        {{
            var data = await _repository.GetAll().Where(x => x.EmployeeId == employeeId).Select(x => new {name}Dto
            {{
                Id = x.Id,
                EmployeeId = x.EmployeeId,
                {e.DtoInit}
            }}).ToListAsync();

            return new APIResponse<List<{name}Dto>>(HttpStatusCode.OK, ""Success"", data);
        }}

        public async Task<APIResponse<{name}Dto>> GetByIdAsync(int id)
        {{
            var data = await _repository.GetAll().Where(x => x.Id == id).Select(x => new {name}Dto
            {{
                Id = x.Id,
                EmployeeId = x.EmployeeId,
                {e.DtoInit}
            }}).FirstOrDefaultAsync();

            if (data == null) return new APIResponse<{name}Dto>(HttpStatusCode.NotFound, ""Not found"");
            return new APIResponse<{name}Dto>(HttpStatusCode.OK, ""Success"", data);
        }}

        public async Task<APIResponse<object>> CreateAsync(Create{name}Dto dto, string username)
        {{
            var entity = new {name}
            {{
                EmployeeId = dto.EmployeeId,
                {e.Props},
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

            entity.EmployeeId = dto.EmployeeId;
            {string.Join(";\n            ", e.Props.Split(new[]{{ ", " }}, StringSplitOptions.None).Select(x => "entity." + x))};
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
}}";
            File.WriteAllText(Path.Combine(basePath, "School.Services", "Hr", $"{name}Service.cs"), serviceContent);
        }
    }
}
