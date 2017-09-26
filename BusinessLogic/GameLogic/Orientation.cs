using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.GameLogic
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum Orientation
    {
        Vertical,
        Horizontal
    }

    public static class OrientationMethods
    {
        public static void AdjustShip(this Orientation orientation, Ship ship)
        {
            Point current = new Point(ship.StartPosition.X, ship.StartPosition.Y);
            switch (ship.ShipOrientation)
            {
                case Orientation.Vertical:
                    for (int i = current.Y; i < ship.StartPosition.Y + ship.Size; i++)
                    {
                        ship.Cells[i - ship.StartPosition.Y] = new Cell
                        {
                            Status = CellStatus.Alive,
                            Position = new Point(current.X, current.Y)
                        };
                        current.Y++;
                    }
                    break;
                case Orientation.Horizontal:
                    for (int i = current.X; i < ship.StartPosition.X + ship.Size; i++)
                    {
                        ship.Cells[i - ship.StartPosition.X] = new Cell
                        {
                            Status = CellStatus.Alive,
                            Position = new Point(current.X, current.Y)
                        };
                        current.X++;
                    }
                    break;
                default:
                    throw new NotImplementedException("AdjustShip() is not implemented for " + orientation);
            }
        }
    }
}
