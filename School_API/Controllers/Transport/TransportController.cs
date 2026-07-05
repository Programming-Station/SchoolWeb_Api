using Microsoft.AspNetCore.Mvc;
using School.Services.Interfaces.Transport;
using School_API.Common.Interface;
using School_DTOs.Transport;
using System.Threading.Tasks;
namespace School_API.Controllers.Transport
{
    [Route(""api/[controller]"")][ApiController]
    public class VehicleController : BaseController
    {
        private readonly IVehicleService _svc;
        public VehicleController(IVehicleService svc,ICurrentUserService cur):base(cur){_svc=svc;}
        [HttpGet] public async Task<IActionResult> GetAll(){var r=await _svc.GetAllAsync();return r.Success?Ok(r):StatusCode((int)r.StatusCode,r);}
        [HttpGet(""{id}"")] public async Task<IActionResult> GetById(int id){var r=await _svc.GetByIdAsync(id);return r.Success?Ok(r):StatusCode((int)r.StatusCode,r);}
        [HttpPost] public async Task<IActionResult> Create([FromBody]CreateVehicleDto m){var r=await _svc.CreateAsync(m,UserName);return StatusCode((int)r.StatusCode,r);}
        [HttpPut] public async Task<IActionResult> Update([FromBody]UpdateVehicleDto m){var r=await _svc.UpdateAsync(m.Id,m,UserName);return r.Success?Ok(r):StatusCode((int)r.StatusCode,r);}
        [HttpDelete(""{id}"")] public async Task<IActionResult> Delete(int id){var r=await _svc.DeleteAsync(id,UserName);return r.Success?Ok(r):StatusCode((int)r.StatusCode,r);}
    }

    [Route(""api/[controller]"")][ApiController]
    public class TransportRouteController : BaseController
    {
        private readonly ITransportRouteService _svc;
        public TransportRouteController(ITransportRouteService svc,ICurrentUserService cur):base(cur){_svc=svc;}
        [HttpGet] public async Task<IActionResult> GetAll(){var r=await _svc.GetAllAsync();return r.Success?Ok(r):StatusCode((int)r.StatusCode,r);}
        [HttpGet(""{id}"")] public async Task<IActionResult> GetById(int id){var r=await _svc.GetByIdAsync(id);return r.Success?Ok(r):StatusCode((int)r.StatusCode,r);}
        [HttpPost] public async Task<IActionResult> Create([FromBody]CreateTransportRouteDto m){var r=await _svc.CreateAsync(m,UserName);return StatusCode((int)r.StatusCode,r);}
        [HttpPut] public async Task<IActionResult> Update([FromBody]UpdateTransportRouteDto m){var r=await _svc.UpdateAsync(m.Id,m,UserName);return r.Success?Ok(r):StatusCode((int)r.StatusCode,r);}
        [HttpDelete(""{id}"")] public async Task<IActionResult> Delete(int id){var r=await _svc.DeleteAsync(id,UserName);return r.Success?Ok(r):StatusCode((int)r.StatusCode,r);}
    }
}
