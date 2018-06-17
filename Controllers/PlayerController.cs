using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Controllers.DAL;

namespace Controllers
{
    class PlayerController : Context
    {
        public int attack(int atual_life)
        {
            if (atual_life == 0)
            {
                return atual_life;
            }
            //receive skill damage
            return atual_life - 25;
        }
        

    }
}
