using HRMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;


namespace HRMS.DAL.Repositories.Interfaces
{
    public interface ISalaryStructureRepo
    {

        Task<List<UserSalaryDto>> GetAllSalaryStructuresAsync(int? UserId);
        Task<int> UpsertSalaryStructureAsync(UserSalaryDto dto);
        Task<int> InsertSalaryBonusAsync(UserSalaryDto dto);
        Task<int> InsertSalaryDeductionAsync(UserSalaryDto dto);



    }
}