using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HRMS.Domain.Entities
{
    public class Categories
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; } 
        public bool IsActive { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public ICollection<SubCategories> SubCategories { get; set; } = new List<SubCategories>();


    }
}