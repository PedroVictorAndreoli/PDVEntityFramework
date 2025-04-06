using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServerPDV.Models
{
    [Table("CupomItem")]
    public class CupomItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }
        [Column("CupomID")]
        public int cupomID { get; set; }

        [Column("ItemID")]
        public int itemID { get; set; }

        [Column("Qtde")]
        public int Qtde { get; set; }

        [Column("PrecoUnit")]
        public decimal PrecoUnit { get; set; }

        [Column("TotalItem")]
        public decimal TotalItem { get; set; }

        [Column("Uid")]
        public int Uid { get; set; }
    }
}
