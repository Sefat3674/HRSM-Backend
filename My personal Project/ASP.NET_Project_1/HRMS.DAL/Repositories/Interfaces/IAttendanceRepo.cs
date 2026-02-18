using HRMS.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRMS.DAL.Repositories.Interfaces
{
    public interface IAttendanceRepo
    {
     
        Task<List<Attendance>> GetAttendanceByUserIdAsync(int userId);

        /// <summary>
      
        
    }
}