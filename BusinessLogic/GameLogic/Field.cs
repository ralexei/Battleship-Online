using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.GameLogic
{
    public enum ShotResult
    {
        Miss,
        Hit
    }

    public abstract class Field
    {
        public Field()
        {
            ShipsInfo = new Dictionary<int, int>();
            Config();
            CheckConfig();
            Cells = new CellStatus[FieldSize, FieldSize];
            Ships = new List<Ship>();
            foreach (var item in ShipsInfo)
            {
                for (int i = 0; i < item.Value; i++)
                {
                    Ships.Add(new Ship(Ships.Count, item.Key));
                }
            }
        }

        public int FieldSize { get; protected set; }
        public Dictionary<int, int> ShipsInfo { get; protected set; }

        [JsonProperty("cells")]
        public CellStatus[,] Cells { get; protected set; }
        [JsonProperty("ships")]
        public List<Ship> Ships { get; protected set; }

        public List<Ship> GetDeadShips
        {
            get
            {
                List<Ship> deadShips = new List<Ship>();
                foreach (var ship in Ships)
                {
                    if (!ship.IsAlive())
                    {
                        deadShips.Add(ship);
                    }
                }
                return deadShips;
            }
        }

        public CellStatus[,] CellsForEnemy
        {
            get
            {
                CellStatus[,] hiddenCells = new CellStatus[FieldSize, FieldSize];
                for (int i = 0; i < FieldSize; i++)
                {
                    for (int j = 0; j < FieldSize; j++)
                    {
                        if (Cells[i, j] == CellStatus.NotSet || Cells[i, j] == CellStatus.Alive)
                        {
                            hiddenCells[i, j] = CellStatus.Hidden;
                        }
                        else
                        {
                            hiddenCells[i, j] = Cells[i, j];
                        }
                    }
                }
                return hiddenCells;
            }
        }

        abstract protected void Config();

        private void CheckConfig()
        {
            if (FieldSize < 1)
            {
                throw new ArgumentException("Invalid field size");
            }
            if (!ShipsInfo.Any())
            {
                throw new ArgumentException("Field has no ships");
            }
            foreach (var item in ShipsInfo)
            {
                if (item.Key > FieldSize)
                {
                    throw new ArgumentException("Ship size " + item.Key.ToString() + " is too big");
                }
            }
            foreach (var item in ShipsInfo)
            {
                if (item.Key < 1)
                {
                    throw new ArgumentException("Invalid ship size " + item.Key.ToString());
                }
            }
        }

        public virtual void SetRandomShips()
        {
            Random rand = new Random();
            Ship tempShip;
            ResetField();
            Ships = Ships.OrderBy(x => x.Size).Reverse().ToList();
            foreach (var ship in Ships.ToList())
            {
                do
                {
                    tempShip = Ship.CreateNewShip(
                        new Point(rand.Next(FieldSize), rand.Next(FieldSize)),
                        (Orientation)rand.Next(Enum.GetNames(typeof(Orientation)).Length),
                        ship.Id,
                        ship.Size
                    );
                } while (!CanShipBeSet(tempShip));
                SetShip(tempShip);
            }
        }

        public void SetShipsList(List<Ship> ships)
        {
            if (!IsShipListRespectsConfig(ships))
            {
                throw new ArgumentException("Ship list violates configuration");
            }
            CellStatus[,] backupCells = Cells;
            List<Ship> backupShips = Ships;
            ResetField();
            Ship tempShip;
            foreach (var ship in ships)
            {
                tempShip = Ship.CreateNewShip(ship.StartPosition, ship.ShipOrientation, ship.Id, ship.Size);
                if (!CanShipBeSet(tempShip))
                {
                    Cells = backupCells;
                    Ships = backupShips;
                    throw new ArgumentException("Ships are overlapping, or out of field");
                }
                SetShip(tempShip);
            }
        }

        protected virtual bool IsShipListRespectsConfig(List<Ship> ships)
        {
            List<Ship> tempList = new List<Ship>();
            foreach (var ship in ships)
            {
                tempList.Add(ship);
            }
            foreach (var shipInfo in ShipsInfo)
            {
                for (int i = 0; i < shipInfo.Value; i++)
                {
                    var ship = tempList.Find(x => x.Size == shipInfo.Key);
                    if (ship != null)
                    {
                        tempList.Remove(ship);
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            if (tempList.Any())
            {
                return false;
            }
            return true;
        }
        
        public virtual void SetShip(Ship shipToSet)
        {
            if (!shipToSet.IsSet())
            {
                shipToSet = Ship.CreateNewShip(
                    shipToSet.StartPosition,
                    shipToSet.ShipOrientation,
                    shipToSet.Id,
                    shipToSet.Size
                );
            }
            Ship ship;
            int shipIndex;
            try
            {
                ship = Ships.Single(x => x.Id == shipToSet.Id);
                shipIndex = Ships.IndexOf(ship);
            }
            catch (InvalidOperationException)
            {
                throw new ArgumentException("No such ship Id");
            }
            if (ship.IsSet())
            {
                DeleteShipFromField(ship.Id);
            }
            if (!CanShipBeSet(shipToSet))
            {
                if (ship.IsSet())
                    AddShipOnField(ship);
                throw new ArgumentException("Ship can not be set.");
            }
            Ships[shipIndex] = shipToSet;
            if (!IsShipListRespectsConfig(Ships))
            {
                Ships[shipIndex] = ship;
                throw new ArgumentException("Ship violates configuration");
            }
            AddShipOnField(shipToSet);
        }

        protected void AddShipOnField(Ship ship)
        {
            foreach (var cell in ship.Cells)
            {
                Cells[cell.Position.Y, cell.Position.X] = CellStatus.Alive;
            }
        }

        public void DeleteShipFromField(int shipId)
        {
            int index;
            try
            {
                index = Ships.IndexOf(Ships.Single(x => x.Id == shipId));
            }
            catch (InvalidOperationException)
            {
                throw new ArgumentException("Ship Id not found");
            }
            if (!Ships[shipId].IsSet())
            {
                throw new ArgumentNullException("Ship is not set");
            }
            foreach (var cell in Ships[index].Cells)
            {
                Cells[cell.Position.Y, cell.Position.X] = CellStatus.NotSet;
            }
        }

        protected virtual bool CanShipBeSet(Ship ship)
        {
            if (!ship.IsSet())
            {
                throw new ArgumentNullException("Ship is not set");
            }
            foreach (var cell in ship.Cells)
            {
                if (cell.Position.X < 0 || cell.Position.Y < 0 || cell.Position.X >= FieldSize || cell.Position.Y >= FieldSize)
                {
                    return false;
                }
            }
            Point start = new Point();
            Point end = new Point();
            start.X = ship.StartPosition.X <= 0 ? ship.StartPosition.X : ship.StartPosition.X - 1;
            start.Y = ship.StartPosition.Y <= 0 ? ship.StartPosition.Y : ship.StartPosition.Y - 1;
            end.X = ship.Cells[ship.Size - 1].Position.X >= FieldSize - 1 ?
                ship.Cells[ship.Size - 1].Position.X
                : ship.Cells[ship.Size - 1].Position.X + 1;
            end.Y = ship.Cells[ship.Size - 1].Position.Y >= FieldSize - 1 ?
                ship.Cells[ship.Size - 1].Position.Y
                : ship.Cells[ship.Size - 1].Position.Y + 1;

            for (int i = start.Y; i <= end.Y; i++)
            {
                for (int j = start.X; j <= end.X; j++)
                {
                    if (Cells[i, j] != CellStatus.NotSet)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public virtual ShotResult TakeShoot(Point point)
        {
            foreach (var item in Ships.ToList())
            {
                if (!item.IsSet())
                {
                    throw new ArgumentNullException("Ship is not set. Ship id: " + item.Id);
                }
            }
            if (!IsPointOnField(point))
            {
                throw new ArgumentException("Shot out of field");
            }
            if (IsCellShot(point))
            {
                throw new ArgumentException("Already Shoot");
            }
            Ship ship = GetShip(point);
            if (ship != null)
            {
                Cells[point.Y, point.X] = CellStatus.Hit;
                Cell cell = ship.GetCell(point);
                cell.Status = CellStatus.Dead;
                if (!ship.IsAlive())
                {
                    SetAutoMiss(ship);
                    foreach (var item in ship.Cells)
                    {
                        Cells[item.Position.Y, item.Position.X] = CellStatus.Dead;
                    }
                }
                else
                {
                    SetCornerAutoMiss(point);
                }
                return ShotResult.Hit;
            }
            else
            {
                Cells[point.Y, point.X] = CellStatus.Missed;
                return ShotResult.Miss;
            }
        }

        protected bool IsPointOnField(Point point)
        {
            return (!(point.X < 0 || point.Y < 0 || point.X >= FieldSize || point.Y >= FieldSize));
        }

        protected bool IsCellShot(Point point)
        {
            return (Cells[point.Y, point.X] != CellStatus.Alive && Cells[point.Y, point.X] != CellStatus.NotSet);
        }

        protected virtual void SetAutoMiss(Ship ship)
        {
            if (ship.IsAlive())
            {
                throw new ArgumentException("Ship is Alive");
            }
            Point start = new Point();
            Point end = new Point();
            start.X = ship.StartPosition.X <= 0 ? ship.StartPosition.X : ship.StartPosition.X - 1;
            start.Y = ship.StartPosition.Y <= 0 ? ship.StartPosition.Y : ship.StartPosition.Y - 1;
            end.X = ship.Cells[ship.Size - 1].Position.X >= FieldSize - 1 ?
                ship.Cells[ship.Size - 1].Position.X
                : ship.Cells[ship.Size - 1].Position.X + 1;
            end.Y = ship.Cells[ship.Size - 1].Position.Y >= FieldSize - 1 ?
                ship.Cells[ship.Size - 1].Position.Y
                : ship.Cells[ship.Size - 1].Position.Y + 1;
            for (int i = start.Y; i <= end.Y; i++)
            {
                for (int j = start.X; j <= end.X; j++)
                {
                    if (Cells[i, j] == CellStatus.NotSet)
                    {
                        Cells[i, j] = CellStatus.AutoMissed;
                    }
                }
            }
        }

        protected virtual void SetCornerAutoMiss(Point point)
        {
            if (point.Y - 1 >= 0 && point.X - 1 >= 0 && Cells[point.Y - 1, point.X - 1] == CellStatus.NotSet)
            {
                Cells[point.Y - 1, point.X - 1] = CellStatus.AutoMissed;
            }
            if (point.Y + 1 < FieldSize && point.X - 1 >= 0 && Cells[point.Y + 1, point.X - 1] == CellStatus.NotSet)
            {
                Cells[point.Y + 1, point.X - 1] = CellStatus.AutoMissed;
            }
            if (point.Y - 1 >= 0 && point.X + 1 < FieldSize && Cells[point.Y - 1, point.X + 1] == CellStatus.NotSet)
            {
                Cells[point.Y - 1, point.X + 1] = CellStatus.AutoMissed;
            }
            if (point.Y + 1 < FieldSize && point.X + 1 < FieldSize && Cells[point.Y + 1, point.X + 1] == CellStatus.NotSet)
            {
                Cells[point.Y + 1, point.X + 1] = CellStatus.AutoMissed;
            }
        }

        public Ship GetShip(Point point)
        {
            foreach (var ship in Ships)
            {
                foreach (var cell in ship.Cells)
                {
                    if (point.X == cell.Position.X && point.Y == cell.Position.Y)
                    {
                        return ship;
                    }
                }
            }
            return null;
        }

        public void ResetField()
        {
            for (int i = 0; i < FieldSize; i++)
            {
                for (int j = 0; j < FieldSize; j++)
                {
                    Cells[i, j] = CellStatus.NotSet;
                }
            }
        }
    }
}
