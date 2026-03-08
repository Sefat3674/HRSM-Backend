using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRMS.Domain.Entities
{
    public class Retailer
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string Phone { get; set; } = string.Empty;

        [Column("User_id")]           // Maps to SQL column User_id
        public int? UserId { get; set; }

        [Column("Shop_name")]         // Maps to SQL column Shop_name
        [MaxLength(150)]
        public string? ShopName { get; set; }

        [Column("Owner_name")]        // Maps to SQL column Owner_name
        [MaxLength(150)]
        public string? OwnerName { get; set; }

        public string? Address { get; set; }

        [Column("Created_at")]        // Maps to SQL column Created_at
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        
    }
}