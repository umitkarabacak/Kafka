using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kafka.Application.Services.Kafka
{
    public interface IKafkaConfigurationService
    {
        Task<List<string>> GetTopicNames();

        Task CreateTopic(string topicName);

        Task DeleteTopic(string topicName);
    }
}
