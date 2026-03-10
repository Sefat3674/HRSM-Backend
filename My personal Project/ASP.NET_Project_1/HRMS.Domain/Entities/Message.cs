using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HRMS.Domain.Entities
{
    public class Message
    {
        public long Id { get; set; }
        public string PhoneNumber { get; set; }
        public string MessageText { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}