using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRMS.Domain.Entities
{
    public class Users
    {
        [Key]
        public int UserId { get; set; }

        [Required, StringLength(100)]
        public string UserName { get; set; }

        [Required, StringLength(255)]
        public string PasswordHash { get; set; }
       
        [Required]
        public int RoleId { get; set; }

            public bool IsActive { get; set; } = true;

        public DateTime CreateAt { get; set; } = DateTime.Now;

        // Navigation
        [ForeignKey("RoleId")]
        public Roles Role { get; set; }

        public UserProfile UserProfile { get; set; } // One-to-one
        public ICollection<Attendance> Attendance { get; set; } = new List<Attendance>();
        public ICollection<SalaryStructure> SalaryStructure { get; set; } = new List<SalaryStructure>();
        public ICollection<SalaryRevisions> SalaryRevisions { get; set; } = new List<SalaryRevisions>();
        public virtual ICollection<SalaryRevisions> ApprovedSalaryRevisions { get; set; } = new List<SalaryRevisions>();
        public ICollection<Deductions> Deductions { get; set; } = new List<Deductions>();
        public ICollection<Bonuses> Bonuses { get; set; } = new List<Bonuses>();
    }
}