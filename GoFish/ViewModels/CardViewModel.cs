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

        private Card myCard;

        public int Number
        {
            get
            {
                return myCard.Number;
            }
        }

        public int Suit
        {
            get
            {
                return myCard.Suit;
            }
        }

        public CardViewModel(Card c)
        {
            myCard = c;
        }

        public bool match(CardViewModel cvm)
        {
            return myCard.match(cvm.myCard);
        }

        private void OnPropertyChanged(string property)
        {
            // Notify any controls bound to the ViewModel that the property changed
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

    }
}
