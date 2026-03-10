using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HRMS.Domain.Entities
{
    public class Catagories
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; } 
        public bool IsActived { get; set; }
        public DateTime? Date { get; set; }
     
        public String? Status { get; set; }
        public Users Users { get; set; } = null!;
    }
}