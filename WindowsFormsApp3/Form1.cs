using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp3
{
    public partial class Form1 : Form
    {
        SqlConnection sqlConnection;
        SqlDataReader sqlReader = null;
        SqlCommand command = null;

        public Form1()
        {
            InitializeComponent();
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            String connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=E:\VS\шаблоны Visual Studio\WindowsFormsApp3\WindowsFormsApp3\Database1.mdf;Integrated Security=TrueData Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=E:\VS\шаблоны Visual Studio\WindowsFormsApp3\WindowsFormsApp3\Database1.mdf;Integrated Security=True";
            sqlConnection = new SqlConnection(connectionString);

            await sqlConnection.OpenAsync();

            SqlCommand command = new SqlCommand("SELECT * FROM Tovar",sqlConnection);

            try
            {
                sqlReader = await command.ExecuteReaderAsync();

                while(await sqlReader.ReadAsync())
                {
                    listBox1.Items.Add(Convert.ToString(sqlReader["Id"])+ "   " + Convert.ToString(sqlReader["Название"]) + "   " + Convert.ToString(sqlReader["Стоимость"]));
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(),ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (sqlReader != null)
                {
                    sqlReader.Close();
                }
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (sqlConnection != null && sqlConnection.State != ConnectionState.Closed)
            {
                sqlConnection.Close();
            }
        }



      //////////////////  //добавить 
        private async void button1_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBox1.Text) && !string.IsNullOrWhiteSpace(textBox1.Text) &&
                !string.IsNullOrEmpty(textBox2.Text) && !string.IsNullOrWhiteSpace(textBox2.Text))
            {
                String sqlString = Convert.ToString("INSERT INTO Tovar([Название],[Стоимость]) VALUES (@name,@price)");
                command = new SqlCommand(sqlString, sqlConnection);
                command.Parameters.AddWithValue("name", textBox1.Text);
                command.Parameters.AddWithValue("price", textBox2.Text);
                await command.ExecuteNonQueryAsync();
                refreshSql();
            }
            else
            {
                label3.Visible = true;
                label3.Text = "Заполните все поля!";
            }
            textBox1.Text = "";
            textBox2.Text = "";
        }



        ///////////////   //обновить
        private void button2_Click(object sender, EventArgs e)
        {
            refreshSql();
        }

        private async void refreshSql()
        {
            listBox1.Items.Clear();
            command = new SqlCommand("SELECT * FROM Tovar", sqlConnection);

            try
            {
                sqlReader = await command.ExecuteReaderAsync();

                while (await sqlReader.ReadAsync())
                {
                    listBox1.Items.Add(Convert.ToString(sqlReader["Id"]) + "   " + Convert.ToString(sqlReader["Название"]) + "   " + Convert.ToString(sqlReader["Стоимость"]));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (sqlReader != null)
                {
                    sqlReader.Close();
                }
            }
        }
    }
}

