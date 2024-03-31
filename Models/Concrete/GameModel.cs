using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFGAME.Models.Concrete
{
    public class GameModel
    {
        public int Score { get; set; }

        // Add methods for game logic, e.g., UpdateScore(), MovePlayer(), etc.
        public void UpdateScore(int points)
        {
            Score += points;
            // Additional logic to handle score updates
        }
    }
}
