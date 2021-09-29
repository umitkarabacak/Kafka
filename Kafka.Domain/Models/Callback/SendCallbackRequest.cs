using System;

namespace Kafka.Domain.Models.Callback
{
    public class SendCallbackRequest
    {
        public string CallbackUrl { get; set; }

        public Guid TraceId { get; set; }

        public SendCallbackRequest(string callbackUrl, Guid traceId)
        {
            CallbackUrl = callbackUrl;
            TraceId = traceId;
        }
    }
}
