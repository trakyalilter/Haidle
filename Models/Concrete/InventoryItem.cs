using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WPFGAME.ViewModels;

namespace WPFGAME.Models
{
    public class InventoryItem:ViewModelBase
    {
        private bool _isSelected;
        private Guid _id;
        public InventoryItem()
        {
            _id = Guid.NewGuid(); // Automatically generate a new unique ID for each item
        }
        public Guid Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged(nameof(Id));
            }
        }
        public string Name { get; set; }
        public string ImageSource { get; set; }
        
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                OnPropertyChanged(nameof(IsSelected));
            }
        }
        private int _quantity;
        public int Quantity
        {
            get => _quantity;
            set
            {
                _quantity = value;
                OnPropertyChanged(nameof(Quantity));
            }
        }
        public Visibility IsQuantityBiggerThanZero => Quantity == 0 ? Visibility.Collapsed : Visibility.Visible;

    }
}
