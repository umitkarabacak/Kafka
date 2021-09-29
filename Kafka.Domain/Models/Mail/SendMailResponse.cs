using System;

namespace Kafka.Domain.Models.Mail
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
