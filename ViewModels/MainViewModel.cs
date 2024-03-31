using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using WPFGAME.Commands;

namespace WPFGAME.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        
        public MainViewModel()
        {
            MinimizeCommand = new RelayCommand(MinimizeWindow);
            CloseCommand = new RelayCommand(CloseWindow);

            SelectedViewModel = new GameViewModel(this);
        }
        private ViewModelBase _selectedViewModel;
        public ViewModelBase SelectedViewModel
        {
            get { return _selectedViewModel; }
            set
            {
                _selectedViewModel = value;
                OnPropertyChanged(nameof(SelectedViewModel));
            }
        }
        private ICommand _minimizeCommand;

        public ICommand MinimizeCommand
        {
            get => _minimizeCommand ?? (_minimizeCommand = new RelayCommand(MinimizeWindow));
            set
            {
                _minimizeCommand = value;
                OnPropertyChanged(nameof(MinimizeCommand));
            }
        }
        public ICommand CloseCommand { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private void MinimizeWindow()
        {
            Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }

        private void CloseWindow()
        {
            IoC.ForestDal.UpdateForestStatsAsync(IoC.ForestDal.ForestStatsModel);
            Application.Current.Shutdown();
        }
    }
}
