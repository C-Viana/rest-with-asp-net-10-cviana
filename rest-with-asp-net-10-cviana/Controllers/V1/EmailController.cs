using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using rest_with_asp_net_10_cviana.Data.DTO.V1;
using rest_with_asp_net_10_cviana.Services;
using System.Text.Json;

namespace rest_with_asp_net_10_cviana.Controllers.V1
{
    [ApiController]
    [Route("api/[controller]/v1")]
    [Authorize("Bearer")]
    public class EmailController(IEmailServices service, ILogger<EmailController> logger) : ControllerBase
    {
        private readonly IEmailServices _service = service;
        private readonly ILogger<EmailController> _logger = logger;

        [HttpPost]
        [ProducesResponseType(200, Type = typeof(string))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        public IActionResult SendEmail([FromBody] EmailRequestDTO emailRequest)
        {
            _logger.LogInformation("Sending email to {to}", emailRequest.To);
            _service.SendBasicEmail(emailRequest);
            return Ok("E-mail sent successfully");
        }

        [HttpPost("attachment")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(200, Type = typeof(string))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> SendEmailWithAttachment([FromForm] string metadata, [FromForm] FileUploadDTO file)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            EmailRequestDTO emailRequest = JsonSerializer.Deserialize<EmailRequestDTO>(metadata, options);

            _logger.LogInformation("Sending email with files attached to {to}", emailRequest.To);
            await _service.SendEmailWithAttachment(emailRequest, file.File);
            return Ok("E-mail with attachment sent successfully");
        }

    }
}
