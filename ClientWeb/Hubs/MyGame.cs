using BusinessLogic.GameLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClientWeb
{
    public class MyGame : Game
    {
        public int HasPlayer(string PlayerId)
        {
            if (Players[0].Id == PlayerId)
                return 0;
            if (Players[1].Id == PlayerId)
                return 1;
            return -1;
        }

        public IPlayer Current_Player()
        {
            return Players[CurrentPlayer];
        }

        public IPlayer Opponent_Player()
        {
            return Players[OpponentPlayer];
        }

        public override void Start()
        {
            CurrentPlayer = new Random().Next(2);
            OpponentPlayer = CurrentPlayer > 0 ? 0 : 1;
        }

        public Field GetMap(int Player)
        {
            return Players[Player].Field;
        }

        public CellStatus[,] GetCellStatus(int Player)
        {
            return Players[Player].Field.CellsForEnemy;
        }

        public List<Ship> GetDeadShips(int Player)
        {
            return Players[Player].Field.GetDeadShips;
        }

        public void TakeShoot(Point Position)
        {
            var cell = Opponent_Player().Field.Cells[Position.Y, Position.X];
            if (cell == CellStatus.Alive || cell == CellStatus.NotSet)
            {
                ShotResult result = Opponent_Player().Field.TakeShoot(Position);
                if (result == ShotResult.Miss)
                {
                    var i = CurrentPlayer;
                    CurrentPlayer = OpponentPlayer;
                    OpponentPlayer = i;
                }
            }
        }
    }
}