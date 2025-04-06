
using CaixaSuperMercado.Service;
using ServerPDV.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CaixaSuperMercado
{
    public partial class FormCaixa : Form
    {

        private ServiceCrud serviceCrud;
        private Login log;
        private List<Produto> listaProdutos;
        private float soma = 0;
        private List<Operacao> listaOperacao;

        //variáveis de controle
        private bool logado = false;
        private bool iniciouTransacao = false;
        private bool finalizado = true;

        public FormCaixa()
        {
            InitializeComponent();
            codigo.Enabled = false;
            produto.Enabled = false;
            preco.Enabled = false;
            quantidade.Enabled = false;
            total.Enabled = false;
            serviceCrud = new ServiceCrud();
            listaOperacao = new List<Operacao>();
            listaProdutos = new List<Produto>();

            log = new Login();
            
        }

        #region Ajustar campos
        public void ajustaCampos()
        { //método de configuração inicial de transação
            codigo.Enabled = true;
            quantidade.Enabled = true;
            fitaCaixa.Text = "";
            fitaCaixa.AppendText("Supermercado EJR\n");
            fitaCaixa.AppendText("CNPJ 00.000.000/0000-00\n");
            fitaCaixa.AppendText("-------------------------------------------------------- ----------\n");
        }
        #endregion

        #region Reajustar campos
        public void reajustaCampos()
        {
            codigo.Enabled = false;
            quantidade.Enabled = false;
            total.Text = "";
        }
        #endregion

        private async void codigo_TextChanged(object sender, EventArgs e)
        {
            #region Tratamento campo código
            if (int.TryParse(codigo.Text, out int aux))
            {

                
                var itemTable = (await serviceCrud.getObjeto<Item>(aux, "item"));

                if (itemTable != null)
                {
                    if (itemTable.EstoqueGondola == 0)
                        MessageBox.Show("Este item está esgotado.");
                    else
                    {
                        produto.Text = itemTable.Descricao;
                        var a = Math.Round(itemTable.PrecoUnit, 2);
                        preco.Text = a.ToString();
                    }
                }
                else
                {
                    MessageBox.Show("Este código não está cadastrado.");
                    codigo.Text = "";
                }
            }
            else if (codigo.Text == null || codigo.Text == "")
            {
                produto.Text = "";
                preco.Text = "";
            }
            else
            {
                codigo.Text = "";
                MessageBox.Show("Favor digitar apenas números para código.");
            }
            #endregion
        }

        private void quantidade_TextChanged(object sender, EventArgs e)
        {
            #region Tratamento campo quantidade
            if (!int.TryParse(quantidade.Text, out _) && !quantidade.Text.Equals("".Trim()))
            {
                MessageBox.Show("Insira apenas números neste campo.");
                codigo.Text = "";
                quantidade.Text = "";
                codigo.Focus();
            }
            #endregion
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                #region Login
                case Keys.F11: //Form login
                    if (!logado)
                    {
                        log.ShowDialog(this);
                        if (log.username != null)
                        {
                            labelLogin.Text = log.username;
                            logado = true;
                        }
                    }
                    break;
                #endregion

                #region Iniciar Transação
                case Keys.F12: //Habilitar transação caixa
                    if (logado && !iniciouTransacao)
                    {
                        finalizado = false;
                        iniciouTransacao = true;
                        ajustaCampos();
                        codigo.Focus();
                    }
                    break;
                #endregion

                #region Adicionar
                case Keys.F1: //Adicionar
                    if (logado && iniciouTransacao)
                    {
                        try
                        {
                            var qtde = listaProdutos.Where(x => x.Codigo == int.Parse(codigo.Text)).ToList().Count;
                            if (qtde > 0) //Entra no if se o produto estiver na lista
                            {
                                var produto = listaProdutos.Where(x => x.Codigo == int.Parse(codigo.Text)).First();

                                var validaGondolaResult = Task.Run(() => validarGondola(produto, produto.Quantidade + int.Parse(quantidade.Text)));
                                if (validaGondolaResult.Result)
                                {
                                    listaProdutos.Where(x => x.Codigo == int.Parse(codigo.Text)).First().Quantidade += int.Parse(quantidade.Text);
                                    CarregarFita(listaProdutos.Where(x => x.Codigo == int.Parse(codigo.Text)).First(), "Adicionar", int.Parse(quantidade.Text));
                                }
                                else
                                    MessageBox.Show("Quantidade nova superior à da gôndola. Favor revisar.");

                            }
                            else
                            {
                                Produto itemProduto = new Produto(int.Parse(codigo.Text), produto.Text, int.Parse(quantidade.Text), float.Parse(preco.Text));

                                var validaGondolaResult =  Task.Run(() => validarGondola(itemProduto, int.Parse(quantidade.Text)));
                                if (validaGondolaResult.Result)
                                {
                                    listaProdutos.Add(itemProduto);
                                    CarregarFita(itemProduto, "Adicionar", int.Parse(quantidade.Text));
                                }
                                else
                                    MessageBox.Show("Quantidade superior à da gôndola. Favor revisar.");
                            }
                        }
                        catch (FormatException ex)
                        {
                            MessageBox.Show("Insira um código e quantidade em números antes de prosseguir.");
                        }

                        codigo.Text = "";
                        quantidade.Text = "";
                        codigo.Focus();
                    }
                    break;
                #endregion

                #region Estorno
                case Keys.F5: //Estorno
                    if (logado && iniciouTransacao)
                    {
                        var achou = false;
                        foreach (Produto itemProduto in listaProdutos)
                        {
                            if (itemProduto.Codigo.ToString().Equals(codigo.Text) &&                                 
                                !achou)
                            {
                                if (quantidade.Text.Equals(""))
                                {
                                    MessageBox.Show("Quantidade não informada.");
                                    break;
                                }
                                achou = true;
                                if (itemProduto.Quantidade > int.Parse(quantidade.Text))
                                {
                                    itemProduto.Quantidade -= int.Parse(quantidade.Text);
                                    CarregarFita(itemProduto, "Remover", int.Parse(quantidade.Text));
                                }
                                else if (itemProduto.Quantidade == int.Parse(quantidade.Text))
                                {
                                    Produto produtoRemovido = itemProduto;
                                    CarregarFita(produtoRemovido, "Remover", int.Parse(quantidade.Text));
                                    listaProdutos.Remove(itemProduto);
                                    break;
                                } 
                                else
                                    MessageBox.Show("Quantidade de estorno maior que a comprada.");
                            } 
                        }

                        if (!achou)
                            MessageBox.Show("Produto não encontrado na lista do caixa.");

                        codigo.Focus();
                    }
                    
                    codigo.Text = "";
                    quantidade.Text = "";
                    break;
                #endregion

                #region Encerrar Transação
                case Keys.F10: //Finalizar
                    if (!finalizado)
                    {
                        if (listaProdutos.Count > 0)
                        {
                            #region Atualiza o estoque
                            var estoqueConcluido = false;
                            //Tratando estoque
                            foreach(var produto in listaProdutos)
                            {

                                var tabelaItem = Task.Run(() => serviceCrud.getObjeto<Item>(produto.Codigo, "item").Result).Result;

                                if (tabelaItem.EstoqueGondola > 0 && produto.Quantidade <= tabelaItem.EstoqueGondola)
                                {
                                    tabelaItem.EstoqueGondola -= produto.Quantidade;
                                    serviceCrud.salvarObjeto<Produto>(tabelaItem, "item");
                                    estoqueConcluido = true;
                                }
                                else if (tabelaItem.EstoqueInterno > 0 && produto.Quantidade <= tabelaItem.EstoqueInterno)
                                {
                                    tabelaItem.EstoqueInterno -= produto.Quantidade;
                                    if (tabelaItem.EstoqueGondola > 0)
                                    {
                                        tabelaItem.EstoqueInterno += tabelaItem.EstoqueGondola;
                                        tabelaItem.EstoqueGondola = 0;
                                    }

                                    serviceCrud.salvarObjeto<Produto>(tabelaItem, "item");
                                    estoqueConcluido = true;
                                }
                                else if (tabelaItem.EstoqueGondola > 0 && produto.Quantidade <= tabelaItem.EstoqueGondola)
                                {
                                    tabelaItem.EstoqueGondola -= (short)produto.Quantidade;
                                    if (tabelaItem.EstoqueInterno > 0)
                                    {
                                        tabelaItem.EstoqueGondola += (short)tabelaItem.EstoqueInterno;
                                        tabelaItem.EstoqueInterno = 0;
                                    }
                                    else if (tabelaItem.EstoqueInterno > 0)
                                    { // Caso não tenha itens no estoque interno mas tenha na unidade
                                        tabelaItem.EstoqueGondola += (short)tabelaItem.EstoqueInterno;
                                        tabelaItem.EstoqueInterno = 0;
                                    }
                                    serviceCrud.salvarObjeto<Produto>(tabelaItem, "item");
                                    estoqueConcluido = true;
                                }
                                else
                                {
                                    MessageBox.Show("Não há estoque o suficiente para realizar a compra do produto: " + produto.Nome);
                                    estoqueConcluido = false;
                                    break;
                                }
                            }
                            #endregion


                            #region Salva os produtos

                            if (estoqueConcluido)
                            {
                                Cupom cupom = new Cupom();  
                                cupom.DtEmissao = DateTime.Now;
                                cupom.CPF = "123";
                                cupom.TotalVenda = decimal.Parse(soma.ToString("N2"));
                                cupom = Task.Run(() => serviceCrud.salvarObjeto<Cupom>(cupom, "cupom").Result).Result;

                                var usuario = log.login;

                                foreach (Produto produto in listaProdutos)
                                {
                                    CupomItem cupomItem = new CupomItem();
                                    cupomItem.cupomID = cupom.Id;
                                    cupomItem.itemID = produto.Codigo;
                                    cupomItem.Qtde = produto.Quantidade;
                                    cupomItem.PrecoUnit = decimal.Parse(produto.Preco.ToString());
                                    cupomItem.TotalItem = decimal.Parse((produto.Quantidade * produto.Preco).ToString());
                                    cupomItem.Uid = usuario.UId;
                                    serviceCrud.salvarObjeto<CupomItem>(cupomItem, "cupomItem");

                                   
                                }

                                FinalizarFita();
                                reajustaCampos();
                            }
                            #endregion
                        }
                        else
                        {
                            var aviso = MessageBox.Show("A sua lista de compras está vazia! Deseja mesmo encerrar?", "Aviso", MessageBoxButtons.YesNo);
                            if (aviso == DialogResult.Yes)
                            {
                                FinalizarFita();
                                reajustaCampos();
                            }
                        }
                    }
                    break;
                #endregion

                #region Fechar programa
                case Keys.Escape: //Fechar caixa
                    if (!iniciouTransacao)
                        this.Close();
                    break;
                #endregion

                default: break;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        #region Carrega a fita de mercado
        private void CarregarFita(Produto produto, string op, int qtde)
        {
            var operacao = new Operacao
            {
                produto = produto,
                tipoOperacao = op.Equals("Adicionar") ? 1 : 0, // Adicionar | Estornar
                qtdeOperacao = qtde
            };
            listaOperacao.Add(operacao);

            fitaCaixa.Text = "";
            fitaCaixa.AppendText("Supermercado EJR\n");
            fitaCaixa.AppendText("CNPJ 00.000.000/0000-00\n");
            fitaCaixa.AppendText("-------------------------------------------------------- ----------\n");
            soma = 0;

            foreach (var ope in listaOperacao)
            {
                if (ope.tipoOperacao == 1) //Adicionar
                {
                    #region Verificação quebra de linha e impressão na fita
                    var dados = ope.produto.Codigo.ToString("000 ") + ope.produto.Nome + " " + ope.qtdeOperacao.ToString() + " x " + ope.produto.Preco.ToString("C2");
                    if (dados.Length > 56)
                    {
                        var dados2 = dados.Substring(56);
                        var dados3 = dados.Remove(56);
                        fitaCaixa.AppendText(dados3 + "\n" + dados2.PadRight(58));
                        //fitaCaixa.AppendText(dados + "\n" + dados.Substring(35).PadRight(58));
                        fitaCaixa.AppendText((ope.qtdeOperacao * ope.produto.Preco).ToString("N2") + "+\n".PadLeft(3));
                        soma += ope.qtdeOperacao * ope.produto.Preco;
                    }
                    else
                    {
                        fitaCaixa.AppendText(dados.PadRight(58));
                        fitaCaixa.AppendText((ope.qtdeOperacao * ope.produto.Preco).ToString("N2") + "+\n".PadLeft(3));
                        soma += ope.qtdeOperacao * ope.produto.Preco;
                    }
                    #endregion

                }
                else if (ope.tipoOperacao == 0) //Estornar
                {
                    #region Verificação quebra de linha e impressão na fita
                    var dados = ope.produto.Codigo.ToString("000 ") + "Est: " + ope.produto.Nome + " " + (-1 * ope.qtdeOperacao).ToString() + " x " + ope.produto.Preco.ToString("C2");
                    if (dados.Length > 56)
                    {
                        var dados2 = dados.Substring(56);
                        var dados3 = dados.Remove(56);
                        fitaCaixa.AppendText(dados3 + "\n" + dados2.PadRight(58));
                        fitaCaixa.AppendText((ope.qtdeOperacao * ope.produto.Preco).ToString("N2") + "-\n".PadLeft(3));
                        soma -= ope.qtdeOperacao * ope.produto.Preco;
                    }
                    else
                    {
                        fitaCaixa.AppendText(dados.PadRight(58));
                        fitaCaixa.AppendText((ope.qtdeOperacao * ope.produto.Preco).ToString("N2") + "-\n".PadLeft(3));
                        soma -= ope.qtdeOperacao * ope.produto.Preco;
                    }
                    #endregion

                }
            }

            if (soma <= 0)
                total.Text = "";
            else
                total.Text = soma.ToString("C2");
        }
        #endregion

        #region Finaliza a fita de mercado
        private void FinalizarFita()
        {
            fitaCaixa.AppendText("-------------------------------------------------------- ----------\n");
            if (listaProdutos.Count > 0)
                fitaCaixa.AppendText("Total geral".PadRight(58) + total.Text);
            else
                fitaCaixa.AppendText("Total geral".PadRight(58) + "R$ 0,00");
            iniciouTransacao = false;
            finalizado = true;
            listaProdutos.Clear();
            listaOperacao.Clear();
            soma = 0;
        }
        #endregion

        #region Validação de item no estoque
        private async Task<bool> validarGondola(Produto produto, int qtde)
        {
            var itemTable = (await serviceCrud.getObjeto<Item>(produto.Codigo, "item"));

            if (itemTable.EstoqueGondola >= qtde)
                return true;
            else
                return false;
        }
        #endregion
    }
}
