using Confluent.Kafka;
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

            await SendTemp();

            return await Task.FromResult(
                new SendMailResponse()
            );
        }

        public async Task SendTemp()
        {
            string[] users = { "eabara", "jsmith", "sgarcia", "jbernard", "htanaka", "awalther" };
            string[] items = { "book", "alarm clock", "t-shirts", "gift card", "batteries" };

            using (var producer = new ProducerBuilder<string, string>(_producerConfig).Build())
            {
                var numProduced = 0;
                const int numMessages = 10;
                for (int i = 0; i < numMessages; ++i)
                {
                    Random rnd = new Random();
                    var user = users[rnd.Next(users.Length)];
                    var item = items[rnd.Next(items.Length)];

                    producer.Produce(TopicNamePurchase, new Message<string, string>
                    {
                        Key = user,
                        Value = item
                    }, (deliveryReport) =>
                    {
                        if (deliveryReport.Error.Code != ErrorCode.NoError)
                        {
                            _logger.LogInformation($"Failed to deliver message: {deliveryReport.Error.Reason}");
                        }
                        else
                        {
                            _logger.LogInformation($"Produced event to topic {TopicNamePurchase}: key = {user,-10} value = {item}");
                            numProduced += 1;
                        }
                    });
                }

                producer.Flush(TimeSpan.FromSeconds(10));
                _logger.LogInformation($"{numProduced} messages were produced to topic {TopicNamePurchase}");

                await Task.CompletedTask;
            }
        }

        /*
            const string topic = "purchases";
            var producerConfig = new ProducerConfig
            {
                BootstrapServers = "localhost:9092",
            };

            string[] users = { "eabara", "jsmith", "sgarcia", "jbernard", "htanaka", "awalther" };
            string[] items = { "book", "alarm clock", "t-shirts", "gift card", "batteries" };

            using (var producer = new ProducerBuilder<string, string>(producerConfig).Build())
            {
                var numProduced = 0;
                const int numMessages = 10;
                for (int i = 0; i < numMessages; ++i)
                {
                    Random rnd = new Random();
                    var user = users[rnd.Next(users.Length)];
                    var item = items[rnd.Next(items.Length)];

                    producer.Produce(topic, new Message<string, string>
                    {
                        Key = user,
                        Value = item
                    }, (deliveryReport) =>
                    {
                        if (deliveryReport.Error.Code != ErrorCode.NoError)
                        {
                            Console.WriteLine($"Failed to deliver message: {deliveryReport.Error.Reason}");
                        }
                        else
                        {
                            Console.WriteLine($"Produced event to topic {topic}: key = {user,-10} value = {item}");
                            numProduced += 1;
                        }
                    });
                }

                producer.Flush(TimeSpan.FromSeconds(10));
                Console.WriteLine($"{numProduced} messages were produced to topic {topic}");
            }
        */
    }
}
