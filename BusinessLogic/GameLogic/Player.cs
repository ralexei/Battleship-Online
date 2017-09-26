using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.GameLogic
{
    public class Player : IPlayer
    {
        public Player()
        {
            Field = new FieldDefault();
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public Field Field { get; set; }
    }
}
