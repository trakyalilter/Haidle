using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFGAME.Dal;
using WPFGAME.ViewModels;

namespace WPFGAME
{
    public class IoC
    {
        public static ForestDal ForestDal;
        public static GameInfoDal GameInfoDal;
        public static void Initilize()
        {
            ForestDal = new ForestDal();
            GameInfoDal = new GameInfoDal();
            GameInfoDal.InitializeDatabase();
            ForestDal.InitializeDatabase();
        }
        public void UpdateAll()
        {
            ForestDal.UpdateForestStatsAsync(ForestDal.ForestStatsModel).Wait();
        }
    }
}
