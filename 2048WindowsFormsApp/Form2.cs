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

namespace _2048WindowsFormsApp
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string userLogin = textBox1.Text;
            string userPass = textBox2.Text;

            DataBase dataBase = new DataBase();  

            MySqlCommand command = new MySqlCommand($"SELECT * FROM `users` WHERE `login`={userLogin} AND `pass`={userPass}", dataBase.Get());//указали к какой БД подключаемся

            DataTable table = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();

            adapter.SelectCommand = command;//выполняем команду command
            adapter.Fill(table);//заполняем объект table теми данными, которые получили

            if (table.Rows.Count > 0)//считаем кол-во рядов
            {
                User.Name = userLogin;
                MessageBox.Show("Вы вошли");
                Hide();
                Form1 form1 = new Form1();
                form1.ShowDialog();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
