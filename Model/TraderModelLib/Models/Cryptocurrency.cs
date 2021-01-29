using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TraderModelLib.Models
{
    [Table("Cryptocurrencies")]
    public class Cryptocurrency
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        public string Currency { get; set; }

        [Required]
        [StringLength(20)]
        public string Symbol { get; set; }
    }
}


