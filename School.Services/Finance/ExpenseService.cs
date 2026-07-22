using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using School.Domain.Finance;
using School.Infrastructure.Repositories.IRepositories;
using School.Infrastructure.UnitOfWork.Interfaces;
using School.Services.Interfaces;
using School.Models.Finance;
using School_DTOs;
using School_DTOs.Finance;

namespace School.Services.Finance
{
    public class ExpenseService : IExpenseService
    {
        private readonly IExpenseRepository _expenseRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ExpenseService(IExpenseRepository expenseRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _expenseRepository = expenseRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<APIResponse<IEnumerable<ExpenseDto>>> GetAllExpensesAsync(int schoolId)
        {
            var expenses = await _expenseRepository.GetAllExpensesAsync();
            var filtered = expenses.Where(e => e.SchoolRegistrationId == schoolId);
            var dtos = _mapper.Map<IEnumerable<ExpenseDto>>(filtered);

            return new APIResponse<IEnumerable<ExpenseDto>>
            {
                Success = true,
                StatusCode = HttpStatusCode.OK,
                Data = dtos
            };
        }

        public async Task<APIResponse<ExpenseDto>> GetExpenseByIdAsync(int id, int schoolId)
        {
            var expense = await _expenseRepository.GetExpenseByIdAsync(id);
            if (expense == null || expense.SchoolRegistrationId != schoolId)
            {
                return new APIResponse<ExpenseDto>
                {
                    Success = false,
                    StatusCode = HttpStatusCode.NotFound,
                    Message = "Expense record not found."
                };
            }

            var dto = _mapper.Map<ExpenseDto>(expense);
            return new APIResponse<ExpenseDto>
            {
                Success = true,
                StatusCode = HttpStatusCode.OK,
                Data = dto
            };
        }

        public async Task<APIResponse<ExpenseDto>> CreateExpenseAsync(ExpenseModel model, int schoolId)
        {
            var expense = _mapper.Map<Expense>(model);
            expense.SchoolRegistrationId = schoolId;
            expense.ExpenseNumber = $"EXP-{DateTime.Now:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 4).ToUpper()}";

            await _expenseRepository.AddAsync(expense);
            await _unitOfWork.CommitAsync();

            // Load populated relationships
            var loaded = await _expenseRepository.GetExpenseByIdAsync(expense.Id);
            var dto = _mapper.Map<ExpenseDto>(loaded);

            return new APIResponse<ExpenseDto>
            {
                Success = true,
                StatusCode = HttpStatusCode.Created,
                Message = "Expense record created successfully.",
                Data = dto
            };
        }

        public async Task<APIResponse<ExpenseDto>> UpdateExpenseAsync(ExpenseModel model, int schoolId)
        {
            var expense = await _expenseRepository.GetExpenseByIdAsync(model.Id);
            if (expense == null || expense.SchoolRegistrationId != schoolId)
            {
                return new APIResponse<ExpenseDto>
                {
                    Success = false,
                    StatusCode = HttpStatusCode.NotFound,
                    Message = "Expense record not found."
                };
            }

            _mapper.Map(model, expense);
            _expenseRepository.Update(expense);
            await _unitOfWork.CommitAsync();

            var loaded = await _expenseRepository.GetExpenseByIdAsync(expense.Id);
            var dto = _mapper.Map<ExpenseDto>(loaded);

            return new APIResponse<ExpenseDto>
            {
                Success = true,
                StatusCode = HttpStatusCode.OK,
                Message = "Expense record updated successfully.",
                Data = dto
            };
        }

        public async Task<APIResponse<bool>> DeleteExpenseAsync(int id, int schoolId)
        {
            var expense = await _expenseRepository.GetExpenseByIdAsync(id);
            if (expense == null || expense.SchoolRegistrationId != schoolId)
            {
                return new APIResponse<bool>
                {
                    Success = false,
                    StatusCode = HttpStatusCode.NotFound,
                    Message = "Expense record not found.",
                    Data = false
                };
            }

            _expenseRepository.Delete(expense);
            await _unitOfWork.CommitAsync();

            return new APIResponse<bool>
            {
                Success = true,
                StatusCode = HttpStatusCode.OK,
                Message = "Expense record deleted successfully.",
                Data = true
            };
        }
    }
}
