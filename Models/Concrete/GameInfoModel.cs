using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFGAME.ViewModels;

namespace WPFGAME.Models.Concrete
{
    public class GameInfoModel:ViewModelBase
    {
        private int _row;
        public int Row
        {
            get => _row;
            set
            {
                _row = value;
                OnPropertyChanged(nameof(Row));
            }
        }

        private int _column;
        public int Column
        {
            get => _column;
            set
            {
                _column = value;
                OnPropertyChanged(nameof(Column));
            }
        }
        private string _userName;
        public string Username { get { return _userName; } set { _userName = value; OnPropertyChanged(nameof(Username)); } }

        private int _characterLevel;
        public int CharacterLevel { get { return _characterLevel; } set { _characterLevel = value; OnPropertyChanged(nameof(CharacterLevel)); } }

        private int _characterMoney;
        public int CharacterMoney { get { return _characterMoney; } set { _characterMoney = value; OnPropertyChanged(nameof(CharacterMoney)); } }
    }
}
