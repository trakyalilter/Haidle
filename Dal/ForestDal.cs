using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;
using System.Windows.Shapes;
using WPFGAME.Models;
using WPFGAME.ViewModels;

namespace WPFGAME.Dal
{

    public class ForestDal:ViewModelBase
    {

        private ForestStatsModel _forestStatsModel;
         public ForestStatsModel ForestStatsModel
        {
            get { return _forestStatsModel; }
            set
            {
                _forestStatsModel = value;
                OnPropertyChanged(nameof(ForestStatsModel));
            }
        }
        public ForestDal()
        {
            
            string directory = Directory.GetCurrentDirectory();
            string dbName = "game.db";
            string path = System.IO.Path.Combine(directory, dbName);

            if (!File.Exists(path))
            {
                SQLiteConnection.CreateFile(path); // This creates a new database file if it doesn't exist
            }

            string connectionString = $"Data Source={path};Version=3;";
            using (SQLiteConnection sQLiteConnection = new SQLiteConnection(connectionString))
            {
                sQLiteConnection.Open();
                // Perform database operations
            }
        }
        public async Task UpdateForestStatsAsync(ForestStatsModel stats)
        {
            string path = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "game.db");
            string connectionString = $"Data Source={path};Version=3;";

            using (var connection = new SQLiteConnection(connectionString))
            {
                await connection.OpenAsync();

                // Update the first row found in the table (based on SQLite's internal ROWID)
                string commandText = @"
            UPDATE ForestStats
            SET SkillLevel=@SkillLevel, Experience = @Experience, MaxExperience = @MaxExperience
            WHERE ROWID = (SELECT ROWID FROM ForestStats LIMIT 1)";

                using (var command = new SQLiteCommand(commandText, connection))
                {
                    command.Parameters.AddWithValue("@SkillLevel", stats.SkillLevel);
                    command.Parameters.AddWithValue("@Experience", stats.Experience);
                    command.Parameters.AddWithValue("@MaxExperience", stats.MaxExperience);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }


        public async Task<List<ForestStatsModel>> LoadForestStatsAsync()
        {
            var items = new List<ForestStatsModel>();
            string path = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "game.db");
            string connectionString = $"Data Source={path};Version=3;";

            using (var connection = new SQLiteConnection(connectionString))
            {
                await connection.OpenAsync();
                string query = "SELECT SkillLevel, Experience, MaxExperience FROM ForestStats";

                using (var command = new SQLiteCommand(query, connection))
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var item = new ForestStatsModel
                        {
                            SkillLevel = Convert.ToInt32(reader["SkillLevel"]),
                            Experience = Convert.ToInt32(reader["Experience"]),
                            MaxExperience = Convert.ToInt32(reader["MaxExperience"]),
                        };
                        items.Add(item);
                    }
                }
            }

            return items;
        }

        
        public async Task<List<InventoryItem>> LoadTreeQuantitiesAsync()
        {
            var items = new List<InventoryItem>();
            string path = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "game.db");
            string connectionString = $"Data Source={path};Version=3;";

            using (var connection = new SQLiteConnection(connectionString))
            {
                await connection.OpenAsync();
                string query = "SELECT Id, TreeType, Quantity FROM TreeQuantities";

                using (var command = new SQLiteCommand(query, connection))
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var item = new InventoryItem
                        {
                            Id = Guid.Parse(reader["Id"].ToString()), // Assuming Id property in InventoryItem is of type Guid
                            Name = reader["TreeType"].ToString(),
                            Quantity = Convert.ToInt32(reader["Quantity"]),
                            ImageSource = $"/Images/{reader["TreeType"].ToString().ToLower()}.png"
                        };
                        items.Add(item);
                    }
                }
            }

            return items;
        }
        public void InitializeDatabase()
        {
            string path = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "game.db");
            string connectionString = $"Data Source={path};Version=3;";

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                // Check if the TreeQuantities table exists
                string checkTableQuery = "SELECT name FROM sqlite_master WHERE type='table' AND name='TreeQuantities';";
                using (var command = new SQLiteCommand(checkTableQuery, connection))
                {
                    var result = command.ExecuteScalar();
                    if (result == null || result.ToString() != "TreeQuantities")
                    {
                        // Create the TreeQuantities table
                        string createTableQuery = @"
                        CREATE TABLE TreeQuantities (
                            Id TEXT PRIMARY KEY,
                            TreeType TEXT NOT NULL,
                            Quantity INTEGER DEFAULT 0
                        );";
                        using (var createTableCommand = new SQLiteCommand(createTableQuery, connection))
                        {
                            createTableCommand.ExecuteNonQuery();
                        }
                    }
                }
            }
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                // Check if the TreeQuantities table exists
                string checkTableQuery = "SELECT name FROM sqlite_master WHERE type='table' AND name='ForestStats';";
                using (var command = new SQLiteCommand(checkTableQuery, connection))
                {
                    var result = command.ExecuteScalar();
                    if (result == null || result.ToString() != "ForestStats")
                    {
                        // Create the TreeQuantities table
                        string createTableQuery = @"
                        CREATE TABLE ForestStats (
                            SkillLevel INTEGER DEFAULT 1,
                            Experience INTEGER DEFAULT 0,
                            MaxExperience INTEGER DEFAULT 10
                        );";
                        using (var createTableCommand = new SQLiteCommand(createTableQuery, connection))
                        {
                            createTableCommand.ExecuteNonQuery();
                        }
                    }
                }
            }
            ForestStatsModel = LoadForestStatsAsync().Result[0];
        }
    }
}
