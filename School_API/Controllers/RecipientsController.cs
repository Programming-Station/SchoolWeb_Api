using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using School.Services.Interfaces;
using School_DTOs;
using School_DTOs.Communication.Recipients;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace School_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RecipientsController : ControllerBase
    {
        private readonly IRecipientService _recipientService;

        public RecipientsController(IRecipientService recipientService)
        {
            _recipientService = recipientService;
        }

        [HttpGet]
        public async Task<ActionResult<APIResponse<IEnumerable<RecipientDto>>>> GetAll()
        {
            var result = await _recipientService.GetAllRecipientsAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<APIResponse<RecipientDto>>> GetById(int id)
        {
            var result = await _recipientService.GetRecipientByIdAsync(id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<APIResponse<RecipientDto>>> Create([FromBody] RecipientCreateDto dto)
        {
            var result = await _recipientService.CreateRecipientAsync(dto);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<APIResponse<bool>>> Delete(int id)
        {
            var result = await _recipientService.DeleteRecipientAsync(id);
            return Ok(result);
        }

        [HttpGet("groups")]
        public async Task<ActionResult<APIResponse<IEnumerable<RecipientGroupDto>>>> GetGroups()
        {
            var result = await _recipientService.GetAllGroupsAsync();
            return Ok(result);
        }
    }
}
