using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bullear.Models
{
    [Table("Positions")]
    public class Position
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [StringLength(50)]
        public string? Status { get; set; }

        [Required]
        [StringLength(50)]
        public string Spread { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Return { get; set; } = 0;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal AvgEntryPrice { get; set; } = 0;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal AvgExitPrice { get; set; } = 0;

        [Required]
        public int ContractSize { get; set; }

        [StringLength(50)]
        public string? PortfolioId { get; set; }

        [Required]
        [Column("OpenDateTime")]
        public DateTime OpenDateTime { get; set; }

        [Column("CloseDateTime")]
        public DateTime? CloseDateTime { get; set; }
    }
}
