using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace BusinessLogic.GameLogic
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum CellStatus
    {
        NotSet,
        Alive,
        Hit,
        Dead,
        Missed,
        AutoMissed,
        Hidden
    }

    public class Cell
    {
        public Cell()
        {
            Position = new Point();
        }

        public Point Position { get; set; }
        public CellStatus Status { get; set; }
    }
}
