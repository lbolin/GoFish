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
        public bool UsingLightTheme { get; }
        public bool UsingWoodlandDeck { get; }

        public GameStateViewModel()
        {
            gameState = new GameState();

            UsingLightTheme = true;
            UsingWoodlandDeck = true;                
            
            foreach (var hs in gameState.HighScores)
            {
                HighScores.Add(new HighScoreViewModel(hs));
            }
        }
    }
}
