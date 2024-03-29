﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoFish.Models
{
    class Card
    {
        public int Number { get; }
        public int Suit { get; }

        public Card(int number, int suit)
        {
            Number = number;
            Suit = suit;
        }

        public bool Match(Card c)
        {
             return (c.Suit == Suit && c.Number == Number);
        }

        public bool GoFishMatch(Card c)
        {
            return c.Suit == Suit;
        }
    }
}
