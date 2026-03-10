using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRMS.Domain.Entities
{
    public class Prices
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int SubCategoryId { get; set; }

        [ForeignKey("SubCategoryId")]
        public SubCategories SubCategories { get; set; } = null!;

        [Required]
        public decimal Price { get; set; }

        public DateTime EffectiveDate { get; set; } = DateTime.Now;


    }
}