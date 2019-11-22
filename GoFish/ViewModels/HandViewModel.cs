using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using GoFish.Models;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

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

            foreach (var card in h.cards)
            {
                var newCard = new CardViewModel(card);
                newCard.PropertyChanged += OnPropertyChanged;
                Cards.Add(newCard);
            }

        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, e);
        }
    }
}
