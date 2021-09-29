using Confluent.Kafka;
using Kafka.Domain;
using Kafka.Domain.Models.Mail;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace Kafka.Application.Services.Kafka
{
    public class KafkaProducerService : IKafkaProducerService
    {
        private readonly ILogger<KafkaProducerService> _logger;
        private readonly ProducerConfig _producerConfig;
        private readonly IKafkaConfigurationService _kafkaConfigurationService;

        public KafkaProducerService(ILogger<KafkaProducerService> logger
            , IOptions<ProducerConfig> options
            , IKafkaConfigurationService kafkaConfigurationService)
        {
            _logger = logger;
            _producerConfig = options.Value;
            _kafkaConfigurationService = kafkaConfigurationService;
        }

        public async Task<SendMailResponse> SendMail(SendMailRequest sendMailRequest)
        {
            var responseObject = new SendMailResponse(Guid.NewGuid());

            using (var producer = new ProducerBuilder<string, string>(_producerConfig).Build())
            {
                await _kafkaConfigurationService.CreateTopic(AppConsts.TopicNameEmail);

                producer.Produce(AppConsts.TopicNameEmail, new Message<string, string>
                {
                    Key = Guid.NewGuid().ToString(),
                    Value = sendMailRequest.ToString()
                }, (deliveryReport) =>
                {
                    _logger.LogInformation($"Send mail response \t {JsonSerializer.Serialize(deliveryReport)}");

                    if (deliveryReport.Error.Code != ErrorCode.NoError)
                    {
                        _logger.LogError($"Failed to deliver message: {deliveryReport.Error.Reason}");
                    }
                    else
                    {
                        _logger.LogInformation($"Produced event to topic {AppConsts.TopicNameEmail}: key = {JsonSerializer.Serialize(responseObject)} value = {JsonSerializer.Serialize(sendMailRequest)}");
                    }
                });
                producer.Flush(TimeSpan.FromSeconds(10));
            }

            return responseObject;
        }
    }
}
