using CaixaSuperMercado.Service;
using ServerPDV.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CaixaSuperMercado
{
    public partial class Login : Form
    {
        public string username { get; set; }
        private ServiceCrud serviceCrud;
        public Usuario login { get; set; }

        public Login()
        {
            InitializeComponent();
            serviceCrud = new ServiceCrud();
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void entrar_Click(object sender, EventArgs e)
        {

            login = serviceCrud.GetLogin<Usuario>(user.Text).Result;

            if (login != null && pass.Text == login.Senha)
            {
                username = "Caixa - " + login.Nome;
                this.Hide();
            }
            else
                MessageBox.Show("Usuário e/ou senha inválidos.");
        }
    }
}
