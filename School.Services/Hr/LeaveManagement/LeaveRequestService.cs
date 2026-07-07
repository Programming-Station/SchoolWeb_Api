using Microsoft.EntityFrameworkCore;
using School.Infrastructure;
using School.Infrastructure.Repositories.IRepositories;
using School.Infrastructure.UnitOfWork.Interfaces;
using School.Services.Interfaces;
using School.Services.Interfaces.Hr.LeaveManagement;
using School_DTOs;
using School_DTOs.Common;
using School_DTOs.Hr.LeaveManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace School.Services.Hr.LeaveManagement
{
    public class LeaveRequestService : ILeaveRequestService
    {
        private readonly IRepository<global::School.Domain.Hr.LeaveManagement.LeaveRequest> _repository;
        private readonly IRepository<global::School.Domain.Hr.LeaveManagement.LeaveBalance> _leaveBalanceRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailService _emailService;
        private readonly SchoolDbContext _dbContext;

        public LeaveRequestService(
            IRepository<global::School.Domain.Hr.LeaveManagement.LeaveRequest> repository,
            IRepository<global::School.Domain.Hr.LeaveManagement.LeaveBalance> leaveBalanceRepository,
            IUnitOfWork unitOfWork,
            IEmailService emailService,
            SchoolDbContext dbContext)
        {
            _repository = repository;
            _leaveBalanceRepository = leaveBalanceRepository;
            _unitOfWork = unitOfWork;
            _emailService = emailService;
            _dbContext = dbContext;
        }

        public async Task<APIResponse<List<LeaveRequestDto>>> GetAllByEmployeeIdAsync(int fkId)
        {
            var data = await _repository.List().Where(x => x.EmployeeId == fkId).Select(x => new LeaveRequestDto
            {
                Id = x.Id,
                EmployeeId = x.EmployeeId,
                LeaveTypeId = x.LeaveTypeId,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                TotalDays = x.TotalDays,
                Reason = x.Reason,
                Status = x.Status,
                ApprovedById = x.ApprovedById,
                Remarks = x.Remarks
            }).ToListAsync();

            return new APIResponse<List<LeaveRequestDto>> { StatusCode = HttpStatusCode.OK, Message = "Success", Data = data };
        }

        public async Task<APIResponse<LeaveRequestDto>> GetByIdAsync(int id)
        {
            var data = await _repository.List().Where(x => x.Id == id).Select(x => new LeaveRequestDto
            {
                Id = x.Id,
                EmployeeId = x.EmployeeId,
                LeaveTypeId = x.LeaveTypeId,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                TotalDays = x.TotalDays,
                Reason = x.Reason,
                Status = x.Status,
                ApprovedById = x.ApprovedById,
                Remarks = x.Remarks
            }).FirstOrDefaultAsync();

            if (data == null) return new APIResponse<LeaveRequestDto> { StatusCode = HttpStatusCode.NotFound, Message = "Not found" };
            return new APIResponse<LeaveRequestDto> { StatusCode = HttpStatusCode.OK, Message = "Success", Data = data };
        }

        public async Task<APIResponse<object>> CreateAsync(CreateLeaveRequestDto dto, string username)
        {
            var entity = new global::School.Domain.Hr.LeaveManagement.LeaveRequest
            {
                EmployeeId = dto.EmployeeId,
                LeaveTypeId = dto.LeaveTypeId,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                TotalDays = dto.TotalDays,
                Reason = dto.Reason,
                Status = "Pending",
                ApprovedById = dto.ApprovedById,
                Remarks = dto.Remarks,
                CreatedBy = username,
                CreatedDate = DateTime.UtcNow
            };

            await _repository.AddAsync(entity);
            await _unitOfWork.CommitAsync();

            // Send Leave Applied email
            var employee = await _dbContext.Employees
                .Include(e => e.Department)
                .Include(e => e.LeaveRequests.Where(lr => lr.LeaveTypeId == dto.LeaveTypeId))
                .FirstOrDefaultAsync(e => e.Id == dto.EmployeeId);

            var leaveType = await _dbContext.Set<global::School.Domain.Hr.LeaveManagement.LeaveType>()
                .FirstOrDefaultAsync(lt => lt.Id == dto.LeaveTypeId);

            if (employee != null && !string.IsNullOrWhiteSpace(employee.Email))
            {
                _ = _emailService.SendGenericTemplateAsync(employee.Email, "Leave Applied", new Dictionary<string, string>
                {
                    { "SchoolName", "School" },
                    { "FirstName", employee.FirstName },
                    { "LastName", employee.LastName },
                    { "EmployeeCode", employee.EmployeeCode },
                    { "Department", employee.Department?.Name ?? "-" },
                    { "LeaveType", leaveType?.Name ?? dto.LeaveTypeId.ToString() },
                    { "StartDate", dto.StartDate.ToString("dd MMM yyyy") },
                    { "EndDate", dto.EndDate.ToString("dd MMM yyyy") },
                    { "TotalDays", dto.TotalDays.ToString("F1") },
                    { "Reason", dto.Reason ?? "-" },
                    { "LoginUrl", "#" }
                });
            }

            return new APIResponse<object> { StatusCode = HttpStatusCode.OK, Message = "Created successfully" };
        }

        public async Task<APIResponse<object>> UpdateAsync(int id, UpdateLeaveRequestDto dto, string username)
        {
            if (id != dto.Id) return new APIResponse<object> { StatusCode = HttpStatusCode.BadRequest, Message = "Id mismatch" };

            var entity = await _repository.List().Where(x => x.Id == id).FirstOrDefaultAsync();
            if (entity == null) return new APIResponse<object> { StatusCode = HttpStatusCode.NotFound, Message = "Not found" };

            entity.EmployeeId = dto.EmployeeId;
            entity.LeaveTypeId = dto.LeaveTypeId;
            entity.StartDate = dto.StartDate;
            entity.EndDate = dto.EndDate;
            entity.TotalDays = dto.TotalDays;
            entity.Reason = dto.Reason;
            entity.Status = dto.Status;
            entity.ApprovedById = dto.ApprovedById;
            entity.Remarks = dto.Remarks;
            entity.UpdatedBy = username;
            entity.UpdatedDate = DateTime.UtcNow;

            _repository.Update(entity);
            await _unitOfWork.CommitAsync();
            return new APIResponse<object> { StatusCode = HttpStatusCode.OK, Message = "Updated successfully" };
        }

        public async Task<APIResponse<object>> DeleteAsync(int id, string username)
        {
            var entity = await _repository.List().Where(x => x.Id == id).FirstOrDefaultAsync();
            if (entity == null) return new APIResponse<object> { StatusCode = HttpStatusCode.NotFound, Message = "Not found" };
            _repository.Delete(entity);
            await _unitOfWork.CommitAsync();
            return new APIResponse<object> { StatusCode = HttpStatusCode.OK, Message = "Deleted successfully" };
        }

        public async Task<APIResponse<object>> ApproveLeaveAsync(int id, int approverEmployeeId, string username)
        {
            var entity = await _repository.List()
                .Include(x => x.Employee)
                .Include(x => x.LeaveType)
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            if (entity == null) return new APIResponse<object> { StatusCode = HttpStatusCode.NotFound, Message = "Leave request not found" };
            if (entity.Status != "Pending") return new APIResponse<object> { StatusCode = HttpStatusCode.BadRequest, Message = "Only pending requests can be approved" };

            var leaveBalance = await _leaveBalanceRepository.List()
                .Where(x => x.EmployeeId == entity.EmployeeId && x.LeaveTypeId == entity.LeaveTypeId && x.Year == DateTime.UtcNow.Year.ToString())
                .FirstOrDefaultAsync();

            if (leaveBalance == null || leaveBalance.AvailableLeaves < entity.TotalDays)
                return new APIResponse<object> { StatusCode = HttpStatusCode.BadRequest, Message = "Insufficient leave balance" };

            leaveBalance.UsedLeaves += entity.TotalDays;
            leaveBalance.AvailableLeaves -= entity.TotalDays;
            _leaveBalanceRepository.Update(leaveBalance);

            entity.Status = "Approved";
            entity.ApprovedById = approverEmployeeId;
            entity.UpdatedBy = username;
            entity.UpdatedDate = DateTime.UtcNow;

            _repository.Update(entity);
            await _unitOfWork.CommitAsync();

            // Get approver name
            var approver = await _dbContext.Employees.FindAsync(approverEmployeeId);

            // Send Leave Approved email
            if (entity.Employee != null && !string.IsNullOrWhiteSpace(entity.Employee.Email))
            {
                _ = _emailService.SendGenericTemplateAsync(entity.Employee.Email, "Leave Approved", new Dictionary<string, string>
                {
                    { "SchoolName", "School" },
                    { "FirstName", entity.Employee.FirstName },
                    { "LastName", entity.Employee.LastName },
                    { "EmployeeCode", entity.Employee.EmployeeCode },
                    { "LeaveType", entity.LeaveType?.Name ?? "-" },
                    { "StartDate", entity.StartDate.ToString("dd MMM yyyy") },
                    { "EndDate", entity.EndDate.ToString("dd MMM yyyy") },
                    { "TotalDays", entity.TotalDays.ToString("F1") },
                    { "ApprovedBy", approver != null ? $"{approver.FirstName} {approver.LastName}" : username },
                    { "LoginUrl", "#" }
                });
            }

            return new APIResponse<object> { StatusCode = HttpStatusCode.OK, Message = "Leave approved successfully" };
        }

        public async Task<APIResponse<object>> RejectLeaveAsync(int id, int approverEmployeeId, string reason, string username)
        {
            var entity = await _repository.List()
                .Include(x => x.Employee)
                .Include(x => x.LeaveType)
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            if (entity == null) return new APIResponse<object> { StatusCode = HttpStatusCode.NotFound, Message = "Leave request not found" };
            if (entity.Status != "Pending") return new APIResponse<object> { StatusCode = HttpStatusCode.BadRequest, Message = "Only pending requests can be rejected" };

            entity.Status = "Rejected";
            entity.ApprovedById = approverEmployeeId;
            entity.Remarks = reason;
            entity.UpdatedBy = username;
            entity.UpdatedDate = DateTime.UtcNow;

            _repository.Update(entity);
            await _unitOfWork.CommitAsync();

            // Send Leave Rejected email
            if (entity.Employee != null && !string.IsNullOrWhiteSpace(entity.Employee.Email))
            {
                _ = _emailService.SendGenericTemplateAsync(entity.Employee.Email, "Leave Rejected", new Dictionary<string, string>
                {
                    { "SchoolName", "School" },
                    { "FirstName", entity.Employee.FirstName },
                    { "LastName", entity.Employee.LastName },
                    { "EmployeeCode", entity.Employee.EmployeeCode },
                    { "LeaveType", entity.LeaveType?.Name ?? "-" },
                    { "StartDate", entity.StartDate.ToString("dd MMM yyyy") },
                    { "EndDate", entity.EndDate.ToString("dd MMM yyyy") },
                    { "TotalDays", entity.TotalDays.ToString("F1") },
                    { "Remarks", reason },
                    { "LoginUrl", "#" }
                });
            }

            return new APIResponse<object> { StatusCode = HttpStatusCode.OK, Message = "Leave rejected successfully" };
        }
    }
}