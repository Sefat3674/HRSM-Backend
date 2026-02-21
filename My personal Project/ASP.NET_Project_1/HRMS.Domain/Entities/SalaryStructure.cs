using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRMS.Domain.Entities
{
    public class SalaryStructure
    {
        [Key]
        public int SalaryStructureId { get; set; }
        public int UserId { get; set; }

        public decimal BasicSalary { get; set; }
        public decimal HouseRentAllowance { get; set; }
        public decimal MedicalAllowance { get; set; }

        public decimal TransportAllowance { get; set; }

        public decimal otherAllowance { get; set; }

        public DateOnly? EffectiveFrom { get; set; }
        public DateOnly? EffectiveTo { get; set; }

        public bool IsActive { get; set; } = true;

        public bool IsDeleted { get; set; } = false;
        public Users? Users { get; set; } = null!;





    }
}