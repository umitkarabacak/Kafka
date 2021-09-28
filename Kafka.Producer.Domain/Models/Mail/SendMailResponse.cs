using System;

namespace Kafka.Producer.Domain.Models.Mail
{
    public class SendMailResponse
    {
        public SendMailResponse()
        {

        }

        public SendMailResponse(Guid traceId)
        {
            TraceId = traceId;
        }

        public Guid TraceId { get; set; }
    }
}
