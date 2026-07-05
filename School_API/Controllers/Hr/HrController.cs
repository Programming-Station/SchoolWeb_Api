using Microsoft.AspNetCore.Mvc;
using School.Services.Hr;
using School_API.Common.Interface;
using School_DTOs.Common;
using School_DTOs.Hr;
using System.Threading.Tasks;

namespace School_API.Controllers.Hr
{
    [ApiController]
    public abstract class HrController<TEntity> : BaseController where TEntity : class, global::School.Domain.BaseEntity.IAuditEntity<int>, global::School.Domain.BaseEntity.ITenantEntity
    {
        private readonly IHrMasterService<TEntity> _masterService;

        protected HrController(IHrMasterService<TEntity> masterService, ICurrentUserService currentUserService) : base(currentUserService)
        {
            _masterService = masterService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PaginationFilterDto filter)
        {
            var result = await _masterService.GetAllAsync(filter);
            if (result.Success)
                return Ok(result);

            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _masterService.GetByIdAsync(id);
            if (result.Success)
                return Ok(result);

            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateHrMasterDto model)
        {
            var result = await _masterService.CreateAsync(model, UserName);
            if (result.Success)
                return StatusCode((int)result.StatusCode, result);

            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateHrMasterDto model)
        {
            var result = await _masterService.UpdateAsync(model.Id, model, UserName);
            if (result.Success)
                return Ok(result);

            return StatusCode((int)result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _masterService.DeleteAsync(id, UserName);
            if (result.Success)
                return Ok(result);

            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPatch("{id}/toggle-status")]
        public async Task<IActionResult> ToggleStatus(int id)
        {
            var result = await _masterService.ToggleStatusAsync(id, UserName);
            if (result.Success)
                return Ok(result);

            return StatusCode((int)result.StatusCode, result);
        }
    }
}



