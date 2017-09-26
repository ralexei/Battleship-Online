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
    public class PointTests
    {
        [TestMethod()]
        public void CtorTest_NoArg()
        {
            Point point = new Point();
            Assert.AreEqual(0, point.X);
            Assert.AreEqual(0, point.Y);
        }

        [TestMethod()]
        public void CtorTest_OneArg()
        {
            Point point = new Point(42);
            Assert.AreEqual(42, point.X);
            Assert.AreEqual(42, point.Y);
        }

        [TestMethod()]
        public void CtorTest_TwoArg()
        {
            Point point = new Point(42, 24);
            Assert.AreEqual(42, point.X);
            Assert.AreEqual(24, point.Y);
        }
    }
}