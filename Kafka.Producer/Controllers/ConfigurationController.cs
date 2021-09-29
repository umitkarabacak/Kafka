using Kafka.Application.Services.Kafka;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Kafka.Producer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConfigurationController : ControllerBase
    {
        private readonly IKafkaConfigurationService _kafkaConfigurationService;

        public ConfigurationController(IKafkaConfigurationService kafkaConfigurationService)
        {
            _kafkaConfigurationService = kafkaConfigurationService;
        }

        [HttpGet("active-topics")]
        public async Task<IActionResult> GetTopicList()
        {
            var topics = await _kafkaConfigurationService.GetTopicNames();

            return Ok(topics);
        }

        [HttpPost("create-topic/{topicName}")]
        public async Task<IActionResult> CreateTopic(string topicName)
        {
            await _kafkaConfigurationService.CreateTopic(topicName);
            var topics = await _kafkaConfigurationService.GetTopicNames();

            return Ok(topics);
        }
    }
}
