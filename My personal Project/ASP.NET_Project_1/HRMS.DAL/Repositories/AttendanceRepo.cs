using HRMS.DAL.Data;
using HRMS.DAL.Repositories.Interfaces;
using HRMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMS.DAL.Repositories
{
    public class AttendanceRepo : IAttendanceRepo
    {
        private readonly HRMSDbContext _context;

        public AttendanceRepo(HRMSDbContext context)
        {
            _context = context;
        }
        public async Task<List<Attendance>> GetAttendanceByUserIdAsync(int userId)
        {
            return await _context.Attendance
                .Where(a => a.UserId == userId)
                .OrderBy(a => a.Date)
                .ToListAsync();
        }
        public async Task<Attendance> GetAttendanceByIdAsync(int id)
        {
            return await _context.Attendance.FindAsync(id);
        }

      
      
    }
}