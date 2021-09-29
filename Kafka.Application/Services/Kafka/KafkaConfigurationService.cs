using Confluent.Kafka;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kafka.Application.Services.Kafka
{
    public class KafkaConfigurationService : IKafkaConfigurationService
    {
        private readonly ProducerConfig _producerConfig;

        public KafkaConfigurationService(IOptions<ProducerConfig> options)
        {
            _producerConfig = options.Value;
        }

        public async Task<List<string>> GetTopicNames()
        {
            using var adminClient = new AdminClientBuilder(_producerConfig).Build();

            var metadata = adminClient.GetMetadata(TimeSpan.FromSeconds(10));
            var topicNames = metadata.Topics.Select(a => a.Topic).ToList();

            return await Task.FromResult(
                topicNames
            );
        }

        public async Task CreateTopic(string topicName)
        {
            using var adminClient = new AdminClientBuilder(_producerConfig).Build();

            var metadata = adminClient.GetMetadata(TimeSpan.FromSeconds(10));
            var topicNames = metadata.Topics.Select(a => a.Topic).ToList();

            if (topicNames.FirstOrDefault(t => t.Equals(topicName)) is null)
                metadata.Topics.Add(new TopicMetadata(topicName, null, null));

            await Task.CompletedTask;
        }

        public async Task DeleteTopic(string topicName)
        {
            using var adminClient = new AdminClientBuilder(_producerConfig).Build();

            var metadata = adminClient.GetMetadata(TimeSpan.FromSeconds(10));
            var topicNames = metadata.Topics.Select(a => a.Topic).ToList();

            var currentTopic = metadata.Topics.FirstOrDefault(t => t.Topic.Equals(topicName));
            if (currentTopic is not null)
                metadata.Topics.Remove(currentTopic);

            await Task.CompletedTask;
        }
    }
}
