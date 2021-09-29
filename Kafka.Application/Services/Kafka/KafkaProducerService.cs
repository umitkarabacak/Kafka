using Confluent.Kafka;
using Kafka.Domain;
using Kafka.Domain.Models.Mail;
using Kafka.Domain.Models.Sms;
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

        public async Task<string> SendTest(string sampleText)
        {
            var responseObject = new SendMailResponse(Guid.NewGuid());

            using (var producer = new ProducerBuilder<string, string>(_producerConfig).Build())
            {
                await _kafkaConfigurationService.CreateTopic(AppConsts.TopicNameTestChannel);

                producer.Produce(AppConsts.TopicNameTestChannel, new Message<string, string>
                {
                    Key = sampleText,
                    Value = sampleText
                }, (deliveryReport) =>
                {
                    _logger.LogInformation($"Send test response \t {JsonSerializer.Serialize(deliveryReport)}");

                    if (deliveryReport.Error.Code != ErrorCode.NoError)
                    {
                        _logger.LogError($"Failed to deliver message: {deliveryReport.Error.Reason}");
                    }
                    else
                    {
                        _logger.LogInformation($"Produced event to topic {AppConsts.TopicNameTestChannel}: key = {sampleText} value = {sampleText}");
                    }
                });
                producer.Flush(TimeSpan.FromSeconds(10));
            }

            return sampleText;
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

        public async Task<SendSmsResponse> SendSms(SendSmsRequest sendSmsRequest)
        {
            var responseObject = new SendSmsResponse();

            using (var producer = new ProducerBuilder<string, string>(_producerConfig).Build())
            {
                await _kafkaConfigurationService.CreateTopic(AppConsts.TopicNameSms);

                producer.Produce(AppConsts.TopicNameSms, new Message<string, string>
                {
                    Key = Guid.NewGuid().ToString(),
                    Value = sendSmsRequest.ToString()
                }, (deliveryReport) =>
                {
                    _logger.LogInformation($"Send sms response \t {JsonSerializer.Serialize(deliveryReport)}");

                    if (deliveryReport.Error.Code != ErrorCode.NoError)
                    {
                        _logger.LogError($"Failed to deliver message: {deliveryReport.Error.Reason}");
                    }
                    else
                    {
                        _logger.LogInformation($"Produced event to topic {AppConsts.TopicNameSms}: key = {JsonSerializer.Serialize(responseObject)} value = {JsonSerializer.Serialize(sendSmsRequest)}");
                    }
                });
                producer.Flush(TimeSpan.FromSeconds(10));
            }

            return responseObject;
        }
    }
}
