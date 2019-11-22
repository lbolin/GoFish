using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoFish.Models
{
    class Game
    {
        public List<Hand> Hands { get; set; }

        public Game()
        {
            Hands = new List<Hand>();

            Hand opponent= new Hand();
            Hand player = new Hand();
            Hand deck = new Hand(true);

            Hands.Add(deck);
            Hands.Add(player);
            Hands.Add(opponent);
        }
    }
}
