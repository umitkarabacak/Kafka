using Kafka.Domain.Models.Mail;
using Kafka.Domain.Models.Sms;
using System.Threading.Tasks;

namespace Kafka.Application.Services.Kafka
{
    public interface IKafkaProducerService
    {
        Task<string> SendTest(string sampleText);

        Task<SendMailResponse> SendMail(SendMailRequest sendMailRequest);

        Task<SendSmsResponse> SendSms(SendSmsRequest sendSmsRequest);
    }
}
