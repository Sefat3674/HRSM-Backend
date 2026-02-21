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
                    EffectiveFrom = sub != null ? sub.EffectiveFrom :null,
                    EffectiveTo = sub != null ? sub.EffectiveTo : null,
                    IsActive = sub != null ? sub.IsActive : false,
                    IsDeleted = sub != null ? sub.IsDeleted : false,
                    
                };

            if (userId.HasValue)
                query = query.Where(x => x.UserId == userId.Value);

            return await query.ToListAsync();
        }
        public async Task<int> UpsertSalaryStructureAsync(UserSalaryDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            // Find existing salary for this UserId
            var existingSalary = await _context.SalaryStructure
                .FirstOrDefaultAsync(s => s.UserId == dto.UserId && !s.IsDeleted);

            if (existingSalary != null)
            {
                // --- UPDATE existing record ---
                existingSalary.BasicSalary = dto.BasicSalary;
                existingSalary.HouseRentAllowance = dto.HouseRentAllowance;
                existingSalary.MedicalAllowance = dto.MedicalAllowance;
                existingSalary.TransportAllowance = dto.TransportAllowance;
                existingSalary.otherAllowance = dto.otherAllowance;
                existingSalary.EffectiveFrom = dto.EffectiveFrom;
                existingSalary.EffectiveTo = dto.EffectiveTo;
                existingSalary.IsActive = dto.IsActive;
                existingSalary.IsDeleted = dto.IsDeleted;

                _context.SalaryStructure.Update(existingSalary);
                await _context.SaveChangesAsync();

                return existingSalary.SalaryStructureId;
            }

            // --- INSERT new record ---
            var newSalary = new SalaryStructure
            {
                UserId = dto.UserId,
                BasicSalary = dto.BasicSalary,
                HouseRentAllowance = dto.HouseRentAllowance,
                MedicalAllowance = dto.MedicalAllowance,
                TransportAllowance = dto.TransportAllowance,
                otherAllowance = dto.otherAllowance,
                EffectiveFrom = dto.EffectiveFrom,
                EffectiveTo = dto.EffectiveTo,
                IsActive = dto.IsActive,
                IsDeleted = dto.IsDeleted
            };

            _context.SalaryStructure.Add(newSalary);
            await _context.SaveChangesAsync();

            return newSalary.SalaryStructureId;
        }

    }
}