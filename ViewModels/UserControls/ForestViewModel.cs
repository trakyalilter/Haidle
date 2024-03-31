using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading; // Required for DispatcherTimer
using WPFGAME.Models;

namespace WPFGAME.ViewModels
{
    public class ForestViewModel : ViewModelBase
    {
        public event EventHandler<TreeType> ShowTreeAddedPopup;

        private DispatcherTimer _progressTimer;
        private double _progressValue;
        private bool _isNormalTreeSelected;
        private bool _isOakTreeSelected;
        private bool _isWillowTreeSelected;
        private bool _isTeakTreeSelected;
        private CancellationTokenSource _progressCts;
        
        private TreeType _selectedTree = TreeType.None;

        public TreeType SelectedTree
        {
            get => _selectedTree;
            set
            {
                if (_selectedTree != value)
                {
                    _selectedTree = value;
                    OnPropertyChanged(nameof(SelectedTree));

                    // Reset and restart progress when a new tree is selected
                    if (value != TreeType.None)
                    {
                        RestartProgress();
                    }
                    else
                    {
                        _progressCts?.Cancel();
                        ProgressValue = 0;
                    }

                    // Update the toggle button states
                    OnPropertyChanged(nameof(IsNormalTreeSelected));
                    OnPropertyChanged(nameof(IsOakTreeSelected));
                    OnPropertyChanged(nameof(IsWillowTreeSelected));
                    OnPropertyChanged(nameof(IsTeakTreeSelected));
                }
            }
        }
        public bool IsNormalTreeSelected
        {
            get => SelectedTree == TreeType.Normal;
            set => SelectedTree = value ? TreeType.Normal : TreeType.None;
        }

        public bool IsOakTreeSelected
        {
            get => SelectedTree == TreeType.Oak;
            set => SelectedTree = value ? TreeType.Oak : TreeType.None;
        }
        public bool IsWillowTreeSelected
        {
            get => SelectedTree == TreeType.Willow;
            set => SelectedTree = value ? TreeType.Willow : TreeType.None;
        }

        public bool IsTeakTreeSelected
        {
            get => SelectedTree == TreeType.Teak;
            set => SelectedTree = value ? TreeType.Teak : TreeType.None;
        }
        private int _skillLevel = 1; // Starting skill level
        private int _experiencePoints = 0;
        

        public int SkillLevel
        {
            get => _skillLevel;
            set
            {
                _skillLevel = value;
                OnPropertyChanged(nameof(SkillLevel));
                OnPropertyChanged(nameof(SkillLevelText)); // Notify change for the SkillLevelText as well
            }
        }

        public int ExperiencePoints
        {
            get => _experiencePoints;
            set
            {
                _experiencePoints = value;
                OnPropertyChanged(nameof(ExperiencePoints));
                OnPropertyChanged(nameof(ExperienceText)); // Notify change for the ExperienceText as well
            }
        }

        // Property to display the skill level text
        public string SkillLevelText => $"Forest Skill Level: {IoC.ForestDal.ForestStatsModel.SkillLevel}";

        // Property to display the XP text
        private string _expText;

        public string ExperienceText
        {
            get
            {
                return _expText;
            }
            set
            {
                _expText = value;
                OnPropertyChanged(nameof(ExperienceText));
            }
        }
             
        public ForestViewModel()
        {

            ExperienceText = $"XP: {IoC.ForestDal.ForestStatsModel.Experience}/{IoC.ForestDal.ForestStatsModel.MaxExperience}";
            _isNormalTreeSelected = false;
            _isOakTreeSelected = false;
            _isWillowTreeSelected = false;
            _isTeakTreeSelected = false;
           
        }
        public int QuantityIncrement = 1;
        private async Task UpdateTreeQuantityInDatabase(TreeType treeType)
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "game.db");
            string connectionString = $"Data Source={path};Version=3;";

            using (var connection = new SQLiteConnection(connectionString))
            {
                await connection.OpenAsync();

                // First, try to update the existing record
                string updateCommandText = $@"
            UPDATE TreeQuantities 
            SET Quantity = Quantity + {QuantityIncrement} 
            WHERE TreeType = @TreeType";

                using (var updateCommand = new SQLiteCommand(updateCommandText, connection))
                {
                    updateCommand.Parameters.AddWithValue("@TreeType", treeType.ToString());
                    var rowsAffected = await updateCommand.ExecuteNonQueryAsync();

                    // If no rows were affected by the UPDATE, the record doesn't exist, so INSERT a new one
                    if (rowsAffected == 0)
                    {
                        string insertCommandText = @"
                    INSERT INTO TreeQuantities (Id, TreeType, Quantity)
                    VALUES (@Id, @TreeType, 1)";

                        using (var insertCommand = new SQLiteCommand(insertCommandText, connection))
                        {
                            insertCommand.Parameters.AddWithValue("@Id", Guid.NewGuid().ToString());
                            insertCommand.Parameters.AddWithValue("@TreeType", treeType.ToString());
                            await insertCommand.ExecuteNonQueryAsync();
                        }
                    }
                }
            }
        }
        private async Task UpdateForestStats(ForestStatsModel forestStatsModel)
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "game.db");
            string connectionString = $"Data Source={path};Version=3;";

            using (var connection = new SQLiteConnection(connectionString))
            {
                await connection.OpenAsync();

                // First, try to update the existing record
                string updateCommandText = $@"
            UPDATE TreeQuantities 
            SET Quantity = Quantity + {QuantityIncrement} 
            WHERE TreeType = @TreeType";

                using (var updateCommand = new SQLiteCommand(updateCommandText, connection))
                {
                    updateCommand.Parameters.AddWithValue("@TreeType", forestStatsModel.ToString());
                    var rowsAffected = await updateCommand.ExecuteNonQueryAsync();

                    // If no rows were affected by the UPDATE, the record doesn't exist, so INSERT a new one
                    if (rowsAffected == 0)
                    {
                        string insertCommandText = @"
                    INSERT INTO TreeQuantities (Id, TreeType, Quantity)
                    VALUES (@Id, @TreeType, 1)";

                        using (var insertCommand = new SQLiteCommand(insertCommandText, connection))
                        {
                            insertCommand.Parameters.AddWithValue("@Id", Guid.NewGuid().ToString());
                            insertCommand.Parameters.AddWithValue("@TreeType", forestStatsModel.ToString());
                            await insertCommand.ExecuteNonQueryAsync();
                        }
                    }
                }
            }
        }
        private async Task UpdateProgressAsync(CancellationToken token)
        {
            ProgressValue = 0;
            try
            {
                while (!token.IsCancellationRequested)
                {
                    await Task.Delay(10, token);
                    ProgressValue += 0.2;
                    if (ProgressValue >= 100) {
                        ProgressValue = 0;
                        IoC.ForestDal.ForestStatsModel.Experience++;
                        ExperienceText = $"{IoC.ForestDal.ForestStatsModel.Experience}/{IoC.ForestDal.ForestStatsModel.MaxExperience}";
                        if (IoC.ForestDal.ForestStatsModel.Experience >= IoC.ForestDal.ForestStatsModel.MaxExperience)
                        {
                            IoC.ForestDal.ForestStatsModel.SkillLevel++; 
                            IoC.ForestDal.ForestStatsModel.Experience = 0;
                                
                            IoC.ForestDal.ForestStatsModel.MaxExperience = (int)Math.Pow(IoC.ForestDal.ForestStatsModel.SkillLevel, 2) * 4; 
                            if (SkillLevel % 5 == 0)
                            {
                                QuantityIncrement++;
                            }
                           
                        }
                        ShowTreeAddedPopup?.Invoke(this, SelectedTree);
                        await UpdateTreeQuantityInDatabase(SelectedTree); 
                    }

                }
                
            }
            catch (TaskCanceledException)
            {
                // Task was cancelled, reset the progress
                ProgressValue = 0;
            }
        }
        private int _maxExperience = 10; // Initialize with default value

        public int MaxExperience
        {
            get => _maxExperience;
            set
            {
                if (_maxExperience != value)
                {
                    _maxExperience = value;
                    OnPropertyChanged(nameof(MaxExperience));
                    OnPropertyChanged(nameof(ExperienceText)); // Notify change for the ExperienceText
                }
            }
        }
        public double ProgressValue
        {
            get => _progressValue;
            set => SetField(ref _progressValue, value);
        }  
        private void RestartProgress()
        {
            _progressCts?.Cancel(); // Cancel any existing task
            _progressCts = new CancellationTokenSource(); // Create a new CancellationTokenSource
            UpdateProgressAsync(_progressCts.Token).ConfigureAwait(false); // Start the task
        }
        
    }
}
