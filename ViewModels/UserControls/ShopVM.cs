using System.Collections.ObjectModel;
using System.Windows.Input;
using WPFGAME.Commands;
using WPFGAME.Models;
using WPFGAME.ViewModels;
namespace WPFGAME.ViewModels
{
    public class ShopVM : ViewModelBase
    {
        public ObservableCollection<ShopItem> ShopItems { get; set; }
        public ICommand BuyCommand { get; private set; }

        public ShopVM()
        {
            ShopItems = new ObservableCollection<ShopItem>
        {
            new ShopItem { Name = "Extra Slot", Price = 100 },
            new ShopItem { Name = "Shield", Price = 150 },
            // Add more items...
        };

            BuyCommand = new RelayCommand2<ShopItem>(BuyItem);
        }

        private void BuyItem(ShopItem item)
        {
            // Handle the item purchase logic here
            // For example, deduct the price from the user's currency, add the item to the user's inventory, etc.
        }
    }

}