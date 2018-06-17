using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Battle
    {
        public int Turn { get; set; }
        public int Turn_play { get; set; }
        public string Character1_red { get; set; }
        public string Character2_red { get; set; }
        public string Character3_red { get; set; }

        //Red Team = agress
        //Blue team = IA
    }
}
