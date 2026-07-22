using Microsoft.AspNetCore.Mvc;
using School.Models.City;
using School.Services.Interfaces;
using School_API.Common.Interface;

namespace School_API.Controllers
{

    public class CityController : BaseController
    {
        private readonly ICityService _cityService;

        public CityController(
            ICityService cityService,
            ICurrentUserService currentUser) : base(currentUser)
        {
            _cityService = cityService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCity([FromBody] CityModel model)
        {
            model.CreatedBy = UserName;
            var result = await _cityService.AddCityAsync(model);

            if (result.Success)
                return StatusCode((int)result.StatusCode, result);

            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet]
        public async Task<IActionResult> GetCityById(int id)
        {
            var result = await _cityService.GetCityByIdAsync(id);

            if (result.Success)
                return Ok(result);

            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCities([FromQuery] int? stateId = null)
        {
            var result = await _cityService.GetAllCitiesAsync(stateId);

            if (result.Success)
                return Ok(result);

            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCity([FromBody] CityModel model)
        {
            model.UpdatedBy = UserName;
            var result = await _cityService.UpdateCityAsync(model);

            if (result.Success)
                return Ok(result);

            return StatusCode((int)result.StatusCode, result);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteCity(int id)
        {
            var result = await _cityService.DeleteCityAsync(id);

            if (result.Success)
                return Ok(result);

            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPatch]
        public async Task<IActionResult> ToggleCityStatus(int id)
        {
            var result = await _cityService.ToggleCityStatusAsync(id);

            if (result.Success)
                return Ok(result);

            return StatusCode((int)result.StatusCode, result);
        }
    }
}
