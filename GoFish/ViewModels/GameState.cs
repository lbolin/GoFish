﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoFish.ViewModels
{
    class GameState
    {
        private List<HighScore> highScores;
        public int Theme { get; }
        public int Deck { get; }
        public int numPlayers { get; }

        public int typeOfGame { get; }

        public GameState(int theme,int deck, int numplayers)
        {
            Theme = theme;
            Deck = deck;
            numPlayers = numplayers;
                

        }
    }
}