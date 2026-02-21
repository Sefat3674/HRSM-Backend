using BCrypt.Net;
using HRMS.API.DTOs;
using HRMS.DAL.Data;
using HRMS.DAL.Repositories;
using HRMS.DAL.Repositories.Interfaces;
using HRMS.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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







    }
}