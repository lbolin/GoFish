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

        public GameViewModel()
        {
            game = new Game();

            Hands = new ObservableCollection<HandViewModel>();

            foreach (var hand in game.Hands)
            {
                var newHand = new HandViewModel(hand);
                newHand.PropertyChanged += OnPropertyChanged;
                Hands.Add(newHand);
            }

            for (int i = 0; i < 5; i++)
            {
                Hands[1].Cards.Add(Hands[0].Cards[0]);
                Hands[0].Cards.RemoveAt(0);
                Hands[2].Cards.Add(Hands[0].Cards[0]);
                Hands[0].Cards.RemoveAt(0);
            }
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, e);
        }
    }
}
