using Microsoft.VisualStudio.TestTools.UnitTesting;
using BusinessLogic.GameLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.GameLogic.Tests
{
    [TestClass()]
    public class FieldTests
    {
        [TestMethod()]
        public void FieldTest_ConfigCheck_ValidField()
        {
            Field field = new Field_Valid_TwoShips();
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void FieldTest_ConfigCheck_FieldNoSize()
        {
            Field field = new Field_NoSize();
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void FieldTest_ConfigCheck_FieldNoShips()
        {
            Field field = new Field_NoShips();
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void FieldTest_ConfigCheck_FieldInvalidShipSize()
        {
            Field field = new Field_InvalidShipSize();
        }

        [TestMethod()]
        public void CellsForEnemyTest_RandomValues()
        {
            Field field = new Field_Valid_OneShip();
            Random rnd = new Random();
            for (int i = 0; i < field.FieldSize; i++)
            {
                for (int j = 0; j < field.FieldSize; j++)
                {
                    do
                    {
                        field.Cells[i, j] = (CellStatus)rnd.Next(Enum.GetNames(typeof(CellStatus)).Length);
                    } while (field.Cells[i, j] != CellStatus.Hidden);
                }
            }
            for (int i = 0; i < field.FieldSize; i++)
            {
                for (int j = 0; j < field.FieldSize; j++)
                {
                    if (field.Cells[i, j] == CellStatus.Alive || field.Cells[i, j] == CellStatus.NotSet)
                    {
                        Assert.AreEqual(CellStatus.Hidden, field.CellsForEnemy[i, j]);
                    }
                    else
                    {
                        Assert.AreEqual(field.Cells[i, j], field.CellsForEnemy[i, j]);
                    }
                }
            }
        }

        [TestMethod()]
        public void CellsForEnemyTest_Hidden()
        {
            Field field = new Field_Valid_OneShip();

            foreach (var cell in field.CellsForEnemy)
            {
                Assert.AreEqual(CellStatus.Hidden, cell);
            }
            for (int i = 0; i < field.FieldSize; i++)
            {
                for (int j = 0; j < field.FieldSize; j++)
                {
                    field.Cells[i, j] = CellStatus.Alive;
                }
            }
            foreach (var cell in field.CellsForEnemy)
            {
                Assert.AreEqual(CellStatus.Hidden, cell);
            }
        }

        [TestMethod()]
        public void SetRandomShipsTest_CheckShipToBeSetted()
        {
            Field field = new Field_Valid_TwoShips();
            field.SetRandomShips();
            foreach (var ship in field.Ships)
            {
                foreach (var cell in ship.Cells)
                {
                    if (cell == null)
                    {
                        Assert.Fail("Ship is not setted");
                    }
                    if (field.Cells[cell.Position.Y, cell.Position.X] != CellStatus.Alive)
                    {
                        Assert.Fail("Ship not added on field");
                    }
                }
            }
        }

        [TestMethod()]
        public void SetShipsListTest_ValidList()
        {
            Field field = new Field_Valid_TwoShips();

            field.SetRandomShips();
            field.SetShipsList(field.Ships.ToList());
            foreach (var ship in field.Ships)
            {
                foreach (var cell in ship.Cells)
                {
                    if (cell == null)
                    {
                        Assert.Fail("Ship is not setted");
                    }
                    if (field.Cells[cell.Position.Y, cell.Position.X] != CellStatus.Alive)
                    {
                        Assert.Fail("Ship not added on field");
                    }
                }
            }
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void SetShipsListTest_ConfigViolation()
        {
            Field field = new Field_Valid_TwoShips();

            List<Ship> list = new List<Ship>();
            int id = 0;
            foreach (var ship in field.Ships)
            {
                list.Add(new Ship(id++, ship.Size));
            }
            list.Add(new Ship(id, 3));
            field.SetShipsList(list);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void SetShipsListTest_OutOfFieldShips()
        {
            Field field = new Field_Valid_TwoShips();
            field.SetRandomShips();
            List<Ship> list = new List<Ship>();
            foreach (var item in field.Ships)
            {
                list.Add(item);
            }
            list[0].StartPosition = new Point(field.FieldSize);
            field.SetShipsList(list);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void SetShipsListTest_OverlappingShips()
        {
            Field field = new Field_Valid_TwoShips();
            field.SetRandomShips();
            List<Ship> list = new List<Ship>();
            foreach (var item in field.Ships)
            {
                list.Add(item);
            }
            list[0].StartPosition = new Point(0);
            list[1].StartPosition = new Point(0);
            field.SetShipsList(list);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void SetShipTest_InvalidShipId()
        {
            Field field = new Field_Valid_OneShip();

            Ship ship = Ship.CreateNewShip(
                field.Ships[0].StartPosition,
                Orientation.Horizontal,
                field.Ships[0].Id + 1,
                field.Ships[0].Size
            );
            field.SetShip(ship);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void DeleteShipFromFieldTest_InvalidShipId()
        {
            Field field = new Field_Valid_OneShip();
            field.DeleteShipFromField(field.Ships[0].Id + 1);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DeleteShipFromFieldTest_NotSetShip()
        {
            Field field = new Field_Valid_OneShip();
            field.DeleteShipFromField(field.Ships[0].Id);
        }

        [TestMethod()]
        public void DeleteShipFromFieldTest_ValidShip()
        {
            Field field = new Field_Valid_OneShip();
            field.SetRandomShips();
            field.DeleteShipFromField(field.Ships[0].Id);
            foreach (var cell in field.Cells)
            {
                Assert.AreEqual(CellStatus.NotSet, cell);
            }
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TakeShootTest_NotSetShip()
        {
            Field field = new Field_Valid_OneShip();
            field.TakeShoot(new Point(0));
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void TakeShootTest_PointOutOfField()
        {
            Field field = new Field_Valid_OneShip();
            field.SetRandomShips();
            field.TakeShoot(new Point(-1));
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void TakeShootTest_AleadyShooted()
        {
            Field field = new Field_Valid_OneShip();
            field.SetRandomShips();
            field.TakeShoot(new Point(0));
            field.TakeShoot(new Point(0));
        }

        [TestMethod()]
        public void TakeShootTest_Miss()
        {
            Field field = new Field_Valid_OneShip();
            Ship ship = Ship.CreateNewShip(new Point(1), Orientation.Vertical, field.Ships[0].Id, field.Ships[0].Size);
            field.SetShip(ship);
            Point point = new Point(0);
            Assert.AreEqual(ShotResult.Miss, field.TakeShoot(point));
            Assert.AreEqual(CellStatus.Missed, field.Cells[point.Y, point.X]);
        }

        [TestMethod()]
        public void TakeShootTest_KillHit()
        {
            Field field = new Field_Valid_OneShip_Size1();
            Point point = new Point(1);
            Ship ship = Ship.CreateNewShip(point, Orientation.Vertical, field.Ships[0].Id, field.Ships[0].Size);
            field.SetShip(ship);
            Assert.AreEqual(ShotResult.Hit, field.TakeShoot(point));
            Assert.AreEqual(CellStatus.Dead, field.Cells[point.Y, point.X]);
        }

        [TestMethod()]
        public void TakeShootTest_Hit()
        {
            Field field = new Field_Valid_OneShip();
            Point point = new Point(1);
            Ship ship = Ship.CreateNewShip(point, Orientation.Vertical, field.Ships[0].Id, field.Ships[0].Size);
            field.SetShip(ship);
            Assert.AreEqual(ShotResult.Hit, field.TakeShoot(point));
            Assert.AreEqual(CellStatus.Hit, field.Cells[point.Y, point.X]);
        }

        [TestMethod()]
        public void TakeShootTest_CornerAutoMiss()
        {
            Field field = new Field_Valid_OneShip();
            Point point = new Point(1);
            Ship ship = Ship.CreateNewShip(point, Orientation.Vertical, field.Ships[0].Id, field.Ships[0].Size);
            field.SetShip(ship);
            Assert.AreEqual(ShotResult.Miss, field.TakeShoot(new Point(point.X - 1, point.Y - 1)));
            Assert.AreEqual(CellStatus.Missed, field.Cells[point.Y - 1, point.X - 1]);
            Assert.AreEqual(ShotResult.Hit, field.TakeShoot(point));
            Assert.AreEqual(CellStatus.Hit, field.Cells[point.Y, point.X]);
            Assert.AreEqual(CellStatus.Missed, field.Cells[point.Y - 1, point.X - 1]);
            Assert.AreEqual(CellStatus.AutoMissed, field.Cells[point.Y + 1, point.X - 1]);
            Assert.AreEqual(CellStatus.AutoMissed, field.Cells[point.Y - 1, point.X + 1]);
            Assert.AreEqual(CellStatus.AutoMissed, field.Cells[point.Y + 1, point.X + 1]);
        }

        [TestMethod()]
        public void TakeShootTest_Automiss()
        {
            Field field = new Field_Valid_OneShip_Size1();
            Point shipPos = new Point(1);
            Point miss = new Point(0);
            Ship ship = Ship.CreateNewShip(shipPos, Orientation.Vertical, field.Ships[0].Id, field.Ships[0].Size);
            field.SetShip(ship);
            Assert.AreEqual(ShotResult.Miss, field.TakeShoot(miss));
            Assert.AreEqual(CellStatus.Missed, field.Cells[miss.Y, miss.X]);
            Assert.AreEqual(ShotResult.Hit, field.TakeShoot(shipPos));
            Assert.AreEqual(CellStatus.Dead, field.Cells[shipPos.Y, shipPos.X]);
            Point start = new Point(0);
            Point end = new Point(2);
            for (int i = start.Y; i <= end.Y; i++)
            {
                for (int j = start.X; j <= end.X; j++)
                {
                    if (j == miss.X && i == miss.Y)
                    {
                        Assert.AreEqual(CellStatus.Missed, field.Cells[i, j]);
                    }
                    else if (!(j == shipPos.X && i == shipPos.Y))
                    {
                        Assert.AreEqual(CellStatus.AutoMissed, field.Cells[i, j]);
                    }
                }
            }
        }

        [TestMethod()]
        public void GetShipTest_NullTest()
        {
            Field field = new Field_Valid_OneShip();
            field.SetShip(Ship.CreateNewShip(
                new Point(1),
                Orientation.Vertical,
                field.Ships[0].Id,
                field.Ships[0].Size)
            );
            Assert.IsNull(field.GetShip(new Point(0)));
        }

        [TestMethod()]
        public void GetShipTest_ValidTest()
        {
            Field field = new Field_Valid_OneShip();
            field.SetShip(Ship.CreateNewShip(
                new Point(1),
                Orientation.Vertical,
                field.Ships[0].Id,
                field.Ships[0].Size)
            );
            ReferenceEquals(field.Ships[0], field.GetShip(new Point(1)));
        }

        [TestMethod()]
        public void ResetFieldTest()
        {
            Field field = new Field_Valid_TwoShips();
            field.SetRandomShips();
            field.ResetField();
            foreach (var cell in field.Cells)
            {
                Assert.AreEqual(CellStatus.NotSet, cell);
            }
        }
    }
}