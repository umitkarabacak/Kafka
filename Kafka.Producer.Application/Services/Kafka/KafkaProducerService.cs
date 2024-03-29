﻿using Confluent.Kafka;
using Kafka.Producer.Domain.Models.Mail;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Net.Mail;
using System.Text.Json;
using System.Threading.Tasks;

namespace Kafka.Producer.Application.Services.Kafka
{
    public class KafkaProducerService : IKafkaProducerService
    {
        private readonly ILogger<KafkaProducerService> _logger;
        private readonly ProducerConfig _producerConfig;

        private const string TopicNamePurchase = "purchases";
        private const string TopicNameEmail = "Email";
        private const string TopicNameSms = "Sms";

        public KafkaProducerService(ILogger<KafkaProducerService> logger
            , IOptions<ProducerConfig> options)
        {
            _logger = logger;
            _producerConfig = options.Value;
        }

        public async Task<SendMailResponse> SendMail(SendMailRequest sendMailRequest)
        {
            _logger.LogInformation(JsonSerializer.Serialize(sendMailRequest));
            var responseObject = new SendMailResponse(Guid.NewGuid());

            using (var producer = new ProducerBuilder<string, string>(_producerConfig).Build())
            {
                producer.Produce(TopicNamePurchase, new Message<string, string>
                {
                    Key = Guid.NewGuid().ToString(),
                    Value = sendMailRequest.ToString()
                }, (deliveryReport) =>
                {
                    _logger.LogInformation("SEND MAIL REQUEST RESPONSE \t" + JsonSerializer.Serialize(deliveryReport));

                    if (deliveryReport.Error.Code != ErrorCode.NoError)
                    {
                        _logger.LogInformation($"Failed to deliver message: {deliveryReport.Error.Reason}");
                    }
                    else
                    {
                        _logger.LogInformation($"Produced event to topic {TopicNamePurchase}: key = {JsonSerializer.Serialize(responseObject)} value = {JsonSerializer.Serialize(sendMailRequest)}");
                    }
                });
                producer.Flush(TimeSpan.FromSeconds(10));
            }

            return await Task.FromResult(
                responseObject
            );
        }
    }
}
