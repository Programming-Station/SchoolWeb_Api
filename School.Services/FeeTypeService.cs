using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using School.Domain;
using School.Domain.FeeManagnment;
using School.Infrastructure.Repositories.IRepositories;
using School.Models.Fee;
using School.Services.Interfaces;
using School.Utilities.Resources;
using School_DTOs;
using School_DTOs.Fee;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace School.Services
{
    public class FeeTypeService : IFeeTypeService
    {
        private readonly IFeeTypeRepository _feeTypeRepository;
        private readonly IMapper _mapper;
        public FeeTypeService(IFeeTypeRepository feeService, IMapper mapper)
        {
            _feeTypeRepository = feeService;
            _mapper = mapper;
        } 

        public async Task<APIResponse<FeeTypeDto>> AddFeeTypeAsync(FeeTypeModel model)
        {
            var entity = _mapper.Map<FeeType>(model);
            entity = await _feeTypeRepository.AddFeeTypeAsync(entity);
            if (entity != null && entity.Id == 0)
            {
                return new APIResponse<FeeTypeDto>
                {
                    Success = false,
                    Data = _mapper.Map<FeeTypeDto>(entity),
                    Message = string.Format(CommonResource.AlreadyExists, typeof(FeeType).Name, model.Name),
                };
            }
            else if (entity != null && entity.Id >= 0)
            {
                return new APIResponse<FeeTypeDto>
                {
                    Success = true,
                    Data = _mapper.Map<FeeTypeDto>(entity),
                    Message = CommonResource.RecordNotFound,
                    StatusCode = System.Net.HttpStatusCode.Created
                };
            }
            else
            {
                return new APIResponse<FeeTypeDto>
                {
                    Success = false,
                    Message = CommonResource.AddFailed,
                    StatusCode = HttpStatusCode.Forbidden
                };
            }
        }

        public async Task<APIResponse<IEnumerable<FeeTypeDto>>> GetFeeTypesBySchoolIdAsync(int schoolId)
        {
            var result = await _feeTypeRepository.GetFeeTypeBySchoolIdAsync(schoolId);

            if (result != null && result.Any())
            {
                return new APIResponse<IEnumerable<FeeTypeDto>>
                {
                    Data = _mapper.Map<IEnumerable<FeeTypeDto>>(result),
                    Message = CommonResource.FetchSuccess,
                    Success = true,
                    StatusCode = HttpStatusCode.OK
                };
            }
            else
            {
                return new APIResponse<IEnumerable<FeeTypeDto>>
                {
                    Message = CommonResource.RecordNotFound,
                    Success = false,
                    StatusCode = HttpStatusCode.OK
                };
            }
        }

    }
}
