using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogic.GameLogic
{
    public interface IPlayer
    {
        string Id { get; set; }
        string Name { get; set; }
        Field Field { get; set; }
    }
}
