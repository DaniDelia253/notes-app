using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace API.Data
{
    public class DatabaseConnector
    {
        public MySqlConnection connection;
        public DatabaseConnector(string connectionString)
        {
            connection = new MySqlConnection(connectionString);
        }
        public MySqlDataReader CreateDatabaseReader(string query)
        {
            connection.Open();

            MySqlCommand cmd = new MySqlCommand
            {
                CommandText = query,
                Connection = connection,
                CommandType = CommandType.Text
            };

            return cmd.ExecuteReader();
        }
        public void CloseConnection()
        {
            connection.Close();
        }
    }
}