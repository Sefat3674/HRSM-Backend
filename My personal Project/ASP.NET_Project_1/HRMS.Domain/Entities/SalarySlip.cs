using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRMS.Domain.Entities
{
    public class SalarySlip
    {
        [Key]
        public int SalarySlipId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal BasicSalary { get; set; } = 0;

        [Column(TypeName = "decimal(18,2)")]
        public decimal HouseRentAllowance { get; set; } = 0;

        [Column(TypeName = "decimal(18,2)")]
        public decimal MedicalAllowance { get; set; } = 0;

        [Column(TypeName = "decimal(18,2)")]
        public decimal TransportAllowance { get; set; } = 0;

        [Column(TypeName = "decimal(18,2)")]
        public decimal OtherAllowance { get; set; } = 0;

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalBonus { get; set; } = 0;

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalDeduction { get; set; } = 0;

        [Column(TypeName = "decimal(18,2)")]
        public decimal NetSalary { get; set; } = 0;

        public bool IsLocked { get; set; } = false;

        [Required]
        [Range(1, 12)]
        public int SalaryMonth { get; set; }

        [Required]
        [Range(2000, 2100)]
        public int SalaryYear { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        /* Navigation Property */
        [ForeignKey("UserId")]
        public virtual Users Users { get; set; } = null!;
        public int PayrollPeriodId { get; set; }

        public virtual PayrollPeriod PayrollPeriod { get; set; } = null!;
    }
}