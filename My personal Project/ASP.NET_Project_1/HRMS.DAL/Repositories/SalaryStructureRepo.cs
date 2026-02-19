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
        
       

      
      
    }
}