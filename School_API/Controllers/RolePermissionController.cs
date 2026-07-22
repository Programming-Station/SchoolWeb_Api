using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using School.Domain.Dtos;
using School.Domain.Entities;
using School.Services.Interfaces;
using School_API.Common.Interface;
namespace School_API.Controllers
{
    
    public class RolePermissionController : BaseController
    {
        private readonly IRolePermissionService _service;

        public RolePermissionController(IRolePermissionService service, ICurrentUserService currentUserService)
            : base(currentUserService)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<RolePermission>>> GetAll(CancellationToken ct)
        {
            var items = await _service.GetAllAsync(ct);
            return Ok(items);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<RolePermission>> GetById(Guid id, CancellationToken ct)
        {
            var item = await _service.GetByIdAsync(id, ct);
            if (item == null) return NotFound();
            return Ok(item);
        }

        [HttpPost]
        public async Task<ActionResult<RolePermission>> Create([FromBody] RolePermissionDto dto, CancellationToken ct)
        {
            var entity = new RolePermission
            {
                Id = Guid.NewGuid(),
                RoleId = dto.RoleId,
                PermissionId = dto.PermissionId
            };
            var created = await _service.CreateAsync(entity, ct);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<RolePermission>> Update(Guid id, [FromBody] RolePermissionDto dto, CancellationToken ct)
        {
            var updatedEntity = new RolePermission
            {
                RoleId = dto.RoleId,
                PermissionId = dto.PermissionId
            };
            var result = await _service.UpdateAsync(id, updatedEntity, ct);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
        {
            var deleted = await _service.DeleteAsync(id, ct);
            if (!deleted) return NotFound();
            return NoContent();
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Microsoft.AspNetCore.Identity.IdentityRole>>> GetRoles(CancellationToken ct)
        {
            var items = await _service.GetRolesAsync(ct);
            return Ok(items);
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Permission>>> GetPermissions(CancellationToken ct)
        {
            var items = await _service.GetPermissionsAsync(ct);
            return Ok(items);
        }
    }
}
