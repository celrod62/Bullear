using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BullearApp.Models
{
    [Table("FidelityUpload")]
    public class FidelityUpload
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Column("RunDate")]
        public DateTime RunDate { get; set; }

        [Required]
        [StringLength(200)]
        public string Action { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Symbol { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string Description { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Type { get; set; } = string.Empty;

        [Required]
        public int Quantity { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; } = 0;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Commission { get; set; } = 0;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Fees { get; set; } = 0;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal AccruedInterest { get; set; } = 0;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; } = 0;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal CashBalance { get; set; } = 0;

        [Required]
        [Column("SettlementDate")]
        public DateTime SettlementDate { get; set; }
    }
}
