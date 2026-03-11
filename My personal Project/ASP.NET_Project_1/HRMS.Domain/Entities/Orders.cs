using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HRMS.Domain.Entities
{
    public class Orders
    {
        [Key]
     
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string Phone { get; set; } = string.Empty;

        public DateTime OrderDate { get; set; } = DateTime.Now;

        // Calculated properties
        public int TotalQuantity { get; set; }
        public decimal TotalCost { get; set; }

        public bool IsPaid { get; set; } = false;

        // Navigation property
        public ICollection<OrderItems> OrderItems { get; set; } = new List<OrderItems>();
        public void CalculateTotals()
        {
            TotalQuantity = 0;
            TotalCost = 0;

            foreach (var item in OrderItems)
            {
                item.TotalPrice = item.Quantity * item.UnitPrice;

                TotalQuantity += item.Quantity;
                TotalCost += item.TotalPrice;
            }
        }
    }
}