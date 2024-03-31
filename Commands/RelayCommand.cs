﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WPFGAME.Commands
{
    public class RelayCommand : ICommand
    {

        private readonly Action _execute;
        private readonly Func<bool> _canExecute;
        private Action<object> openFarmingTab;

        public RelayCommand(Action execute)
            : this(execute, null)
        {
        }

        public RelayCommand(Action execute, Func<bool> canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");

            _execute = execute;
            _canExecute = canExecute;
        }

        public RelayCommand(Action<object> openFarmingTab)
        {
            this.openFarmingTab = openFarmingTab;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null ? true : _canExecute();
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter)
        {
            _execute();
        }
    }
}
