using GoFish.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoFish.ViewModels
{
    class GameStateViewModel
    {
        private GameState gameState;

        public ObservableCollection<HighScoreViewModel> HighScores;
        public bool UsingLightTheme { get; set; }
        public bool UsingWoodlandDeck { get; set; }        
        public int AIDifficulty { get; set; }

        public GameStateViewModel()
        {
            gameState = new GameState();

            UsingLightTheme = true;
            UsingWoodlandDeck = true;
            AIDifficulty = 0;

            HighScores = new ObservableCollection<HighScoreViewModel>();
            foreach (var hs in gameState.HighScores)
            {
                HighScores.Add(new HighScoreViewModel(hs));
            }
        }
    }
}
