using GoFish.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoFish.Models
{
   
    class HighScore {
        public string Name { get; }
        public int Books { get; }
        public int Time { get; }

    



        public HighScore(string name, int books, int time)
        {
            Name = name;
            Books = books;
            Time = time;
        }

        
    }
}
