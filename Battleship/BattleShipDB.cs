using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
namespace Battleship
{
    class BattleShipDB
    {
        private static SQLiteConnection sqlConnection = new SQLiteConnection("Data Source=BattleShipDB.db; Version=3");
        public static void IncCountShots()
        {
            sqlConnection.Open();
            var command = new SQLiteCommand("UPDATE MyStatistic SET CountShots = CountShots + 1", sqlConnection);
            command.ExecuteNonQuery();
            sqlConnection.Close();
        }
        public static void IncCountHits()
        {
            sqlConnection.Open();
            var command = new SQLiteCommand("UPDATE MyStatistic SET CountHits = CountHits + 1", sqlConnection);
            command.ExecuteNonQuery();
            sqlConnection.Close();
        }
        public static void IncCountGame()
        {
            sqlConnection.Open();
            var command = new SQLiteCommand("UPDATE MyStatistic SET CountGame = CountGame + 1", sqlConnection);
            command.ExecuteNonQuery();
            sqlConnection.Close();
        }
        public static void IncCountWins()
        {
            sqlConnection.Open();
            var command = new SQLiteCommand("UPDATE MyStatistic SET CountWins = CountWins + 1", sqlConnection);
            command.ExecuteNonQuery();
            sqlConnection.Close();
        }
        public static void IncCountKill()
        {
            sqlConnection.Open();
            var command = new SQLiteCommand("UPDATE MyStatistic SET CountKill = CountKill + 1", sqlConnection);
            command.ExecuteNonQuery();
            sqlConnection.Close();
        }
        public static string SelectCountShots()
        {
            
            sqlConnection.Open();
            var command = new SQLiteCommand("SELECT CountShots FROM MyStatistic", sqlConnection);
            string value=command.ExecuteScalar().ToString();
            sqlConnection.Close();
            return value;
        }
        public static string SelectCountHits()
        {
            sqlConnection.Open();
            var command = new SQLiteCommand("SELECT CountHits FROM MyStatistic", sqlConnection);
            string value = command.ExecuteScalar().ToString();
            sqlConnection.Close();
            return value;
        }
        public static string SelectCountGame()
        {
            sqlConnection.Open();
            var command = new SQLiteCommand("SELECT CountGame FROM MyStatistic", sqlConnection);
            string value = command.ExecuteScalar().ToString();
            sqlConnection.Close();
            return value;
        }
        public static string SelectCountWins()
        {
            sqlConnection.Open();
            var command = new SQLiteCommand("SELECT CountWins FROM MyStatistic", sqlConnection);
            string value = command.ExecuteScalar().ToString();
            sqlConnection.Close();
            return value;
        }
        public static string SelectCountKill()
        {
            sqlConnection.Open();
            var command = new SQLiteCommand("SELECT CountKill FROM MyStatistic", sqlConnection);
            string value = command.ExecuteScalar().ToString();
            sqlConnection.Close();
            return value;
        }

    }
}
