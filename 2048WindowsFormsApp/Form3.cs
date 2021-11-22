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
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        public bool CheckUser()
        {
            DataBase dataBase = new DataBase();

            DataTable table = new DataTable();

            MySqlDataAdapter adapter = new MySqlDataAdapter();

            MySqlCommand command = new MySqlCommand($"SELECT * FROM `users` WHERE `login`={textBox1.Text}", dataBase.Get());

            adapter.SelectCommand = command;
            adapter.Fill(table);

            if (table.Rows.Count > 0)//в таблице ищем пользователя
            {
                MessageBox.Show("Логин занят");
                return true;
            }
            else
                return false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (CheckUser())
                return;

            DataBase data = new DataBase();//создаем объект
            //INSERT INTO оператор для вставки данных в таблицу БД
            MySqlCommand command = new MySqlCommand($"INSERT INTO `users`(`login`,`pass`,`score`) VALUES({textBox1.Text},{textBox2.Text},0)", data.Get());

            data.Open();
            if (command.ExecuteNonQuery() == 1)//функция, выполняющая SQL запрос
            {
                User.Name = textBox1.Text;
                Hide();
                Form1 form1 = new Form1();
                form1.ShowDialog();
            }
            data.Close();
        }
    }
}

