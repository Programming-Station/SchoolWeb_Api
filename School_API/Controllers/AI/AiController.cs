using Microsoft.AspNetCore.Mvc;
using School.Infrastructure.Interfaces;
using School.Services.Interfaces;
using School_API.Common.Interface;
using School_DTOs.AI;

namespace School_API.Controllers.AI
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AiController : BaseController
    {
        private readonly IAiService _aiService;
        private readonly ITenantService _tenantService;

        public AiController(
            IAiService aiService,
            ITenantService tenantService,
            ICurrentUserService currentUserService) : base(currentUserService)
        {
            _aiService = aiService;
            _tenantService = tenantService;
        }

        private int SchoolId => _tenantService.GetTenantId() ?? 1;

        // ── Predictions ────────────────────────────────────────────────────────

        [HttpGet]
        public async Task<IActionResult> GetPredictions([FromQuery] string? predictionType)
        {
            var filter = new AiPredictionFilterDto { PredictionType = predictionType };
            var r = await _aiService.GetPredictionsAsync(filter, SchoolId);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpPost]
        public async Task<IActionResult> RunPrediction([FromQuery] string predictionType, [FromQuery] int targetId)
        {
            var r = await _aiService.RunPredictionAsync(predictionType, targetId, SchoolId);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        // ── Generations ────────────────────────────────────────────────────────

        [HttpGet]
        public async Task<IActionResult> GetGenerations([FromQuery] string? type)
        {
            var r = await _aiService.GetGenerationsAsync(type, SchoolId);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpPost]
        public async Task<IActionResult> GenerateContent([FromBody] AiGenerationRequestDto request)
        {
            var r = await _aiService.GenerateContentAsync(request, SchoolId);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        // ── Chatbot / Assistant ────────────────────────────────────────────────

        [HttpPost]
        public async Task<IActionResult> CreateChatSession()
        {
            var r = await _aiService.CreateChatSessionAsync(UserId, UserName, SchoolId);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpGet]
        public async Task<IActionResult> GetChatSessions()
        {
            var r = await _aiService.GetChatSessionsAsync(UserId, SchoolId);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpGet("{sessionId}")]
        public async Task<IActionResult> GetChatSession(Guid sessionId)
        {
            var r = await _aiService.GetChatSessionAsync(sessionId, SchoolId);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpPost("{sessionId}")]
        public async Task<IActionResult> SendChatMessage(Guid sessionId, [FromBody] AiChatRequestDto request)
        {
            var r = await _aiService.SendChatMessageAsync(sessionId, request, SchoolId);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }
    }
}
