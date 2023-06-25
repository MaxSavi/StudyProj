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
using System.Web.Http;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace уп
{
    public partial class Form1 : Form
    {
        
        public Form1()
        {
            InitializeComponent();
            string db_INFO = System.Configuration.ConfigurationManager.AppSettings["Setting1"];
            NpgsqlConnection conn = new NpgsqlConnection(db_INFO);
            conn.Open();
            NpgsqlCommand comm = new NpgsqlCommand();
            comm.Connection = conn;
            comm.CommandType = CommandType.Text;
            comm.CommandText = "select * from Logs";
            NpgsqlDataReader dr = comm.ExecuteReader();
            if (dr.HasRows)
            {
                DataTable dt = new DataTable();
                dt.Load(dr);
                dataGridView2.DataSource = dt;
            }
            conn.Dispose();
            conn.Close();
        }
        
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            string filePath = System.Configuration.ConfigurationManager.AppSettings["Setting2"];
            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    MatchCollection ip = Regex.Matches(line, @"\b(?:\d{1,3}\.){3}\d{1,3}\b");
                    string ip_logs = ip[0].Value;
                    MatchCollection datetime = Regex.Matches(line, @"(\d{2}/\w{3}/\d{4}:\d{2}:\d{2}:\d{2})");
                    string datetime_logs = datetime[0].Value;
                    MatchCollection request = Regex.Matches(line, @"\""([^\""]*)\""");
                    string request_logs = request[0].Value;
                    MatchCollection http_status = Regex.Matches(line, @"\"" \d{3}");
                    string http_status_logs = http_status[0].Value;
                    http_status_logs = http_status_logs.Replace("\" ", "");


                    string connectionString = System.Configuration.ConfigurationManager.AppSettings["Setting1"];

                    using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                    {
                        connection.Open();

                        string tableName = "Logs";
                        string columnName1 = "IP_Logs";
                        string columnName2 = "datetime_Logs";
                        string columnName3 = "Request_Logs";
                        string columnName4 = "http_status_Logs";

                        string insertQuery = $"INSERT INTO {tableName} ({columnName1}, {columnName2}, {columnName3}, {columnName4}) VALUES (@value1, @value2, @value3, @value4)";

                        using (NpgsqlCommand command = new NpgsqlCommand(insertQuery, connection))
                        {
                            command.Parameters.AddWithValue("@value1", ip_logs);
                            command.Parameters.AddWithValue("@value2", datetime_logs);
                            command.Parameters.AddWithValue("@value3", request_logs);
                            command.Parameters.AddWithValue("@value4", http_status_logs);

                            int rowsAffected = command.ExecuteNonQuery();
                            Console.WriteLine($"Добавлено {rowsAffected} строк.");
                        }
                    }
                }
            }
            Form1 frm1 = new Form1();
            this.Hide();
            frm1.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form2 frm2 = new Form2();
            frm2.Show();
            this.Hide();
        }

        private void Form1_Load(object sender, EventArgs e)
        {


        }

        private void button5_Click(object sender, EventArgs e)
        {
            string db_INFO = System.Configuration.ConfigurationManager.AppSettings["Setting1"];
            NpgsqlConnection conn = new NpgsqlConnection(db_INFO);
            conn.Open();
            NpgsqlCommand comm = new NpgsqlCommand();
            comm.Connection = conn;
            comm.CommandType = CommandType.Text;
            comm.CommandText = "select IP_Logs, count(*) from Logs group by IP_Logs; ";
            NpgsqlDataReader dr = comm.ExecuteReader();
            if (dr.HasRows)
            {
                DataTable dt = new DataTable();
                dt.Load(dr);
                dataGridView2.DataSource = dt;
            }
            conn.Dispose();
            conn.Close();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            string db_INFO = System.Configuration.ConfigurationManager.AppSettings["Setting1"];
            NpgsqlConnection conn = new NpgsqlConnection(db_INFO);
            conn.Open();
            NpgsqlCommand comm = new NpgsqlCommand();
            comm.Connection = conn;
            comm.CommandType = CommandType.Text;
            comm.CommandText = "select datetime_Logs, count(*) from Logs group by datetime_Logs; ";
            NpgsqlDataReader dr = comm.ExecuteReader();
            if (dr.HasRows)
            {
                DataTable dt = new DataTable();
                dt.Load(dr);
                dataGridView2.DataSource = dt;
            }
            conn.Dispose();
            conn.Close();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            string db_INFO = System.Configuration.ConfigurationManager.AppSettings["Setting1"];
            NpgsqlConnection conn = new NpgsqlConnection(db_INFO);
            conn.Open();
            NpgsqlCommand comm = new NpgsqlCommand();
            comm.Connection = conn;
            comm.CommandType = CommandType.Text;
            comm.CommandText = "select * from Logs;";
            NpgsqlDataReader dr = comm.ExecuteReader();
            if (dr.HasRows)
            {
                DataTable dt = new DataTable();
                dt.Load(dr);
                dataGridView2.DataSource = dt;
            }
            conn.Dispose();
            conn.Close();
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {
            string connectionString = System.Configuration.ConfigurationManager.AppSettings["Setting1"];
            NpgsqlConnection connection = new NpgsqlConnection(connectionString);

            try
            {
                connection.Open();

                string sql = "SELECT * FROM Logs";
                NpgsqlCommand command = new NpgsqlCommand(sql, connection);
                NpgsqlDataReader reader = command.ExecuteReader();

                JsonSerializer serializer = new JsonSerializer();

                using (StreamWriter file = File.CreateText("output.json"))
                {
                    while (reader.Read())
                    {
                        var data = new
                        {
                            Id = reader.GetInt32(0),
                            IP = reader.GetString(1),
                            DataTime = reader.GetString(2),
                            Request = reader.GetString(3),
                            http_status = reader.GetString(4),
                        };

                        serializer.Serialize(file, data);
                        file.WriteLine();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Произошла ошибка: " + ex.Message);
            }
            finally
            {
                connection.Close();
                connection.Dispose();
            }
        }
    }
}
