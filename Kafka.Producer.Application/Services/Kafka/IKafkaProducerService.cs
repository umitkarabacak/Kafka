using Kafka.Producer.Domain.Models.Mail;
using System.Threading.Tasks;

namespace Kafka.Producer.Application.Services.Kafka
{
    public interface IKafkaProducerService
    {
        Task<SendMailResponse> SendMail(SendMailRequest sendMailRequest);
    }
}
