using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServerPDV.Models
{
    [Table("Cupom")]
    public class Cupom
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }
        [Column("DtEmissao")]
        public DateTime DtEmissao { get; set; }
        [Column("TotalVenda")]
        public decimal TotalVenda { get; set; }
        [Column("CPF")]
        public string CPF { get; set; }
    }

}
