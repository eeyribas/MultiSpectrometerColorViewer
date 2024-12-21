using MultiSpectrometerColorViewer.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MultiSpectrometerColorViewer
{
    public partial class Form5 : Form
    {
        public Form5()
        {
            InitializeComponent();

            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void Form5_Load(object sender, EventArgs e)
        {
            string connectionString = "DataSource=" + Parameters.nameDatabase + "; Version = 3;";
            string requestCode = "select * from " + Parameters.nameTable;

            Parameters.command = new SQLiteCommand();
            Parameters.connection = new SQLiteConnection(connectionString);
            Parameters.connection.Open();
            if (Parameters.connection.State == ConnectionState.Open)
            {
                Parameters.command.Connection = Parameters.connection;
                Parameters.command.CommandText = requestCode;
                Parameters.command.ExecuteNonQuery();
                SQLiteDataReader reader = Parameters.command.ExecuteReader();
                while (reader.Read())
                {
                    dataGridView1.Rows.Add(reader["id"].ToString(), reader["date"].ToString(), reader["deltaE1"].ToString(),
                                           reader["deltaE2"].ToString(), reader["deltaE3"].ToString(), reader["kf12"].ToString(),
                                           reader["kf13"].ToString(), reader["kf23"].ToString());
                }
                Parameters.connection.Close();
            }
            else
            {
                MessageBox.Show("Failed to list data...", "Information");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string connectionString = "DataSource=" + Parameters.nameDatabase + "; Version = 3;";
            string requestCode = "delete from " + Parameters.nameTable;

            Parameters.command = new SQLiteCommand();
            Parameters.connection = new SQLiteConnection(connectionString);
            Parameters.connection.Open();
            if (Parameters.connection.State == ConnectionState.Open)
            {
                Parameters.command.Connection = Parameters.connection;
                Parameters.command.CommandText = requestCode;
                Parameters.command.ExecuteNonQuery();
                Parameters.connection.Close();
                dataGridView1.Rows.Clear();
                Parameters.id = 1;
            }
            else
            {
                MessageBox.Show("Failed to delete table...", "Information");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
