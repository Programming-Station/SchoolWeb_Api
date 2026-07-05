using School.Models.Fee;
using School_DTOs; 
using School_DTOs.Fee;
using System;
using System.Collections.Generic;
using System.Text;

namespace School.Services.Interfaces
{
    public interface IFeeTypeService
    {
        Task<APIResponse<FeeTypeDto>> AddFeeTypeAsync(FeeTypeModel model);
        Task<APIResponse<IEnumerable<FeeTypeDto>>> GetFeeTypesBySchoolIdAsync(int schoolId);
    }
}
