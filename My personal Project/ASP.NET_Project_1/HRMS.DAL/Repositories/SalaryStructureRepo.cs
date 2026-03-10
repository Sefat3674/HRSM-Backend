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
        public async Task<PayrollPeriodDto> CreatePayrollPeriod(PayrollPeriodDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            // Optional: check duplicates
            bool exists = await _context.PayrollPeriod
                .AnyAsync(p => p.Month == dto.Month && p.Year == dto.Year);

            if (exists)
                throw new Exception("Payroll period already exists for this month and year.");

            var newPayrollPeriod = new PayrollPeriod
            {
                PayrollCode = dto.PayrollCode,
                Month = dto.Month,
                Year = dto.Year,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                Status = dto.Status,
                IsLocked = false,
                CreatedBy = dto.CreatedBy,
                CreatedAt = DateTime.UtcNow
            };

            _context.PayrollPeriod.Add(newPayrollPeriod);
            await _context.SaveChangesAsync();

            // Map entity back to DTO
            var resultDto = new PayrollPeriodDto
            {
                PayrollPeriodId = newPayrollPeriod.PayrollPeriodId,
                PayrollCode = newPayrollPeriod.PayrollCode,
                Month = newPayrollPeriod.Month,
                Year = newPayrollPeriod.Year,
                StartDate = newPayrollPeriod.StartDate,
                EndDate = newPayrollPeriod.EndDate,
                Status = newPayrollPeriod.Status,
                IsLocked = newPayrollPeriod.IsLocked,
                CreatedBy = newPayrollPeriod.CreatedBy,
                CreatedAt = newPayrollPeriod.CreatedAt
            };

            return resultDto;
        }
        public async Task<List<PayrollPeriodDto>> GetAllPayrollPeriods()
        {
            var payrollPeriods = await _context.PayrollPeriod
                .OrderByDescending(p => p.Year)
                .ThenByDescending(p => p.Month)
                .ToListAsync();

            // Map entities to DTOs
            var dtoList = payrollPeriods.Select(p => new PayrollPeriodDto
            {
                PayrollPeriodId = p.PayrollPeriodId,
                PayrollCode = p.PayrollCode,
                Month = p.Month,
                Year = p.Year,
                StartDate = p.StartDate,
                EndDate = p.EndDate,
                TotalEmployees=p.TotalEmployees,
                TotalBasicAmount=p.TotalBasicAmount,
                TotalBonusAmount=p.TotalBonusAmount,
                TotalDeductionAmount=p.TotalDeductionAmount,
                TotalNetSalaryAmount=p.TotalNetSalaryAmount,
                Status = p.Status,
                IsLocked = p.IsLocked,
                CreatedBy = p.CreatedBy,
                CreatedAt = p.CreatedAt
            }).ToList();

            return dtoList;
        }
        public async Task<List<SalarySlipDto>> GetSalarySlipsAsync(int? userId = null, int? month = null, int? year = null)
        {
            var query = _context.SalarySlip.AsQueryable();

            // Apply optional filters
            if (userId.HasValue)
                query = query.Where(x => x.UserId == userId.Value);

            if (month.HasValue)
                query = query.Where(x => x.SalaryMonth == month.Value);

            if (year.HasValue)
                query = query.Where(x => x.SalaryYear == year.Value);

            var result = await query
                .Select(ss => new SalarySlipDto
                {
                    UserId = ss.UserId,
                    UserName = ss.Users.UserName, // Assuming SalarySlip has navigation property User
                    SalaryMonth = ss.SalaryMonth,
                    MonthName = new[] { "", "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" }[ss.SalaryMonth],
                    SalaryYear = ss.SalaryYear,
                    BasicSalary = ss.BasicSalary,
                    HouseRentAllowance = ss.HouseRentAllowance,
                    MedicalAllowance = ss.MedicalAllowance,
                    OtherAllowance = ss.OtherAllowance,
                    TotalBonus = ss.TotalBonus,
                    TotalDeduction = ss.TotalDeduction,
                    NetSalary = ss.NetSalary,

                    BonusDetails = string.Join(", ",
                        _context.Bonuses
                            .Where(b => b.UserId == ss.UserId
                                     && b.BonusMonth == ss.SalaryMonth
                                     && b.BonusYear == ss.SalaryYear)
                            .Select(b => b.BonusType + "(" + b.BonusAmount + ")")),

                    DeductionDetails = string.Join(", ",
                        _context.Deductions
                            .Where(d => d.UserId == ss.UserId
                                     && d.DeductionMonth == ss.SalaryMonth
                                     && d.DeductionYear == ss.SalaryYear)
                            .Select(d => d.DeductionType + "(" + d.DeductionAmount + ")"))
                })
                .ToListAsync();

            return result;
        }



    }
}