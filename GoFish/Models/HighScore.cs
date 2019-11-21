using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoFish.Models
{
    class HighScore
    {
        public int Name { get; }
        public int Books { get; }
        public int Time { get; }

        public HighScore(int name,int books,int time)
        {
            Name = name;
            Books = books;
            Time = time;
        }

        public HighScore GetHighScore(int score)
        {

        }

    }
}
