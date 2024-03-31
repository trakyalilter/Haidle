using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Threading;
using WPFGAME.Models;
using WPFGAME.Models.Concrete;

namespace WPFGAME.ViewModels
{
    public class InventoryViewModel:ViewModelBase
    {
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
        private DispatcherTimer _timer;
        public ObservableCollection<InventoryItem> Items { get; set; } = new ObservableCollection<InventoryItem>();

        private InventoryItem _selectedItem;

        public InventoryItem SelectedItem
        {
            get => _selectedItem;
            set
            {
                if (_selectedItem != value)
                {
                    if (_selectedItem != null) _selectedItem.IsSelected = false;
                    _selectedItem = value;
                    if (_selectedItem != null) _selectedItem.IsSelected = true;
                    OnPropertyChanged(nameof(SelectedItem));
                }
            }
        }
        public InventoryViewModel()
        {
            GameInfoModel = IoC.GameInfoDal.ReadGameInfo(id: 1);
            // Populate the collection with sample items
            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(250)
            };
            _timer.Tick += async (sender, e) => await LoadItems();
            _timer.Start();

        }
        public void DeselectAllExcept(InventoryItem selectedItem)
        {
            foreach (var item in Items)
            {
                if (item != selectedItem)
                {
                    item.IsSelected = false;
                }
            }
        }

        public async Task LoadItems()
        {
            var selectedItemId = Items.FirstOrDefault(item => item.IsSelected)?.Id; 
            var treeQuantities = await IoC.ForestDal.LoadTreeQuantitiesAsync();
            // Populate your Items collection here
            // Example:
            Items.Clear();
            foreach (var item in treeQuantities.Where(item => item.Quantity > 0))
            {
                Items.Add(item);
            }
            var itemToSelect = Items.FirstOrDefault(item => item.Id == selectedItemId);
            if (itemToSelect != null)
            {
                itemToSelect.IsSelected = true;
            }
            // Calculate the number of empty slots needed to make up 12 slots in total
            GameInfoModel = IoC.GameInfoDal.ReadGameInfo(id: 1);
            int emptySlotsNeeded = GameInfoModel.Row*GameInfoModel.Column - Items.Count;
            //OnPropertyChanged(nameof(GameInfoModel));
            // Fill the remaining slots with empty InventoryItem objects if needed
            for (int i = 0; i < emptySlotsNeeded; i++)
            {
                Items.Add(new InventoryItem {Quantity=0}); // Assuming you have an "empty.png" for empty slots
            }
        }
    }
}
