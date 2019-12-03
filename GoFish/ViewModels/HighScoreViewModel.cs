using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GoFish.Models;

namespace GoFish.ViewModels
{
    class HighScoreViewModel
    {
        private Models.HighScore highScore;

        public string Name { get; }
        public int Books { get; }
        public int Time { get; }

        public HighScoreViewModel(Models.HighScore hs)
        {
            highScore = hs;
        }
    }
}
