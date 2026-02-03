#nullable disable
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models
{
    public partial class PlcSetup
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Ip { get; set; }

        [Required]
        [StringLength(50)]
        public string Prefix { get; set; }

        public bool IsActive { get; set; }

        [Required]
        [StringLength(20)]
        public string AddedBy { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime AddedDate { get; set; }

        [StringLength(50)]
        public string UpdatedBy { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? UpdatedDate { get; set; }
    }
}
