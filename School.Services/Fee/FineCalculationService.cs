using Microsoft.EntityFrameworkCore;
using School.Domain.FeeManagnment;
using School.Infrastructure;
using School.Infrastructure.Repositories.IRepositories;
using School.Infrastructure.UnitOfWork.Interfaces;
using School.Services.Interfaces;
using School_DTOs.Fee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace School.Services.Fee
{
    public class FineCalculationService : IFineCalculationService
    {
        private readonly IFineRuleRepository _fineRuleRepo;
        private readonly IFeeFineRepository _feeFineRepo;
        private readonly SchoolDbContext _dbContext;
        private readonly IUnitOfWork _uow;

        public FineCalculationService(
            IFineRuleRepository fineRuleRepo,
            IFeeFineRepository feeFineRepo,
            SchoolDbContext dbContext,
            IUnitOfWork uow)
        {
            _fineRuleRepo = fineRuleRepo;
            _feeFineRepo = feeFineRepo;
            _dbContext = dbContext;
            _uow = uow;
        }

        public async Task<(bool Success, string Message, FineRuleDto? Rule)> CreateRuleAsync(FineRuleDto dto, string createdBy, int schoolId)
        {
            var exists = await _dbContext.FineRules.AnyAsync(r => r.FeeTypeId == dto.FeeTypeId && r.SchoolRegistrationId == schoolId && !r.IsDeleted);
            if (exists) return (false, "A fine rule already exists for this Fee Type.", null);

            var rule = new FineRule
            {
                FeeTypeId = dto.FeeTypeId,
                GraceDays = dto.GraceDays,
                FineType = dto.FineType,
                FineAmount = dto.FineAmount,
                MaxFine = dto.MaxFine,
                IsActive = dto.IsActive,
                SchoolRegistrationId = schoolId,
                CreatedBy = createdBy,
                CreatedDate = DateTime.Now
            };

            await _fineRuleRepo.AddAsync(rule);
            dto.Id = rule.Id;
            return (true, "Fine rule created successfully.", dto);
        }

        public async Task<IEnumerable<FineRuleDto>> GetAllRulesAsync(int schoolId)
        {
            var rules = await _fineRuleRepo.GetAllAsync(schoolId);
            return rules.Select(r => new FineRuleDto
            {
                Id = r.Id,
                FeeTypeId = r.FeeTypeId,
                FeeTypeName = r.FeeType?.Name,
                GraceDays = r.GraceDays,
                FineType = r.FineType,
                FineAmount = r.FineAmount,
                MaxFine = r.MaxFine,
                IsActive = r.IsActive,
                SchoolRegistrationId = r.SchoolRegistrationId
            });
        }

        public async Task<(bool Success, string Message)> UpdateRuleAsync(FineRuleDto dto)
        {
            var rule = await _dbContext.FineRules.FindAsync(dto.Id);
            if (rule == null || rule.IsDeleted) return (false, "Rule not found.");

            rule.GraceDays = dto.GraceDays;
            rule.FineType = dto.FineType;
            rule.FineAmount = dto.FineAmount;
            rule.MaxFine = dto.MaxFine;
            rule.IsActive = dto.IsActive;
            rule.UpdatedDate = DateTime.Now;

            await _fineRuleRepo.UpdateAsync(rule);
            return (true, "Fine rule updated successfully.");
        }

        public async Task<(bool Success, string Message)> DeleteRuleAsync(int id)
        {
            var rule = await _dbContext.FineRules.FindAsync(id);
            if (rule == null || rule.IsDeleted) return (false, "Rule not found.");

            await _fineRuleRepo.DeleteAsync(id);
            return (true, "Fine rule deleted successfully.");
        }

        public async Task<System.Collections.Generic.IEnumerable<School_DTOs.Fee.FeeFineDto>> GetFinesAsync(int schoolId)
        {
            var fines = await _dbContext.FeeFines
                .Include(f => f.Student)
                .Include(f => f.FeeInstallment)
                .Where(f => f.SchoolRegistrationId == schoolId && !f.IsDeleted)
                .OrderByDescending(f => f.CreatedDate)
                .ToListAsync();

            return fines.Select(f => new School_DTOs.Fee.FeeFineDto
            {
                Id = f.Id,
                FeeInstallmentId = f.FeeInstallmentId,
                InstallmentName = f.FeeInstallment?.InstallmentName,
                StudentId = f.StudentId,
                StudentName = f.Student?.Name,
                FineAmount = f.FineAmount,
                FineType = f.FineType,
                DaysLate = f.DaysLate,
                Reason = f.Reason,
                Status = f.Status,
                AppliedBy = f.AppliedBy,
                SchoolRegistrationId = f.SchoolRegistrationId,
                CreatedDate = f.CreatedDate ?? DateTime.Today
            });
        }

        public async Task<(bool Success, string Message)> WaiveFineAsync(int fineId, string reason, string updatedBy)
        {
            var fine = await _dbContext.FeeFines.FindAsync(fineId);
            if (fine == null || fine.IsDeleted) return (false, "Fine record not found.");

            if (fine.Status == "Paid") return (false, "Cannot waive a fine that has already been paid.");

            fine.Status = "Waived";
            fine.Reason = reason;
            fine.UpdatedBy = updatedBy;
            fine.UpdatedDate = DateTime.Now;

            // Subtract fine from installment
            var installment = await _dbContext.FeeInstallments.FindAsync(fine.FeeInstallmentId);
            if (installment != null)
            {
                installment.FineAmount = 0;
                installment.Remarks = (installment.Remarks ?? "") + " [Fine Waived]";
            }

            await _dbContext.SaveChangesAsync();
            return (true, "Fine waived successfully.");
        }

        public async Task<(bool Success, string Message)> RunDailyFineCalculationAsync(int schoolId)
        {
            var rules = (await _fineRuleRepo.GetAllAsync(schoolId)).Where(r => r.IsActive).ToList();
            if (!rules.Any()) return (true, "No active fine rules found for calculation.");

            // Find all pending/overdue installments past due date
            var overdueInstallments = await _dbContext.FeeInstallments
                .Include(i => i.FeeStructure)
                    .ThenInclude(fs => fs.FeeStructureItems)
                .Where(i => i.SchoolRegistrationId == schoolId 
                       && (i.Status == "Pending" || i.Status == "PartiallyPaid" || i.Status == "Overdue") 
                       && i.DueDate < DateTime.Today 
                       && !i.IsDeleted)
                .ToListAsync();

            int appliedCount = 0;

            foreach (var inst in overdueInstallments)
            {
                int? feeTypeId = inst.FeeStructure?.FeeStructureItems?.FirstOrDefault()?.FeeTypeId;
                if (!feeTypeId.HasValue) continue;

                var rule = rules.FirstOrDefault(r => r.FeeTypeId == feeTypeId.Value);
                if (rule == null) continue;

                int daysLate = (DateTime.Today - inst.DueDate).Days;
                if (daysLate <= rule.GraceDays) continue;

                int activeDays = daysLate - rule.GraceDays;
                decimal fineCalculated = rule.FineType == "Percentage" 
                    ? (inst.Amount * rule.FineAmount / 100) * activeDays
                    : rule.FineAmount * activeDays;

                fineCalculated = Math.Min(fineCalculated, rule.MaxFine);

                var existingFine = await _dbContext.FeeFines.FirstOrDefaultAsync(f => f.FeeInstallmentId == inst.Id && !f.IsDeleted);

                if (existingFine == null)
                {
                    var newFine = new FeeFine
                    {
                        FeeInstallmentId = inst.Id,
                        StudentId = inst.StudentId,
                        FineAmount = fineCalculated,
                        FineType = rule.FineType,
                        DaysLate = daysLate,
                        Reason = $"Auto late penalty: {daysLate} days late.",
                        Status = "Pending",
                        AppliedBy = "System Daily Cron",
                        SchoolRegistrationId = schoolId,
                        CreatedDate = DateTime.Now
                    };
                    _dbContext.FeeFines.Add(newFine);
                }
                else if (existingFine.Status == "Pending")
                {
                    existingFine.FineAmount = fineCalculated;
                    existingFine.DaysLate = daysLate;
                    existingFine.Reason = $"Auto late penalty: {daysLate} days late.";
                    existingFine.UpdatedDate = DateTime.Now;
                }

                inst.FineAmount = fineCalculated;
                if (inst.Status == "Pending") inst.Status = "Overdue";

                appliedCount++;
            }

            await _dbContext.SaveChangesAsync();
            return (true, $"Fine calculation completed. Updated/Applied fines to {appliedCount} overdue installments.");
        }
    }
}
