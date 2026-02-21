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

        public async Task<List<UserSalaryDto>> GetAllSalaryStructuresAsync(int? userId)
        {
            var query =
                from u in _context.Users
                join s in _context.SalaryStructure
                    .Where(x => !x.IsDeleted)
                    on u.UserId equals s.UserId into gj
                from sub in gj.DefaultIfEmpty()
                select new UserSalaryDto
                {
                    SalaryStructureId = sub != null ? sub.SalaryStructureId : 0,
                    UserId = u.UserId,
                    UserName = u.UserName,
                    otherAllowance = sub != null ? sub.otherAllowance : 0,
                    BasicSalary = sub != null ? sub.BasicSalary : 0,
                    HouseRentAllowance = sub != null ? sub.HouseRentAllowance : 0,
                    MedicalAllowance = sub != null ? sub.MedicalAllowance : 0,
                    TransportAllowance = sub != null ? sub.TransportAllowance : 0,
                    EffectiveTo = sub != null ? sub.EffectiveTo : null,
                    IsActive = sub != null ? sub.IsActive : false,
                    IsDeleted = sub != null ? sub.IsDeleted : false
                };

            if (userId.HasValue)
                query = query.Where(x => x.UserId == userId.Value);

            return await query.ToListAsync();
        }

    }
}