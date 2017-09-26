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
    public class ShipTests
    {
        [TestMethod()]
        public void ShipCtorTest()
        {
            int size = 1;
            int id = 1;
            Ship ship = new Ship(id, size);
            Assert.AreEqual(size, ship.Size);
            Assert.AreEqual(id, ship.Id);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void ShipCtorTest_InvalidSize()
        {
            int size = -2;
            int id = 1;
            Ship ship = new Ship(id, size);
        }

        [TestMethod()]
        public void CreateNewShipTest()
        {
            int size = 4;
            int id = 1;
            Orientation orientation = Orientation.Vertical;
            Point start = new Point(1);
            Ship ship = Ship.CreateNewShip(start, orientation, id, size);
            Assert.IsTrue(ship.IsSet());
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateNewShipTest_InvalidSize()
        {
            int size = -2;
            int id = 1;
            Orientation orientation = Orientation.Vertical;
            Point start = new Point(1);
            Ship ship = Ship.CreateNewShip(start, orientation, id, size);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetCellTest_UnsetShip()
        {
            Ship ship = new Ship(1, 1);
            ship.GetCell(new Point());
        }

        [TestMethod()]
        public void GetCellTest_InvalidPoint()
        {
            Ship ship = Ship.CreateNewShip(new Point(1), Orientation.Vertical, 1, 4);
            Assert.IsNull(ship.GetCell(new Point(0)));
        }

        [TestMethod()]
        public void GetCellTest()
        {
            Ship ship = Ship.CreateNewShip(new Point(1), Orientation.Vertical, 1, 4);
            for (int i = 0; i < ship.Size; i++)
            {
                ReferenceEquals(ship.Cells[i], ship.GetCell(ship.Cells[i].Position));
            }
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void IsAliveTest_UnsetShip()
        {
            Ship ship = new Ship(1, 1);
            ship.IsAlive();
        }

        [TestMethod()]
        public void IsAliveTest_AliveShip()
        {
            Ship ship = Ship.CreateNewShip(new Point(1), Orientation.Vertical, 1, 1);
            Assert.IsTrue(ship.IsAlive());
        }

        [TestMethod()]
        public void IsAliveTest_DeadShip()
        {
            Ship ship = Ship.CreateNewShip(new Point(1), Orientation.Vertical, 1, 1);
            for (int i = 0; i < ship.Size; i++)
            {
                ship.Cells[i].Status = CellStatus.Dead;
            }
            Assert.IsFalse(ship.IsAlive());
        }

        [TestMethod()]
        public void IsSet_UnsetShip()
        {
            Ship ship = new Ship(1, 1);
            Assert.IsFalse(ship.IsSet());
        }

        [TestMethod()]
        public void IsSet_SetShip()
        {
            Ship ship = Ship.CreateNewShip(new Point(1), Orientation.Vertical, 1, 1);
            Assert.IsTrue(ship.IsSet());
        }
    }
}