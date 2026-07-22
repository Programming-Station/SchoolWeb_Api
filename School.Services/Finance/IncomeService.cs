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
    public class IncomeService : IIncomeService
    {
        private readonly IIncomeRepository _incomeRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public IncomeService(IIncomeRepository incomeRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _incomeRepository = incomeRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<APIResponse<IEnumerable<IncomeDto>>> GetAllIncomesAsync(int schoolId)
        {
            var incomes = await _incomeRepository.GetAllIncomesAsync();
            var filtered = incomes.Where(i => i.SchoolRegistrationId == schoolId);
            var dtos = _mapper.Map<IEnumerable<IncomeDto>>(filtered);

            return new APIResponse<IEnumerable<IncomeDto>>
            {
                Success = true,
                StatusCode = HttpStatusCode.OK,
                Data = dtos
            };
        }

        public async Task<APIResponse<IncomeDto>> GetIncomeByIdAsync(int id, int schoolId)
        {
            var income = await _incomeRepository.GetIncomeByIdAsync(id);
            if (income == null || income.SchoolRegistrationId != schoolId)
            {
                return new APIResponse<IncomeDto>
                {
                    Success = false,
                    StatusCode = HttpStatusCode.NotFound,
                    Message = "Income record not found."
                };
            }

            var dto = _mapper.Map<IncomeDto>(income);
            return new APIResponse<IncomeDto>
            {
                Success = true,
                StatusCode = HttpStatusCode.OK,
                Data = dto
            };
        }

        public async Task<APIResponse<IncomeDto>> CreateIncomeAsync(IncomeModel model, int schoolId)
        {
            var income = _mapper.Map<Income>(model);
            income.SchoolRegistrationId = schoolId;
            income.IncomeNumber = $"INC-{DateTime.Now:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 4).ToUpper()}";

            await _incomeRepository.AddAsync(income);
            await _unitOfWork.CommitAsync();

            var loaded = await _incomeRepository.GetIncomeByIdAsync(income.Id);
            var dto = _mapper.Map<IncomeDto>(loaded);

            return new APIResponse<IncomeDto>
            {
                Success = true,
                StatusCode = HttpStatusCode.Created,
                Message = "Income record created successfully.",
                Data = dto
            };
        }

        public async Task<APIResponse<IncomeDto>> UpdateIncomeAsync(IncomeModel model, int schoolId)
        {
            var income = await _incomeRepository.GetIncomeByIdAsync(model.Id);
            if (income == null || income.SchoolRegistrationId != schoolId)
            {
                return new APIResponse<IncomeDto>
                {
                    Success = false,
                    StatusCode = HttpStatusCode.NotFound,
                    Message = "Income record not found."
                };
            }

            _mapper.Map(model, income);
            _incomeRepository.Update(income);
            await _unitOfWork.CommitAsync();

            var loaded = await _incomeRepository.GetIncomeByIdAsync(income.Id);
            var dto = _mapper.Map<IncomeDto>(loaded);

            return new APIResponse<IncomeDto>
            {
                Success = true,
                StatusCode = HttpStatusCode.OK,
                Message = "Income record updated successfully.",
                Data = dto
            };
        }

        public async Task<APIResponse<bool>> DeleteIncomeAsync(int id, int schoolId)
        {
            var income = await _incomeRepository.GetIncomeByIdAsync(id);
            if (income == null || income.SchoolRegistrationId != schoolId)
            {
                return new APIResponse<bool>
                {
                    Success = false,
                    StatusCode = HttpStatusCode.NotFound,
                    Message = "Income record not found.",
                    Data = false
                };
            }

            _incomeRepository.Delete(income);
            await _unitOfWork.CommitAsync();

            return new APIResponse<bool>
            {
                Success = true,
                StatusCode = HttpStatusCode.OK,
                Message = "Income record deleted successfully.",
                Data = true
            };
        }
    }
}
