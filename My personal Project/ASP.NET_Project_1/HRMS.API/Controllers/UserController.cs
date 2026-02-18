using HRMS.DAL.Data;
using HRMS.DAL.Repositories.Interfaces;
using HRMS.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace HRMS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly HRMSDbContext _context;
        private readonly IAttendanceRepo _attendanceRepo;

        // Inject both DbContext and AttendanceRepo
        public UserController(HRMSDbContext context, IAttendanceRepo attendanceRepo)
        {
            _context = context;
            _attendanceRepo = attendanceRepo;
        }

        // GET: api/User/Attendance/1
        [HttpGet("Attendance/{userId}")]
        
        public async Task<IActionResult> GetUserAttendance(int userId)
        {
            // Use repository for attendance
            var attendanceList = (await _attendanceRepo.GetAttendanceByUserIdAsync(userId)).ToList();

            if (attendanceList == null || !attendanceList.Any())
                return NotFound();

            return Ok(attendanceList);
        }
    }
}