
using System;

namespace ServerPDV.Models
{
    public class Cupom
    {
        public int Id { get; set; }
        public DateTime DtEmissao { get; set; }
        public decimal TotalVenda { get; set; }
        public string CPF { get; set; }
    }

}
