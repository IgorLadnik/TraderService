using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TraderModelLib.Models
{
    [Table("TraderToCurrency")]
    public class TraderToCurrency
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [Required]
        public int TraderId { get; set; }

        [Required]
        public int CurrencyId { get; set; }
    }
}



