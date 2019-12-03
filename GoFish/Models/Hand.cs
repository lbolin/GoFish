using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoFish.Models
{
    class Hand
    {
        public List<Card> cards;

        public Hand(bool fullDeck = false)
        {
            cards = new List<Card>();

            if (fullDeck)
            {
                for (int  i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 13; j++)
                    {
                        Card c = new Card(j, i);
                        cards.Add(c);
                    }
                }

                //shuffle the deck once it is created
                Random random = new Random();
                for (int i = 0; i < 5000; i++)
                {
                    for (int j = 1; j < cards.Count; j++)
                    {
                        if (random.Next(2) == 0)
                        {
                            Card temp = cards[j];
                            cards[j] = cards[j - 1];
                            cards[j - 1] = temp;
                        }
                    }
                }
            }
        }
    }
}
