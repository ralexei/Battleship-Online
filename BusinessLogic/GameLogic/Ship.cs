using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogic.GameLogic
{
    public class Ship
    {
        private int _size;
        
        public Ship(int id, int size)
        {
            Id = id;
            Size = size;
            Cells = new Cell[size];
            StartPosition = new Point();
        }

        public int Id { get; }
        public int Size
        {
            get { return _size; }
            private set
            {
                if (value < 1)
                {
                    throw new ArgumentException("Invalid ship size");
                }
                _size = value;
            }
        }
        public Cell[] Cells { get; }
        public Point StartPosition { get; set; }
        public Orientation ShipOrientation { get; set; }

        public static Ship CreateNewShip(Point startPosition, Orientation shipOrientation, int id, int size)
        {
            Ship ship = new Ship(id, size)
            {
                ShipOrientation = shipOrientation,
                StartPosition = startPosition
            };
            ship.ShipOrientation.AdjustShip(ship);
            return ship;
        }

        public Cell GetCell(Point point)
        {
            if (!IsSet())
            {
                throw new ArgumentNullException("Ship is not set");
            }
            foreach (var cell in Cells)
            {
                if (cell.Position.X == point.X && cell.Position.Y == point.Y)
                {
                    return cell;
                }
            }
            return null;
        }

        public bool IsAlive()
        {
            if (!IsSet())
            {
                throw new ArgumentNullException("Ship is not set");
            }
            foreach (var cell in Cells)
            {
                if (cell.Status == CellStatus.Alive)
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsSet()
        {
            foreach (var cell in Cells)
            {
                if (cell == null)
                {
                    return false;
                }
            }
            return true;
        }
    }
}