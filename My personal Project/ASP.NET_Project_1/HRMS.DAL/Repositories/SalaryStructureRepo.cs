using HRMS.DAL.Data;
using HRMS.DAL.Repositories.Interfaces;
using HRMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

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
            decimal newGrossSalary = dto.BasicSalary
                           + dto.HouseRentAllowance
                           + dto.MedicalAllowance
                           + dto.TransportAllowance
                           + dto.otherAllowance;


            // Find existing salary for this UserId
            var existingSalary = await _context.SalaryStructure
                .FirstOrDefaultAsync(s => s.UserId == dto.UserId && !s.IsDeleted);

            if (existingSalary != null)
            {
                decimal oldGrossSalary = existingSalary.BasicSalary
                               + existingSalary.HouseRentAllowance
                               + existingSalary.MedicalAllowance
                               + existingSalary.TransportAllowance
                               + existingSalary.otherAllowance;
                var revision = new SalaryRevisions
                {
                    UserId = dto.UserId,
                    OldSalary = oldGrossSalary,
                    NewSalary = newGrossSalary,
                    RevisionDate = DateOnly.FromDateTime(DateTime.Now),
                    Reason = dto.Reason ?? "Salary Update",
                    ApprovedBy = dto.ApprovedBy
                };

                _context.SalaryRevisions.Add(revision);

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

            var initialRevision = new SalaryRevisions
            {
                UserId = dto.UserId,
                OldSalary = 0,
                NewSalary = newGrossSalary,
                RevisionDate = DateOnly.FromDateTime(DateTime.Now),
                Reason = dto.Reason ?? "Initial Salary",
                ApprovedBy = dto.ApprovedBy
            };
            _context.SalaryRevisions.Add(initialRevision);

            await _context.SaveChangesAsync();

            return newSalary.SalaryStructureId;
        }
        public async Task<bool> InsertSalaryAdjustmentAsync(SalaryAdjustmentDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            // Insert Bonuses
            if (dto.Bonuses != null && dto.Bonuses.Any())
            {
                foreach (var item in dto.Bonuses)
                {
                    var bonus = new Bonuses
                    {
                        UserId = dto.UserId,
                        BonusType = item.BonusType,
                        BonusAmount = item.Amount,
                        Description = item.Description,
                        BonusMonth = dto.Month,
                        BonusYear = dto.Year
                    };

                    _context.Bonuses.Add(bonus);
                }
            }

            // Insert Deductions
            if (dto.Deductions != null && dto.Deductions.Any())
            {
                foreach (var item in dto.Deductions)
                {
                    var deduction = new Deductions
                    {
                        UserId = dto.UserId,
                        DeductionType = item.DeductionType,
                        DeductionAmount = item.Amount,
                        DeductionDescription = item.Description,
                        DeductionMonth = dto.Month,
                        DeductionYear = dto.Year
                    };

                    _context.Deductions.Add(deduction);
                }
            }

            await _context.SaveChangesAsync();

            return true;
        }
        public async Task<SalaryAdjustmentDto> GetSalaryAdjustmentByUserAsync(int userId)
        {
            var user = await _context.Users
                .Where(u => u.UserId == userId)
                .Select(u => new SalaryAdjustmentDto
                {
                    UserId = u.UserId,
                    

                    Bonuses = u.Bonuses
                        .OrderBy(b => b.BonusYear)
                        .ThenBy(b => b.BonusMonth)
                        .Select(b => new BonusDto
                        {
                            BonusType = b.BonusType,
                            Amount = b.BonusAmount,
                            Description = $"{b.BonusMonth}/{b.BonusYear} - {b.Description}"
                        }).ToList(),

                    Deductions = u.Deductions
                        .OrderBy(d => d.DeductionYear)
                        .ThenBy(d => d.DeductionMonth)
                        .Select(d => new DeductionDto
                        {
                            DeductionType = d.DeductionType,
                            Amount = d.DeductionAmount,
                            Description = $"{d.DeductionMonth}/{d.DeductionYear} - {d.DeductionDescription}"
                        }).ToList()
                })
                .FirstOrDefaultAsync();

            return user;
        }



    }
}