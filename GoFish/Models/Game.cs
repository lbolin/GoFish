////using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoFish.Models
{
    class Game
    {
        private List<Hand> hands;

        public Game()
        {
            Hand opponent= new Hand();
            Hand player = new Hand();
            Hand deck = new Hand(true);

            hands.Add(deck);
            hands.Add(player);
            hands.Add(opponent);

        }

        public Hand getHand(int index)
        {
            if (index != 0 && index != 1 && index != 2)
            {
                return null;
            }
            else
            {
                return hands[index];
            }
        }
    }
}
