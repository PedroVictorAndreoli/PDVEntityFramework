using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ServerPDV.Models
{
    public class Item
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }
        [Required]
        [Column("descricao")]
        public string Descricao { get; set; }
        [Required]
        [Column("unidade")]
        public string Unidade { get; set; }
        [Required]
        [Column("precounit")]
        public decimal PrecoUnit { get; set; }
        [Required]
        [Column("estoqueinterno")]
        public int EstoqueInterno { get; set; }
        [Required]
        [Column("estoquegondola")]
        public int EstoqueGondola { get; set; }

    }

}
