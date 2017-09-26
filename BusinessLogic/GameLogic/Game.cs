using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace BusinessLogic.GameLogic
{
    public abstract class Game
    {
        public Game()
        {
            Players = new Player[2];
        }

        public IPlayer[] Players { get; set; }
        public int CurrentPlayer { get; set; }
        public int OpponentPlayer { get; set; }

        abstract public void Start();

        public bool IsNotGameOver()
        {
            foreach (var ship in Players[OpponentPlayer].Field.Ships)
            {
                if (ship.IsAlive())
                {
                    return true;
                }
            }
            return false;
        }
    }
}
