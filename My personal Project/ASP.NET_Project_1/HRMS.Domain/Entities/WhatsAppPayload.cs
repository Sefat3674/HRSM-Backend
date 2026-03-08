using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HRMS.Domain.Entities
{
    public class WhatsAppPayload
    {
        public List<WhatsAppEntry> entry { get; set; }
    }

    public class WhatsAppEntry
    {
        public List<WhatsAppChange> changes { get; set; }
    }

    public class WhatsAppChange
    {
        public WhatsAppValue value { get; set; }
    }

    public class WhatsAppValue
    {
        public List<WhatsAppMessage> messages { get; set; }
    }

    public class WhatsAppMessage
    {
        public string from { get; set; }
        public WhatsAppText text { get; set; }
    }

    public class WhatsAppText
    {
        public string body { get; set; }
    }
}