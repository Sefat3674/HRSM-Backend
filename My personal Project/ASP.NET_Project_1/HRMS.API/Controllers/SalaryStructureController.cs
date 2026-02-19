using BCrypt.Net;
using HRMS.API.DTOs;
using HRMS.DAL.Data;
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

        public SalaryStructureController(HRMSDbContext context)
        {
            _context = context;
        }

        
        
      
        
    }
}