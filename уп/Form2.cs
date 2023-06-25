using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;
using System.Text.RegularExpressions;
using System.IO;
using System.Configuration;

namespace уп
{
    public partial class Form2 : Form
    {
        
        public Form2()
        {
            InitializeComponent();
            string db_INFO = System.Configuration.ConfigurationManager.AppSettings["Setting1"];
            NpgsqlConnection conn = new NpgsqlConnection(db_INFO);
            conn.Open();
            NpgsqlCommand comm = new NpgsqlCommand();
            comm.Connection = conn;
            comm.CommandType = CommandType.Text;
            comm.CommandText = "select * from Users";
            NpgsqlDataReader dr = comm.ExecuteReader();
            if (dr.HasRows)
            {
                DataTable dt = new DataTable();
                dt.Load(dr);
                dataGridView1.DataSource = dt;
            }
            conn.Dispose();
            conn.Close();
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            Form1 frm1 = new Form1();
            frm1.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            string connectionString = System.Configuration.ConfigurationManager.AppSettings["Setting1"];

            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                string tableName = "Users";
                string columnName1 = "Name_Users";
                string columnName2 = "Surname_Users";
                string columnName3 = "MiddleName_Users";
                string columnName4 = "Password_Users";


                string insertQuery = $"INSERT INTO {tableName} ({columnName1}, {columnName2}, {columnName3}, {columnName4}) VALUES (@value1, @value2, @value3, @value4)";


                using (NpgsqlCommand command = new NpgsqlCommand(insertQuery, connection))
                {

                    command.Parameters.AddWithValue("@value1", textBox1.Text);
                    command.Parameters.AddWithValue("@value2", textBox3.Text);
                    command.Parameters.AddWithValue("@value3", textBox4.Text);
                    command.Parameters.AddWithValue("@value4", textBox2.Text);

                    int rowsAffected = command.ExecuteNonQuery();
                    MessageBox.Show($"Добавлено {rowsAffected} строк.");
                }
            }
            Form2 frm2 = new Form2();
            this.Hide();
            frm2.Show();
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
