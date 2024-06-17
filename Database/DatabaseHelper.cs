using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManagementApp.Classes
{
    public static class DatabaseHelper
    {
        private const string ConnectionString = "Data Source=Test.db";

        public static void InitializeDatabase()
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                string tableQuery = @"CREATE TABLE IF NOT EXISTS Users (
                                  Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                  Name TEXT NOT NULL,
                                  Surname TEXT NOT NULL,
                                  Address TEXT NOT NULL,
                                  PhoneNumber TEXT NOT NULL,
                                  Email TEXT NOT NULL                
                                );";
                using (var command = new SQLiteCommand(tableQuery, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        public static SQLiteConnection GetConnection()
        {
            return new SQLiteConnection(ConnectionString);
        }
    }
}
