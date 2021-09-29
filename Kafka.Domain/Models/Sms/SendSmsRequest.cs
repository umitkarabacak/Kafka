using System.Text.Json;

namespace Kafka.Domain.Models.Sms
{
    public class SendSmsRequest
    {
        public string PhoneNumber { get; set; }

        public string SmsContent { get; set; }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
