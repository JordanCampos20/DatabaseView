using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace DatabaseView
{

    public partial class Tela1 : Form
    {
        public Tela1()
        {
            InitializeComponent();
        }

        MySqlConnection connection = new MySqlConnection("datasource=IP_DA_DATABASE;" +
                                                    "port=PORTA_DA_DATABASE;" +
                                                    "username=USUARIO;" +
                                                    "password=SENHA;" +
                                                    "initial catalog=NOME_DA_TABELA_DA_DATABASE");

        MySqlCommand command = new MySqlCommand();

        MySqlDataReader dt;

        private void ConexaoSQL()
        {
            try
            {
                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    label1.Text = "Connected";
                    label1.ForeColor = Color.Green;
                }
                else
                {
                    label1.Text = "Not Connected";
                    label1.ForeColor = Color.Red;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void DesablitaCampos()
        {
            nomeBox.Enabled = false;
            enderecoBox.Enabled = false;
            emailBox.Enabled = false;
            cpfBox.Enabled = false;
            bairroBox.Enabled = false;
            telefoneBox.Enabled = false;
            cpfBox1.Enabled = true;
            MradioButton.Enabled = false;
            FradioButton.Enabled = false;
            alterarbtn.Enabled = false;
            limparbtn.Enabled = false;
            excluirbtn.Enabled = false;
            adicionarbtn.Enabled = false;
        }

        private void HablitaCampos()
        {
            nomeBox.Enabled = true;
            enderecoBox.Enabled = true;
            emailBox.Enabled = true;
            cpfBox.Enabled = true;
            bairroBox.Enabled = true;
            telefoneBox.Enabled = true;
            cpfBox1.Enabled = true;
            MradioButton.Enabled = true;
            FradioButton.Enabled = true;
            alterarbtn.Enabled = true;
            limparbtn.Enabled = true;
            excluirbtn.Enabled = true;
            adicionarbtn.Enabled = true;
        }

        private void LimpaCampos()
        {
            nomeBox.Text = "";
            enderecoBox.Text = "";
            emailBox.Text = "";
            cpfBox.Text = "";
            bairroBox.Text = "";
            telefoneBox.Text = "";
            cpfBox1.Text = "";
            codigoBox.Text = "";
            MradioButton.Checked = false;
            FradioButton.Checked = false;
        }

        private void Tela1_Load(object sender, EventArgs e)
        {
            DesablitaCampos();
        }

        private void novobtn_Click(object sender, EventArgs e)
        {
            HablitaCampos();
            LimpaCampos();
        }

        private void limparbtn_Click(object sender, EventArgs e)
        {
            LimpaCampos();
        }

        private void adicionarbtn_Click(object sender, EventArgs e)
        {
            string sex;

            if (MradioButton.Checked)
            {
                sex = "Masculino";

            }
            else
            {
                sex = "Feminino";
            }
            try
            {
                connection.Open();
                string strSQL = "Select cpf from users where cpf = " + cpfBox.Text;
                command.Connection = connection;
                command.CommandText = strSQL;
                dt = command.ExecuteReader();
                if (dt.HasRows)
                {
                    MessageBox.Show("CPF já Cadastrado", "Ops", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cpfBox.Text = "";
                    connection.Close();
                }
                else
                {
                    if (!dt.IsClosed) { dt.Close(); }
                    strSQL = "insert into users(nome,endereco,email,bairro,telefone,cpf,sexo)values(@nome,@endereco,@email,@bairro,@telefone,@cpf,@sexo)";
                    command.Parameters.Add("@nome", MySqlDbType.VarChar).Value = nomeBox.Text;
                    command.Parameters.Add("@endereco", MySqlDbType.VarChar).Value = enderecoBox.Text;
                    command.Parameters.Add("@email", MySqlDbType.VarChar).Value = emailBox.Text;
                    command.Parameters.Add("@bairro", MySqlDbType.VarChar).Value = bairroBox.Text;
                    command.Parameters.Add("@telefone", MySqlDbType.VarChar).Value = telefoneBox.Text;
                    command.Parameters.Add("@cpf", MySqlDbType.VarChar).Value = cpfBox.Text;
                    command.Parameters.Add("@sexo", MySqlDbType.VarChar).Value = sex;
                    command.Connection = connection;
                    command.CommandText = strSQL;
                    command.ExecuteNonQuery();
                    MessageBox.Show("Dados Cadastrado Com Sucesso", "Dados Cadastrados", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    LimpaCampos();
                    DesablitaCampos();

                    command.Parameters.Clear();
                    connection.Close();

                }
            }
            catch (Exception erro)
            {
                MessageBox.Show(erro.Message);
                connection.Close();
            }
        }

        private void excluirbtn_Click(object sender, EventArgs e)
        {
            try
            {
                connection.Open();

                string strSQL = "delete from users where id = " + codigoBox.Text;

                command.Connection = connection;

                command.CommandText = strSQL;

                command.ExecuteNonQuery();

                MessageBox.Show("Registro Excluido Com Sucesso", "Exclusão de Registro", MessageBoxButtons.OK, MessageBoxIcon.Information);

                LimpaCampos();

                connection.Close();

                DesablitaCampos();

            }
            catch (Exception erro)
            {
                MessageBox.Show(erro.Message);
                connection.Close();
            }
        }

        private void alterarbtn_Click(object sender, EventArgs e)
        {

        }
        private void sairbtn_Click(object sender, EventArgs e)
        {
            connection.Close();
            Application.Exit();
        }

        private void pesquisarbtn_Click(object sender, EventArgs e)
        {
            try
            {
                connection.Open();
                string strSQL = "Select * from users where cpf = " + cpfBox1.Text;
                command.Connection = connection;
                command.CommandText = strSQL;
                dt = command.ExecuteReader();
                if (!dt.HasRows)
                {
                    MessageBox.Show("CPF não Cadastrado", "Erro no CPF", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cpfBox1.Text = "";
                    connection.Close();
                }
                else
                {
                    dt.Read();
                    codigoBox.Text = dt["id"].ToString();
                    nomeBox.Text = dt["nome"].ToString();
                    enderecoBox.Text = dt["endereco"].ToString();
                    emailBox.Text = dt["email"].ToString();
                    bairroBox.Text = dt["bairro"].ToString();
                    telefoneBox.Text = dt["telefone"].ToString();
                    cpfBox.Text = dt["cpf"].ToString();
                    string sex = dt["sexo"].ToString();
                    if (sex == "Masculino")
                    {
                        MradioButton.Checked = true;
                    }
                    else
                    {
                        FradioButton.Checked = true;
                    }

                    HablitaCampos();
                    if (!dt.IsClosed) { dt.Close(); }
                    connection.Close();

                }
            }
            catch (Exception erro)
            {
                MessageBox.Show(erro.Message);
                connection.Close();
            }
        }
    }
}
