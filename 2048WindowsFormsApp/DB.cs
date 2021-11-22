using MySql.Data.MySqlClient;

namespace _2048WindowsFormsApp
{
    class DataBase
    {
        //Подключение к удаленной БД
        MySqlConnection connection = new MySqlConnection("server=localhost;port=3306;username=root;password=root;database=user_info;");

        public void Open()//открывает соединение с БД
        {
            if (connection.State == System.Data.ConnectionState.Closed)
                connection.Open();
        }

        public void Close()//закрывает соединение с БД
        {
            if (connection.State == System.Data.ConnectionState.Open)
                connection.Close();
        }

        public MySqlConnection Get()//возвращает соединение с БД
        {
            return connection;
        }
    }
}
