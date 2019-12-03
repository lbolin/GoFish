using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoFish.Models
{
    class HighScore
    {
        public string Name { get; }
        public int Books { get; }
        public int Time { get; }

        public HighScore(string name,int books,int time)
        {
            Name = name;
            Books = books;
            Time = time;
        }
    }
}
