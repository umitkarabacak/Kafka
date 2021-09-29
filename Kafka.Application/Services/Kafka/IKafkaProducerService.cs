using Kafka.Domain.Models.Mail;
using System.Threading.Tasks;

namespace Kafka.Application.Services.Kafka
{
    public interface IKafkaProducerService
    {
        Task<SendMailResponse> SendMail(SendMailRequest sendMailRequest);
    }
}
