using GoFish.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoFish.ViewModels
{
    class GameViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private Game game;

        public ObservableCollection<HandViewModel> Hands { get; set; }
        public ObservableCollection<int> Scores { get; set; }

        public GameViewModel()
        {
            game = new Game();

            Hands = new ObservableCollection<HandViewModel>();
            Scores = new ObservableCollection<int>();

            foreach (var hand in game.Hands)
            {
                var newHand = new HandViewModel(hand);
                newHand.PropertyChanged += OnPropertyChanged;
                Hands.Add(newHand);
            }

            foreach (var score in game.Scores)
            {
                Scores.Add(score);
            }

            for (int i = 0; i < 5; i++)
            {
                Hands[1].Cards.Add(Hands[0].Cards[0]);
                Hands[0].Cards.RemoveAt(0);
                Hands[2].Cards.Add(Hands[0].Cards[0]);
                Hands[0].Cards.RemoveAt(0);
            }
        }

        public void PostMoveRefresh()
        {
            for (int i = 1; i < Hands.Count(); i++)
            {                
                //assemble a list
                var cards = new List<int>();
                for (int j = 0; j < 13; j++) cards.Add(0);
                foreach (var c in Hands[i].Cards)
                {
                    cards[c.Number]++;
                }

                //check for books
                for (int j = 0; j < 13; j++)
                {
                    if (cards[j] == 4)
                    {
                        //remove all four of the cards
                        foreach (var c in Hands[i].Cards)
                        {
                            if (c.Number == j)
                            {
                                Hands[i].Cards.Remove(c);
                            }
                        }
                        //add point
                        Scores[i - 1]++;
                    }
                }

                //redraw if hand empty and able
                if (Hands[i].Cards.Count() == 0 && Hands[0].Cards.Count() > 0)
                {
                    for (int j = 0; j < 5 && Hands[0].Cards.Count() > 0; j++)
                    {
                        var c = Hands[0].Cards[0];
                        Hands[i].Cards.Add(c);
                        Hands[0].Cards.Remove(c);
                    }
                }
            }
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, e);
        }
    }
}
