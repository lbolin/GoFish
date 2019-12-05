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

        public string Name { get; set; }
        public int Books { get; set; }
        public int Time { get; set; }

        public HighScoreViewModel(Models.HighScore hs)
        {
            highScore = hs;
            Name = hs?.Name;
            Books = hs?.Books ?? 0;
            Time = hs?.Time ?? 0;
        }
    }
}
