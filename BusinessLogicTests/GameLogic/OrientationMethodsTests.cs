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
    public class OrientationMethodsTests
    {
        [TestMethod()]
        public void AdjustShipTest_Vertical()
        {
            int startX = 1;
            int startY = 1;
            Ship ship = new Ship(1, 4)
            {
                ShipOrientation = Orientation.Vertical,
                StartPosition = new Point(startX, startY)
            };
            ship.ShipOrientation.AdjustShip(ship);
            for (int i = 0; i < ship.Size; i++)
            {
                Assert.AreEqual(startX, ship.Cells[i].Position.X);
                Assert.AreEqual(startY + i, ship.Cells[i].Position.Y);
            }
        }

        [TestMethod()]
        public void AdjustShipTest_Horizontal()
        {
            int startX = 1;
            int startY = 1;
            Ship ship = new Ship(1, 4)
            {
                ShipOrientation = Orientation.Horizontal,
                StartPosition = new Point(startX, startY)
            };
            ship.ShipOrientation.AdjustShip(ship);
            for (int i = 0; i < ship.Size; i++)
            {
                Assert.AreEqual(startX + i, ship.Cells[i].Position.X);
                Assert.AreEqual(startY, ship.Cells[i].Position.Y);
            }
        }
    }
}