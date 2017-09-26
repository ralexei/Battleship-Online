using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLogic.GameLogic;

namespace BusinessLogic.GameLogic.Tests
{
    public class Field_Valid_OneShip_Size1 : Field
    {
        protected override void Config()
        {
            FieldSize = 10;
            ShipsInfo.Add(1, 1);
        }
    }

    public class Field_Valid_OneShip : Field
    {
        protected override void Config()
        {
            FieldSize = 10;
            ShipsInfo.Add(4, 1);
        }
    }
    public class Field_Valid_TwoShips : Field
    {
        protected override void Config()
        {
            FieldSize = 10;
            ShipsInfo.Add(1, 1);
            ShipsInfo.Add(4, 1);
        }
    }

    public class Field_NoSize : Field
    {
        protected override void Config()
        {
            ShipsInfo.Add(1, 4);
            ShipsInfo.Add(2, 3);
            ShipsInfo.Add(3, 2);
            ShipsInfo.Add(4, 1);
        }
    }

    public class Field_NoShips : Field
    {
        protected override void Config()
        {
            FieldSize = 10;
        }
    }

    public class Field_ShipSizeOverFieldSize : Field
    {
        protected override void Config()
        {
            FieldSize = 5;
            ShipsInfo.Add(6, 1);
        }
    }

    public class Field_InvalidShipSize : Field
    {
        protected override void Config()
        {
            FieldSize = 10;
            ShipsInfo.Add(-1, 1);
        }
    }
}
