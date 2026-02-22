using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRMS.Domain.Entities
{
    public class Deductions
    {
        [Key]
        public int DeductionId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        [MaxLength(200)]
        public string DeductionType { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal DeductionAmount { get; set; }

        [MaxLength(500)]
        public string? DeductionDescription { get; set; }

        [Required]
        [Range(1, 12)]
        public int DeductionMonth { get; set; }

        [Required]
        [Range(2000, 2100)]
        public int DeductionYear { get; set; }

        public DateTime DeductionCreatedAt { get; set; } = DateTime.UtcNow;

       
        [ForeignKey("UserId")]
        public virtual Users Users { get; set; } = null!;

    }
}