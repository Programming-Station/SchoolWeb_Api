using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using School.Models.Event;
using School.Services.Interfaces;
using School_API.Common.Interface;

namespace School_API.Controllers
{
    public class EventController : BaseController
    {
        private readonly IEventService _eventService;

        public EventController(IEventService eventService, ICurrentUserService currentUser) : base(currentUser)
        {
            _eventService = eventService;
        }

        /// <summary>
        /// Create a new event
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateEvent([FromBody] EventModel model)
        {
            model.CreatedBy = UserName;
            var result = await _eventService.AddEventAsync(model);

            if (result.Success)
                return StatusCode((int)result.StatusCode, result);

            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Get event by ID
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetEventById(int id)
        {
            var result = await _eventService.GetEventByIdAsync(id);

            if (result.Success)
                return Ok(result);

            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Get all events (optionally filter for upcoming events only)
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllEvents([FromQuery] bool? upcomingOnly = null)
        {
            var result = await _eventService.GetAllEventsAsync(upcomingOnly);

            if (result.Success)
                return Ok(result);

            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Get upcoming events only
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetUpcomingEvents()
        {
            var result = await _eventService.GetAllEventsAsync(upcomingOnly: true);

            if (result.Success)
                return Ok(result);

            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Update an existing event
        /// </summary>
        [HttpPut]
        public async Task<IActionResult> UpdateEvent([FromBody] EventModel model)
        {
            model.UpdatedBy = UserName;
            var result = await _eventService.UpdateEventAsync(model);

            if (result.Success)
                return Ok(result);

            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Delete an event
        /// </summary>
        [HttpDelete]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            var result = await _eventService.DeleteEventAsync(id);

            if (result.Success)
                return Ok(result);

            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Toggle event active status
        /// </summary>
        [HttpPatch]
        public async Task<IActionResult> ToggleEventStatus(int id)
        {
            var result = await _eventService.ToggleEventStatusAsync(id);

            if (result.Success)
                return Ok(result);

            return StatusCode((int)result.StatusCode, result);
        }
    }
}

