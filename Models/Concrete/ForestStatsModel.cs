using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFGAME.ViewModels;

namespace WPFGAME.Models
{
    public class ForestStatsModel:ViewModelBase
    {
        public int SkillLevel;
        private int _experience;
        public int Experience
        {
            get
            {
                return _experience;
            }
            set
            {
                _experience = value;
                OnPropertyChanged(nameof(Experience));
                OnPropertyChanged("ExperienceText");
            }
        }
        public int MaxExperience;
    }
}
