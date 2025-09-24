using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BullearApp.Models
{
    [Table("Trades")]
    public class Trade
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Column("TradeDateTime")]
        public DateTime TradeDateTime { get; set; }

        [Required]
        [StringLength(30)]
        public string Symbol { get; set; } = string.Empty;

        [Required]
        [StringLength(5)]
        public string Action { get; set; } = string.Empty;

        [Required]
        public int Quantity { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; } = 0;

        [Required]
        [StringLength(50)]
        public string Spread { get; set; } = string.Empty;

        [Required]
        public DateTime Expiration { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal StrikePrice { get; set; }

        [Required]
        [StringLength(50)]
        public string OptionType { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Commission { get; set; } = 0;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Fee { get; set; } = 0;

        [StringLength(50)]
        public string? PortfolioId { get; set; }
    }
}
