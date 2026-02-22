using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRMS.Domain.Entities
{
    public class Bonuses
    {
        [Key]
        public int BonusId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        [MaxLength(200)]
        public string BonusType { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [Range(0, double.MaxValue, ErrorMessage = "Amount must be non-negative")]
        public decimal BonusAmount { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        [Required]
        [Range(1, 12, ErrorMessage = "BonusMonth must be between 1 and 12")]
        public int BonusMonth { get; set; }

        [Required]
        [Range(2000, 2100, ErrorMessage = "BonusYear must be >= 2000")]
        public int BonusYear { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /* Navigation Property */
        [ForeignKey("UserId")]
       
        public virtual Users Users { get; set; } = null!;

    }
}