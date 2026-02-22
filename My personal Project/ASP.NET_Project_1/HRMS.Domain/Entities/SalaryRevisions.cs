using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRMS.Domain.Entities
{
    public class SalaryRevisions
    {
        [Key]
        public int RevisionId { get; set; }
        public int UserId { get; set; }
        public decimal OldSalary { get; set; }
        public decimal NewSalary { get; set; }
        public DateOnly RevisionDate { get; set; }
        public string? Reason { get; set; }
        public int ? ApprovedBy { get; set; }
        public virtual Users Users { get; set; } = null!;
        public virtual Users? ApprovedByUser { get; set; }

    }
}