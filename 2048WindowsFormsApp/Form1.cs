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
    public partial class Form1 : Form
    {
        private const int mapSize = 4;
        private Label[,] labelsMap;
        private static Random random = new Random();
        private int score = 0;
        DataTable table = new DataTable();
        DataBase dataBase = new DataBase();
        MySqlDataAdapter adapter = new MySqlDataAdapter();
        int indexUser;
        int countList = 0;//количества пользователей
        List<string[]> data = new List<string[]>();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dataGridView1.Enabled = false;

            InitMap();
            GenerateNumber();
            ShowScore();

            MySqlCommand command1 = new MySqlCommand("SELECT `login`, `score` FROM `users` ORDER BY `users`.`score` DESC", dataBase.Get());
            dataBase.Open();
            MySqlDataReader sr = command1.ExecuteReader();


            while (sr.Read())
            {
                data.Add(new string[2]);

                data[data.Count - 1][0] = sr[0].ToString();
                data[data.Count - 1][1] = sr[1].ToString();
                countList++;
            }
            sr.Close();
            dataBase.Close();

            for (int i = 0; i < countList; i++)
            {
                if (data[i][0] == User.Name)
                {
                    indexUser = i;
                }
            }

            foreach (string[] item in data)
            {
                dataGridView1.Rows.Add(item);
            }
        }

        private void ShowScore()
        {
            scoreLabel.Text = score.ToString();
        }

        private void InitMap()
        {
            labelsMap = new Label[mapSize, mapSize];

            for (int i = 0; i < mapSize; i++)
            {
                for (int j = 0; j < mapSize; j++)
                {
                    var newLabel = CreateLabel1(i, j);
                    Controls.Add(newLabel);
                    labelsMap[i, j] = newLabel;
                }
            }
        }

        private void GenerateNumber()
        {
            while (true)
            {
                var randomNumberLable = random.Next(mapSize * mapSize);
                var indexRow = randomNumberLable / mapSize; //i*4+j=number
                var indexColumn = randomNumberLable % mapSize;//j

                if (labelsMap[indexRow, indexColumn].Text == String.Empty)
                {
                    Random rnd = new Random();
                    if (rnd.Next(1, 5) != 4)
                        labelsMap[indexRow, indexColumn].Text = "2";
                    else
                        labelsMap[indexRow, indexColumn].Text = "4";
                    break;
                }
            }
        }

        private Label CreateLabel1(int indexRow, int indexColumn)
        {
            var label = new Label();//вызываем через конструктор
            label.BackColor = System.Drawing.SystemColors.ActiveBorder;
            label.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            label.Size = new System.Drawing.Size(70, 70);

            int x = 10 + indexColumn * 76;
            int y = 70 + indexRow * 76;
            label.Location = new System.Drawing.Point(x, y);
            return label;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode.ToString())
            {
                case "Right":
                    for (int i = 0; i < mapSize; i++)
                    {
                        for (int j = mapSize - 1; j >= 0; j--)
                        {
                            if (labelsMap[i, j].Text != string.Empty)
                            {
                                for (int k = j - 1; k >= 0; k--)//левее от ячейки
                                {
                                    if (labelsMap[i, k].Text != string.Empty)
                                    {
                                        if (labelsMap[i, j].Text == labelsMap[i, k].Text)
                                        {
                                            var number = int.Parse(labelsMap[i, j].Text);
                                            score += number * 2;
                                            labelsMap[i, j].Text = (number * 2).ToString();
                                            labelsMap[i, k].Text = string.Empty;
                                        }
                                        break;//2 4 2 чтобы не схлопывались 2 с 2
                                    }
                                }
                            }
                        }
                    }

                    for (int i = 0; i < mapSize; i++)
                    {
                        for (int j = mapSize - 1; j >= 0; j--)
                        {
                            if (labelsMap[i, j].Text == string.Empty)
                            {
                                for (int k = j - 1; k >= 0; k--)//левее от ячейки
                                {
                                    if (labelsMap[i, k].Text != string.Empty)
                                    {
                                        labelsMap[i, j].Text = labelsMap[i, k].Text;
                                        labelsMap[i, k].Text = string.Empty;
                                        break;//2 4 2 двигаем
                                    }
                                }
                            }
                        }
                    }
                    GenerateNumber();
                    ShowScore();
                    break;

                case "Left":
                    for (int i = 0; i < mapSize; i++)
                    {
                        for (int j = 0; j < mapSize; j++)
                        {
                            if (labelsMap[i, j].Text != string.Empty)
                            {
                                for (int k = j + 1; k < mapSize; k++)
                                {
                                    if (labelsMap[i, k].Text != string.Empty)
                                    {
                                        if (labelsMap[i, j].Text == labelsMap[i, k].Text)
                                        {
                                            var number = int.Parse(labelsMap[i, j].Text);
                                            score += number * 2;
                                            labelsMap[i, j].Text = (number * 2).ToString();
                                            labelsMap[i, k].Text = string.Empty;
                                        }
                                        break;
                                    }
                                }
                            }
                        }
                    }

                    for (int i = 0; i < mapSize; i++)
                    {
                        for (int j = 0; j < mapSize; j++)
                        {
                            if (labelsMap[i, j].Text == string.Empty)
                            {
                                for (int k = j + 1; k < mapSize; k++)
                                {
                                    if (labelsMap[i, k].Text != string.Empty)
                                    {
                                        labelsMap[i, j].Text = labelsMap[i, k].Text;
                                        labelsMap[i, k].Text = string.Empty;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    GenerateNumber();
                    ShowScore();
                    break;

                case "Up":
                    for (int j = 0; j < mapSize; j++)
                    {
                        for (int i = 0; i < mapSize; i++)//строка
                        {
                            if (labelsMap[i, j].Text != string.Empty)
                            {
                                for (int k = i + 1; k < mapSize; k++)//за строки отвечает i. строка
                                {
                                    if (labelsMap[k, j].Text != string.Empty)
                                    {
                                        if (labelsMap[i, j].Text == labelsMap[k, j].Text)
                                        {
                                            var number = int.Parse(labelsMap[i, j].Text);
                                            score += number * 2;
                                            labelsMap[i, j].Text = (number * 2).ToString();
                                            labelsMap[k, j].Text = string.Empty;
                                        }
                                        break;
                                    }
                                }
                            }
                        }
                    }

                    for (int j = 0; j < mapSize; j++)
                    {
                        for (int i = 0; i < mapSize; i++)
                        {
                            if (labelsMap[i, j].Text == string.Empty)
                            {
                                for (int k = i + 1; k < mapSize; k++)
                                {
                                    if (labelsMap[k, j].Text != string.Empty)
                                    {
                                        labelsMap[i, j].Text = labelsMap[k, j].Text;
                                        labelsMap[k, j].Text = string.Empty;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    GenerateNumber();
                    ShowScore();
                    break;

                case "Down":
                    for (int j = 0; j < mapSize; j++)
                    {
                        for (int i = mapSize - 1; i >= 0; i--)
                        {
                            if (labelsMap[i, j].Text != string.Empty)
                            {
                                for (int k = i - 1; k >= 0; k--)
                                {
                                    if (labelsMap[k, j].Text != string.Empty)
                                    {
                                        if (labelsMap[i, j].Text == labelsMap[k, j].Text)
                                        {
                                            var number = int.Parse(labelsMap[i, j].Text);
                                            score += number * 2;
                                            labelsMap[i, j].Text = (number * 2).ToString();
                                            labelsMap[k, j].Text = string.Empty;
                                        }
                                        break;
                                    }
                                }
                            }
                        }
                    }

                    for (int j = 0; j < mapSize; j++)
                    {
                        for (int i = mapSize - 1; i >= 0; i--)
                        {
                            if (labelsMap[i, j].Text == string.Empty)
                            {
                                for (int k = i - 1; k >= 0; k--)
                                {
                                    if (labelsMap[k, j].Text != string.Empty)
                                    {
                                        labelsMap[i, j].Text = labelsMap[k, j].Text;
                                        labelsMap[k, j].Text = string.Empty;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    GenerateNumber();
                    ShowScore();
                    break;

            }
        }

        private void ShowLider()
        {
            if (Convert.ToInt32(data[indexUser][1]) < score)
            {
                MySqlCommand command = new MySqlCommand($"UPDATE `users` SET `score` ={score}  WHERE `users`.`login` ='{User.Name}'", dataBase.Get());
                adapter.SelectCommand = command;
                adapter.Fill(table);
                MySqlCommand command2 = new MySqlCommand($"SELECT `login`, `score` FROM `users` ORDER BY `users`.`score` DESC", dataBase.Get());
                dataBase.Open();
                MySqlDataReader sr1 = command2.ExecuteReader();
                List<string[]> data1 = new List<string[]>();

                while (sr1.Read())
                {
                    data1.Add(new string[2]);

                    data1[data1.Count - 1][0] = sr1[0].ToString();
                    data1[data1.Count - 1][1] = sr1[1].ToString();
                }
                sr1.Close();
                dataBase.Close();
                dataGridView1.Rows.Clear();
                foreach (string[] item in data1)
                {
                    dataGridView1.Rows.Add(item);
                }
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void рестартToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
