using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using GoFish.Models;
using System.Collections.ObjectModel;

namespace GoFish.ViewModels
{
    class HandViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private Hand hand;
        public ObservableCollection<CardViewModel> Cards { get; set; }

        public HandViewModel(Hand h)
        {
            hand = h;

            Cards = new ObservableCollection<CardViewModel>();

            foreach (var card in h.getCards())
            {
                var newCard = new CardViewModel(card);
                newCard.PropertyChanged += OnPropertyChanged;
                Cards.Add(newCard);
            }

        }

        public void shuffle()
        {
            hand.shuffle();
        }

        public bool isEmpty()
        {
            return hand.isEmpty();
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // Notify any controls bound to the ViewModel that the property changed
            PropertyChanged?.Invoke(this, e);
        }
    }
}
