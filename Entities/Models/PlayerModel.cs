using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class PlayerModel
    {
        public Int32 Id { get; set; }
        public string Name { get; set; }
        public Int32 Elo { get; set; }
        public string CurrentConnectionId { get; set; }
    }
}
