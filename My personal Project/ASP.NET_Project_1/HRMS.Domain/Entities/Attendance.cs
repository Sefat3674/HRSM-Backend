using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HRMS.Domain.Entities
{
    public class Attendance
    {
        [Key]
        public int AttendanceId { get; set; }
        public int UserId { get; set; }
        public DateOnly Date { get; set; }
        public TimeOnly? CheckInTime { get; set; }
        public TimeOnly? CheckOutTime { get; set; }
        public String? Status { get; set; }
        public Users Users { get; set; } = null!;
    }
}