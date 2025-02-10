

namespace ServerPDV.Models
{
    public class Item
    {
  
        public int Id { get; set; }
      
        public string Descricao { get; set; }
       
        public string Unidade { get; set; }
       
        public decimal PrecoUnit { get; set; }
       
        public int EstoqueInterno { get; set; }
       
        public int EstoqueGondola { get; set; }

    }

}
