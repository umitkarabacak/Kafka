using Kafka.Application.Services.Callback;
using Kafka.Application.Services.Kafka;
using Kafka.Domain.Models.Callback;
using Kafka.Domain.Models.Mail;
using Kafka.Domain.Models.Sms;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace Kafka.Producer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationController : ControllerBase
    {
        private readonly ILogger<NotificationController> _logger;
        private readonly IKafkaProducerService _kafkaProducerService;
        private readonly ICallbackService _callbackService;

        public NotificationController(ILogger<NotificationController> logger
            , IKafkaProducerService kafkaProducerService
            , ICallbackService callbackService)
        {
            _logger = logger;
            _kafkaProducerService = kafkaProducerService;
            _callbackService = callbackService;
        }

        [HttpPost("send-test/{sampleText}")]
        public async Task<IActionResult> SendTest(string sampleText)
        {
            _logger.LogInformation($"Send Test {JsonSerializer.Serialize(sampleText)}");

            var response = await _kafkaProducerService.SendTest(sampleText);

            return Ok(response);
        }

        [HttpPost("send-email")]
        [ProducesResponseType(typeof(SendMailResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> SendMailMessage(SendMailRequest sendMailRequest)
        {
            _logger.LogInformation($"Send Mail {JsonSerializer.Serialize(sendMailRequest)}");

            var response = await _kafkaProducerService.SendMail(sendMailRequest);

            if (sendMailRequest.IsCallback)
            {
                var callbackRequest = new SendCallbackRequest(sendMailRequest.CallbackUrl, response.TraceId);
                _logger.LogInformation($"Send Callback {JsonSerializer.Serialize(callbackRequest)}");

                await _callbackService.SetCallback(callbackRequest);
            }

            return Ok(response);
        }

        [HttpPost("send-sms")]
        [ProducesResponseType(typeof(SendSmsResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> SendSmsMessage(SendSmsRequest sendSmsRequest)
        {
            _logger.LogInformation($"Send Sms {JsonSerializer.Serialize(sendSmsRequest)}");

            var response = await _kafkaProducerService.SendSms(sendSmsRequest);

            return Ok(response);
        }
    }
}
