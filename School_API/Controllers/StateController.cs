using School.Models.State;
using School.Services.Interfaces;
using School_API.Common.Interface;
using Microsoft.AspNetCore.Mvc;

namespace School_API.Controllers
{

    public class StateController : BaseController
    {
        private readonly IStateService _stateService;

        public StateController(
            IStateService stateService,
            ICurrentUserService currentUser) : base(currentUser)
        {
            _stateService = stateService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateState([FromBody] StateModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            model.CreatedBy = UserName;
            var result = await _stateService.AddStateAsync(model);

            if (result.Success)
                return StatusCode((int)result.StatusCode, result);

            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet]
        public async Task<IActionResult> GetStateById(int id)
        {
            var result = await _stateService.GetStateByIdAsync(id);

            if (result.Success)
                return Ok(result);

            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllStates()
        {
            var result = await _stateService.GetAllStatesAsync();

            if (result.Success)
                return Ok(result);

            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateState([FromBody] StateModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            model.UpdatedBy = UserName;
            var result = await _stateService.UpdateStateAsync(model);

            if (result.Success)
                return Ok(result);

            return StatusCode((int)result.StatusCode, result);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteState(int id)
        {
            var result = await _stateService.DeleteStateAsync(id);

            if (result.Success)
                return Ok(result);

            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPatch]
        public async Task<IActionResult> ToggleStateStatus(int id)
        {
            var result = await _stateService.ToggleStateStatusAsync(id);

            if (result.Success)
                return Ok(result);

            return StatusCode((int)result.StatusCode, result);
        }
    }
}
