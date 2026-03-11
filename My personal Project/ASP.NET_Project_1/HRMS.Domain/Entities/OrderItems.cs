using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRMS.Domain.Entities
{
    public class OrderItems
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int OrderId { get; set; }

        [ForeignKey("OrderId")]
        public Orders Orders { get; set; } = null!;

        [Required]
        public int SubCategoryId { get; set; }

        [ForeignKey("SubCategoryId")]
        public SubCategories SubCategories { get; set; } = null!;

        [Required]
        public int Quantity { get; set; }

        [Required]
        public decimal UnitPrice { get; set; }

        // Calculated field
        public decimal TotalPrice { get; set; }
    }
}