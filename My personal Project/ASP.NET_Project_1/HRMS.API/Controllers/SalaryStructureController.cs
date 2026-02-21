using BCrypt.Net;
using HRMS.API.DTOs;
using HRMS.DAL.Data;
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
        public async Task<IActionResult>GetUserSalary()
        {
            var result=await _SalaryStructureRepo.GetAllSalaryStructuresAsync();
            return Ok(result);
        }
       






    }
}