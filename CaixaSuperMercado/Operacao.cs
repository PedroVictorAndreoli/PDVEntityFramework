using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaixaSuperMercado
{
    internal class Operacao
    {
        public Produto produto { get; set; }
        public int tipoOperacao { get; set; }
        public int qtdeOperacao {get; set; }

        public Operacao()
        {

        }

    }
}
