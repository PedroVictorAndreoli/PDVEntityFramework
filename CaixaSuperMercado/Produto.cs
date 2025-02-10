using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CaixaSuperMercado
{
    internal class Produto
    {
        public int Codigo { get; set; }
        public string Nome { get; set; }
        public float Preco { get; set; }
        public int Quantidade { get; set; }

        public Produto()
        {

        }

        public Produto(int codigo, string nome, int quantidade, float preco)
        {
            this.Codigo = codigo;
            this.Nome = nome;
            this.Quantidade = quantidade;
            this.Preco = preco;
        }

   

    }
}
