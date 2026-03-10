using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HRMS.Domain.Entities
{
    public class WhatsAppSessions
    {
        [Key]
        public string Phone { get; set; }

        public string CurrentStep { get; set; }

        public string TempData { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}