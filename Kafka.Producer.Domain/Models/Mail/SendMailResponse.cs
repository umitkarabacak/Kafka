using System;

namespace Kafka.Producer.Domain.Models.Mail
{
    public class SendMailResponse
    {
        public Guid TraceId { get; set; } = Guid.NewGuid();
    }
}
