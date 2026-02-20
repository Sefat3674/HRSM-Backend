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
    public class SalaryStructureRepo : ISalaryStructureRepo
    {
        private readonly HRMSDbContext _context;

        public SalaryStructureRepo(HRMSDbContext context)
        {
            _context = context;
        }
      
        public async Task<List<UserSalaryDto>> GetAllSalaryStructuresAsync()
        {
            var result = await (
                from u in _context.Users
                join s in _context.SalaryStructure on u.UserId equals s.UserId into gj
                from sub in gj.DefaultIfEmpty()
                select new UserSalaryDto
                {
                    SalaryStructureId = sub != null ? sub.UserId : 0,
                    UserId = u.UserId,
                    UserName = u.UserName,
                    BasicSalary = sub != null ? sub.BasicSalary : 0,
                    HouseRentAllowance = sub != null ? sub.HouseRentAllowance : 0,
                    MedicalAllowance = sub != null ? sub.MedicalAllowance : 0,
                    TransportAllowance = sub != null ? sub.TransportAllowance : 0,
                    otherAllowance = sub != null ? sub.otherAllowance : 0,
                    EffectiveFrom = sub != null ? sub.EffectiveFrom : DateOnly.MinValue,
                    EffectiveTo = sub != null ? sub.EffectiveTo : (DateOnly?)null,
                    IsActive = sub != null ? sub.IsActive : false
                    
                }).ToListAsync();
                return result;
        }

    }
}