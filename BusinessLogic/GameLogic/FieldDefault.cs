using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.GameLogic
{
    public class FieldDefault : Field
    {
        protected override void Config()
        {
            FieldSize = 10;
            ShipsInfo.Add(1, 4);
            ShipsInfo.Add(2, 3);
            ShipsInfo.Add(3, 2);
            ShipsInfo.Add(4, 1);
        }
    }
}
