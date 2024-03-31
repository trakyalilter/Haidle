using System.Collections.ObjectModel;
using System.Windows.Input;
using WPFGAME.Commands;
using WPFGAME.Models;
using WPFGAME.Models.Concrete;
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
            new ShopItem { Name = "Extra Slot", Price = 100,ShopItemEnum=ShopItemEnum.ExtraSlot },
            new ShopItem { Name = "Shield", Price = 150 },
            // Add more items...
        };

            BuyCommand = new RelayCommand2<ShopItem>(BuyItem);
        }

        private void BuyItem(ShopItem item)
        {
            switch (item.ShopItemEnum)
            {
                case ShopItemEnum.ExtraSlot:
                    IoC.GameInfoDal.GameInfoModel.Row++;
                    break;
                default:
                    break;
            }
        }
    }

}