using Kafka.Application.Services.Callback;
using Kafka.Application.Services.Kafka;
using Kafka.Domain.Models.Callback;
using Kafka.Domain.Models.Mail;
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

        [HttpPost]
        [ProducesResponseType(typeof(SendMailResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> SendMailMessage(SendMailRequest sendMailRequest)
        {
            _logger.LogInformation($"Send Mail {JsonSerializer.Serialize(sendMailRequest)}");

            var response = await _kafkaProducerService.SendMail(sendMailRequest);

            if (sendMailRequest.IsCallback)
                await _callbackService.SetCallback(new SendCallbackRequest(sendMailRequest.CallbackUrl, response.TraceId));

            return Ok(response);
        }
    }
}
