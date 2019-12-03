using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoFish.Models
{
    class GameState
    {
        public List<HighScore> HighScores;
        public bool usingLightTheme;
        public bool usingWoodlandDeck;

        public GameState()
        {
            usingLightTheme = true;
            usingWoodlandDeck = true;
            HighScores = new List<HighScore>();
        }
    }
}
