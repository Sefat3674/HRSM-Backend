using BCrypt.Net;
using HRMS.API.DTOs;
using HRMS.DAL.Data;
using HRMS.DAL.Repositories;
using HRMS.DAL.Repositories.Interfaces;
using HRMS.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace HRMS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SalaryStructureController : ControllerBase
    {
        private readonly HRMSDbContext _context;
        private readonly ISalaryStructureRepo _SalaryStructureRepo;

        public SalaryStructureController(HRMSDbContext context, ISalaryStructureRepo SalaryStructureRepo)
        {
            _context = context;
            _SalaryStructureRepo = SalaryStructureRepo;
        }       
        [HttpGet("getusersalary")]
        public async Task<IActionResult>GetUserSalary(int? UserId = null)
        {
            var result=await _SalaryStructureRepo.GetAllSalaryStructuresAsync(UserId);
            if (result == null || !result.Any())
                return NotFound(new { Message = "No salary data found." });

            return Ok(result);
        }
        
        [HttpPost("upsert/{userId}")]
        public async Task<IActionResult> UpsertSalary(int userId, [FromBody] UserSalaryDto dto)
        {
            if (dto == null)
                return BadRequest("Salary data is required.");

            // Check if user exists
            var userExists = await _context.Users.AnyAsync(u => u.UserId == userId);

            if (!userExists)
                return BadRequest($"User with ID {userId} does not exist.");

            dto.UserId = userId;

            var result = await _SalaryStructureRepo.UpsertSalaryStructureAsync(dto);

            return Ok(new
            {
                Message = "Salary structure upserted successfully.",
                SalaryStructureId = result
            });
        }

       
        [HttpPost("insertSalaryBonusDeduction/{UserId}")]
        public async Task<IActionResult> InsertSalaryBonusDeduction(int UserId, [FromBody] SalaryAdjustmentDto dto)
        {
            dto.UserId = UserId;
            var result = await _SalaryStructureRepo.InsertSalaryAdjustmentAsync(dto);
            if (!result)
                return BadRequest("Failed to insert salary adjustments.");

            return Ok(new
            {
                Message = "Salary bonuses and deductions inserted successfully.",
                UserId = UserId
            });
 }
        [HttpGet("getSalaryAdjustmentByUser")]
        public async Task<IActionResult> GetSalaryAdjustmentByUser([FromQuery] int UserId)
        
        {
            var result = await _SalaryStructureRepo.GetSalaryAdjustmentByUserAsync(UserId);

            if (result == null)
                return NotFound("User not found or no salary records.");

            return Ok(result);
        }

        [HttpPost("insertPayroll")]
        public async Task<IActionResult> InsertPayroll([FromBody] PayrollPeriodDto dto)
        {
            if (dto == null)
                return BadRequest("Payroll data is required.");

            var result = await _SalaryStructureRepo.CreatePayrollPeriod(dto);

            return Ok(new
            {
                Message = "Payroll inserted successfully.",
                Payroll = result // full DTO
            });
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> GetAllPayrollPeriods()
        {
            var payrolls = await _SalaryStructureRepo.GetAllPayrollPeriods();
            return Ok(payrolls);
        }

        [HttpPost("previewPayroll")]
        public IActionResult PreviewPayroll([FromBody] PayrollPreviewRequestDto dto)
        {
            if (dto == null)
                return BadRequest("Request body is required.");

            try
            {
                var connectionString = _context.Database.GetDbConnection().ConnectionString;

                using (SqlConnection conn = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand("dbo.sp_PreviewPayroll", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserId", dto.UserId);
                    cmd.Parameters.AddWithValue("@Month", dto.Month);
                    cmd.Parameters.AddWithValue("@Year", dto.Year);

                    conn.Open();

                    using (var reader = cmd.ExecuteReader())
                    {
                        var rows = new List<Dictionary<string, object>>();

                        while (reader.Read())
                        {
                            var row = new Dictionary<string, object>();
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                row[reader.GetName(i)] = reader.IsDBNull(i) ? null : reader.GetValue(i);
                            }
                            rows.Add(row);
                        }

                        return Ok(rows); // returns JSON
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
            }
    }









    }
