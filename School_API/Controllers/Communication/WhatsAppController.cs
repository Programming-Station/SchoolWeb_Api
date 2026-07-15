using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using School.Services.Interfaces;
using School_API.Common.Interface;
using School_DTOs;
using School_DTOs.Communication;

namespace School_API.Controllers.Communication
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class WhatsAppController : BaseController
    {
        private readonly IWhatsAppService _svc;

        public WhatsAppController(IWhatsAppService svc, ICurrentUserService cur) : base(cur)
        {
            _svc = svc;
        }

        [HttpGet]
        public async Task<IActionResult> GetWhatsAppAccount()
        {
            var r = await _svc.GetWhatsAppAccountAsync();
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpPost]
        public async Task<IActionResult> SaveWhatsAppAccount([FromBody] WhatsAppAccountDto dto)
        {
            var r = await _svc.SaveWhatsAppAccountAsync(dto, UserName);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpGet]
        public async Task<IActionResult> GetTemplates()
        {
            var r = await _svc.GetTemplatesAsync();
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTemplate([FromBody] WhatsAppTemplateDto dto)
        {
            var r = await _svc.CreateTemplateAsync(dto, UserName);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpPost]
        public async Task<IActionResult> SendText([FromBody] SendWhatsAppTextRequest request)
        {
            var r = await _svc.SendTextAsync(request.RecipientPhone, request.Message, UserName);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpPost]
        public async Task<IActionResult> SendTemplate([FromBody] SendWhatsAppTemplateRequest request)
        {
            var r = await _svc.SendTemplateAsync(request.RecipientPhone, request.TemplateName, request.Variables, UserName);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpPost]
        public async Task<IActionResult> SendMedia([FromBody] SendWhatsAppMediaRequest request)
        {
            var r = await _svc.SendMediaAsync(request.RecipientPhone, request.MediaUrl, request.MimeType, request.Filename, UserName);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpGet]
        public async Task<IActionResult> GetQueuedMessages()
        {
            var r = await _svc.GetQueuedMessagesAsync();
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpPost("{queueId}")]
        public async Task<IActionResult> RetryFailedMessage(int queueId)
        {
            var r = await _svc.RetryFailedMessageAsync(queueId, UserName);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpPost("{queueId}")]
        public async Task<IActionResult> CancelQueuedMessage(int queueId)
        {
            var r = await _svc.CancelQueuedMessageAsync(queueId, UserName);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpGet]
        public async Task<IActionResult> GetLogs([FromQuery] string? recipientPhone, [FromQuery] string? status, [FromQuery] DateTime? fromDate, [FromQuery] DateTime? toDate)
        {
            var filter = new GatewayLogFilterDto { RecipientNo = recipientPhone, Status = status, FromDate = fromDate, ToDate = toDate };
            var r = await _svc.GetLogsAsync(filter);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        // Webhook Verification (Meta endpoint validation call)
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Webhook([FromQuery(Name = "hub.mode")] string mode, [FromQuery(Name = "hub.verify_token")] string verifyToken, [FromQuery(Name = "hub.challenge")] string challenge)
        {
            // Meta verifies webhook by sending a challenge string if the verify token matches
            if (mode == "subscribe" && !string.IsNullOrWhiteSpace(verifyToken))
            {
                return Ok(challenge);
            }
            return BadRequest("Invalid validation parameters.");
        }

        // Webhook Status Updates Callback
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Webhook()
        {
            using var reader = new StreamReader(Request.Body);
            var payload = await reader.ReadToEndAsync();
            var r = await _svc.ProcessWebhookCallbackAsync(payload);
            return r.Success ? Ok(r) : BadRequest(r.Message);
        }
    }

    public class SendWhatsAppTextRequest
    {
        public string RecipientPhone { get; set; } = null!;
        public string Message { get; set; } = null!;
    }

    public class SendWhatsAppTemplateRequest
    {
        public string RecipientPhone { get; set; } = null!;
        public string TemplateName { get; set; } = null!;
        public Dictionary<string, string> Variables { get; set; } = new();
    }

    public class SendWhatsAppMediaRequest
    {
        public string RecipientPhone { get; set; } = null!;
        public string MediaUrl { get; set; } = null!;
        public string MimeType { get; set; } = null!;
        public string Filename { get; set; } = null!;
    }
}
