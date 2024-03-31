using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WPFGAME.Commands;
using WPFGAME.Models.Concrete;

namespace WPFGAME.ViewModels
{
    public class GameViewModel : ViewModelBase
    {

        private MainViewModel _mainViewModel;
        private readonly GameModel _gameModel;
        private ICommand _openFarmingTabCommand;

        public ICommand OpenFarmingTabCommand
        {
            get => _openFarmingTabCommand ?? (_openFarmingTabCommand = new RelayCommand(OpenFarmingTab));
            set
            {
                _openFarmingTabCommand = value;
                OnPropertyChanged(nameof(OpenFarmingTabCommand));
            }
        }
        private ICommand _openForestTabCommand;

        public ICommand OpenForestTabCommand
        {
            get => _openForestTabCommand ?? (_openForestTabCommand = new RelayCommand(OpenForestTab));
            set
            {
                _openForestTabCommand = value;
                OnPropertyChanged(nameof(OpenForestTabCommand));
            }
        }
        public NavigateViewModel NavigateViewModel { get; set; }
        public GameViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
            NavigateViewModel = new NavigateViewModel();
            _gameModel = new GameModel();
            OpenFarmingTabCommand = new RelayCommand(OpenFarmingTab);
        }

        public GameInfoModel GameInfo => IoC.GameInfoDal.GameInfoModel;

        private void OpenFarmingTab()
        {
            _mainViewModel.SelectedViewModel = new FarmingViewModel();

        }
        private void OpenForestTab()
        {
            _mainViewModel.SelectedViewModel = new ForestViewModel();

        }

        // ViewModel property that calls OnPropertyChanged
        public int Score
        {
            get { return _gameModel.Score; }
            set
            {
                if (_gameModel.Score != value)
                {
                    _gameModel.Score = value;
                    OnPropertyChanged(nameof(Score));
                }
            }
        }
    }
}
