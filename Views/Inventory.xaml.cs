using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPFGAME.Models;
using WPFGAME.ViewModels;

namespace WPFGAME.Views
{
    /// <summary>
    /// Inventory.xaml etkileşim mantığı
    /// </summary>
    public partial class Inventory : UserControl
    {
        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var border = sender as Border;
            if (border.DataContext is InventoryItem selectedItem)
            {
                // Access the ViewModel
                var viewModel = DataContext as InventoryViewModel;

                // Deselect all except the selected item
                viewModel.DeselectAllExcept(selectedItem);

                // Toggle the selection state of the clicked item
                if (selectedItem.Quantity > 0)
                {
                    selectedItem.IsSelected = !selectedItem.IsSelected;
                }
                
            }
        }

        public Inventory()
        {
            //var vm = DataContext as InventoryViewModel;
            //vm.LoadItems();
            InitializeComponent();
            
        }
    }
}
