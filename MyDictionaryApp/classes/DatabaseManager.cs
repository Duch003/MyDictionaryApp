using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Npgsql;

namespace MyDictionaryApp.classes
{
    public class DatabaseManager
    {
        private static NpgsqlConnection _connect;
        private static string _connString;
        private static bool _connected;

        public DatabaseManager(string server,string user, string password, string port, string dbName)
        {
            _connected = false;
            if (server == null || user == null || password == null || port == null || dbName == null)
                return;
            var connString = $"Server={server};Port={port};User id={user};Password={password};Database={dbName}";
            _connString = connString;
        }

        public static void Connect()
        {
            if(_connString == null)
                throw new Exception("Cannot connect to database.");
            _connect = new NpgsqlConnection(_connString);
            _connect.Open();
            _connected = true;
        }

        public static DataSet GetAllWords(int userid = 0)
        {
            if (!_connected)
                throw new Exception("Connection with server is closed.");
            var query = userid == 0 ? "SELECT * FROM dictionary" : $"SELECT * FROM dictionary WHERE userid = {userid}";
            var dataAdapter = new NpgsqlDataAdapter(query, _connect);
            var dataSet = new DataSet();
            dataSet.Reset();
            dataAdapter.Fill(dataSet);
            return dataSet;
        }

        public static DataSet Query(string select, string from, string condition)
        {
            if (!_connected)
                throw new Exception("Connection with server is closed.");
            var query =  $"SELECT {select} FROM {from} {condition}";
            var dataAdapter = new NpgsqlDataAdapter(query, _connect);
            var dataSet = new DataSet();
            dataSet.Reset();
            dataAdapter.Fill(dataSet);
            return dataSet;
        }

        public static void Update(string table, string set, string condition)
        {
            if (!_connected)
                throw new Exception("Connection with server is closed.");
            var query = $"UPDATE {table} SET {set} {condition}";
            var command = new NpgsqlCommand(query, _connect);
            command.ExecuteNonQuery();
        }

        public static void Delete(string table, string condition)
        {
            if (!_connected)
                throw new Exception("Connection with server is closed.");
            var query = $"DELETE FROM {table} {condition}";
            var command = new NpgsqlCommand(query, _connect);
            command.ExecuteNonQuery();
        }

        public static void Insert(string table, string values)
        {
            if (!_connected)
                throw new Exception("Connection with server is closed.");
            var query = $"INSERT INTO {table} VALUES ({values})";
            var command = new NpgsqlCommand(query, _connect);
            command.ExecuteNonQuery();
        }

        public static void Disconnect()
        {
            if (!_connected)
                throw new Exception("Connection with server is closed.");
            _connect.Close();
            _connected = false;
        }
    }


}
