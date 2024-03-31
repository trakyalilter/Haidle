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
using System.Windows.Threading;
using WPFGAME.ViewModels;

namespace WPFGAME.Views
{
    /// <summary>
    /// Forest.xaml etkileşim mantığı
    /// </summary>
    public partial class Forest : UserControl
    {
        public Forest()
        {
            InitializeComponent();
            this.DataContext = new ForestViewModel();
            if (DataContext is ForestViewModel viewModel)
            {
                viewModel.ShowTreeAddedPopup += ViewModel_ShowTreeAddedPopup;
            }
            void ViewModel_ShowTreeAddedPopup(object sender,TreeType treeType)
            {
                // Show the popup
                var a =sender as ForestViewModel;
                
                CompletionPopup.IsOpen = true;
                txtPop.Text = $"+{a.QuantityIncrement} {treeType.ToString()} Wood";
                // Set a timer to close the popup after 2 seconds
                var closeTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(2) };
                closeTimer.Tick += (s, args) =>
                {
                    CompletionPopup.IsOpen = false;
                    closeTimer.Stop();
                };
                closeTimer.Start();
            }
        }
    }
}
