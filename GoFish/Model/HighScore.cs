using GoFish.Models;

namespace Model
{
    internal class HighScore : GoFish.Models.HighScore
    {
        public HighScore(string name, int books, int time) : base(name, books, time)
        {
        }
    }
}