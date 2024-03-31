using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFGAME.Models.Concrete;
using WPFGAME.ViewModels;

namespace WPFGAME.Dal
{
    public class GameInfoDal:ViewModelBase
    {
        private string _connectionString;
        private GameInfoModel _gameInfoModel;
        public GameInfoModel GameInfoModel
        {
            get => _gameInfoModel;
            set
            {
                if (_gameInfoModel != value)
                {
                    _gameInfoModel = value;
                    OnPropertyChanged(nameof(GameInfoModel));
                }
            }
        }
        public GameInfoDal()
        {
            string dbPath = Path.Combine(Directory.GetCurrentDirectory(), "game.db");
            _connectionString = $"Data Source={dbPath};Version=3;";
        }
        public void InitializeDatabase()
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "game.db");
            string connectionString = $"Data Source={path};Version=3;";

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                // Check if the GameInfo table exists
                string checkTableQuery = "SELECT name FROM sqlite_master WHERE type='table' AND name='GameInfo';";
                using (var command = new SQLiteCommand(checkTableQuery, connection))
                {
                    var result = command.ExecuteScalar();
                    if (result == null || result.ToString() != "GameInfo") // Corrected the table name here
                    {
                        // Create the GameInfo table
                        string createTableQuery = @"
                    CREATE TABLE GameInfo (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Row INTEGER NOT NULL,
                    Column INTEGER NOT NULL,
                    Username TEXT NOT NULL,
                    CharacterLevel INTEGER NOT NULL
                );";
                        using (var createTableCommand = new SQLiteCommand(createTableQuery, connection))
                        {
                            createTableCommand.ExecuteNonQuery();
                        }
                        CreateGameInfo(new GameInfoModel { Row = 3, Column = 4, Username = "", CharacterLevel = 1 });
                    }
                }
                    
            }
            GameInfoModel = ReadGameInfo(id: 1);
            OnPropertyChanged(nameof(GameInfoModel));
        }
        public void CreateGameInfo(GameInfoModel gameInfo)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                string sql = "INSERT INTO GameInfo (Row, Column, Username, CharacterLevel) VALUES (@Row, @Column, @Username, @CharacterLevel)";

                using (var command = new SQLiteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Row", gameInfo.Row);
                    command.Parameters.AddWithValue("@Column", gameInfo.Column);
                    command.Parameters.AddWithValue("@Username", gameInfo.Username);
                    command.Parameters.AddWithValue("@CharacterLevel", gameInfo.CharacterLevel);

                    command.ExecuteNonQuery();
                }
            }
        }

        public GameInfoModel ReadGameInfo(int id)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                string sql = "SELECT * FROM GameInfo WHERE Id = @Id";

                using (var command = new SQLiteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new GameInfoModel
                            {
                                Row = Convert.ToInt32(reader["Row"]),
                                Column = Convert.ToInt32(reader["Column"]),
                                Username = reader["Username"].ToString(),
                                CharacterLevel = Convert.ToInt32(reader["CharacterLevel"])
                            };
                        }
                    }
                }
            }

            return null; // or throw an exception
        }

        public void UpdateGameInfo(int id, GameInfoModel gameInfo)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                string sql = "UPDATE GameInfo SET Row = @Row, Column = @Column, Username = @Username, CharacterLevel = @CharacterLevel WHERE Id = @Id";

                using (var command = new SQLiteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    command.Parameters.AddWithValue("@Row", gameInfo.Row);
                    command.Parameters.AddWithValue("@Column", gameInfo.Column);
                    command.Parameters.AddWithValue("@Username", gameInfo.Username);
                    command.Parameters.AddWithValue("@CharacterLevel", gameInfo.CharacterLevel);

                    command.ExecuteNonQuery();
                }
            }
        }

        public void DeleteGameInfo(int id)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                string sql = "DELETE FROM GameInfo WHERE Id = @Id";

                using (var command = new SQLiteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
