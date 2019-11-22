using GoFish.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace GoFish.ViewModels
{
    class CardViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private Card card;

        public int Number
        {
            get
            {
                return card.Number;
            }
        }

        public int Suit
        {
            get
            {
                return card.Suit;
            }
        }

        public CardViewModel(Card c)
        {
            card = c;
        }

        public bool Match(CardViewModel cvm)
        {
            return card.Match(cvm.card);
        }

        public bool GoFishMatch(CardViewModel cvm)
        {
            return card.GoFishMatch(cvm.card);
        }

        public Card GetModel()
        {
            return card;
        }

        private void OnPropertyChanged(string property)
        {
            // Notify any controls bound to the ViewModel that the property changed
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

    }
}
