
namespace ServerPDV.Models
{
    public class CupomItem
    {

        public int Id { get; set; }
        public int cupomID { get; set; }
        public int itemID { get; set; }
        public int Qtde { get; set; }
        public decimal PrecoUnit { get; set; }

        public decimal TotalItem { get; set; }
        public int Uid { get; set; }
    }
}
