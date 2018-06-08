using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controllers
{
    class PlayerController
    {
        public int attack(int atual_life)
        {
            //receive skill damage
            return atual_life - 25;
        }
    }
}
