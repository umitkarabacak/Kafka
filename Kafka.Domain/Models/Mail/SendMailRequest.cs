using System.Collections.Generic;
using System.Text.Json;

namespace Kafka.Domain.Models.Mail
{
    public class SendMailRequest
    {
        public string SenderFullName { get; set; }
        public string SenderEmailAddress { get; set; }

        public string Subject { get; set; }
        public bool IsBodyHtml { get; set; } = true;
        public string Body { get; set; }

        public List<string> ToEmails { get; set; } = new();
        public List<string> CcEmails { get; set; } = new();
        public List<string> BccEmails { get; set; } = new();

        public List<string> AttachmentPaths { get; set; } = new();

        public string CallbackUrl { get; set; }

        public bool IsCallback => !string.IsNullOrWhiteSpace(CallbackUrl);

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
