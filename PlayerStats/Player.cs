using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayerStats
{
    class Player
    {
        public string Name { get; set; }
        public ushort PlayingSince { get; set; }
        //PG SG SF PF C
        public Position Position { get; set; }
        public byte Rating { get; set; }
    }
}
