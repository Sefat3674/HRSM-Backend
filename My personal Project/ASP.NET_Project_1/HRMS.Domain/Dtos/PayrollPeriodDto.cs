using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HRMS.Domain.Entities
{
    public class PayrollPeriodDto
    {
        
        public int PayrollPeriodId { get; set; }
        public string PayrollCode { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public string Status { get; set; }
        public int? TotalEmployees { get; set; }
        public decimal? TotalBasicAmount { get; set; }
        public decimal? TotalBonusAmount { get; set; }
        public decimal TotalDeductionAmount { get; set; }
        public decimal? TotalNetSalaryAmount { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? ProcessedBy { get; set; }
        public DateTime? ProcessedAt { get; set; }
        public int? ApprovedBy { get; set; }
        public DateTime? ApprovedAt { get; set; }
        public bool IsLocked { get; set; }

    }
}