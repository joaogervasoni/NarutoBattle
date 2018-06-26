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
        public int attack(int atual_life, int skillSelect, string attackChar)
        {
            int damage = int.Parse(damage_skill(skillSelect.ToString(), attackChar));

            if (atual_life == 0)
            {
                return atual_life;
            }
            //receive skill damage

            int return_value = atual_life - damage;
            if (return_value < 0)
            {
                return_value = 0;
            }

            return return_value;
        }

        public int heal(int atual_life, int skillSelect, string attackChar)
        {
            int heal = int.Parse(heal_skill(skillSelect.ToString(), attackChar));

            if (atual_life == 0)
            {
                return atual_life;
            }

            int return_value = atual_life + heal;
            if (return_value > 100)
            {
                return_value = 100;
            }

            return return_value;
        }

    }
}
