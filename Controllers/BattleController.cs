using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Controllers
{

    public class BattleController
    {
        private Battle batt;
        private PlayerController playc;
        private IAController iac;

        public BattleController()
        {
            playc = new PlayerController();
            iac = new IAController();
            batt = new Battle();
            initial_turn();
            
        }

        public void turn(int atual_play)
        {
            //1 red
            //2 blue
            if (atual_play == 1)
            {
                batt.Turn_play = 2;
            }
            else if (atual_play == 2)
            {
                batt.Turn_play = 1;
            }
        }

        public void initial_turn()
        {
            //1 red
            //2 blue
            Random random = new Random();
            batt.Turn_play = random.Next(1,2);
        }

        public int pass_turn(object atual_turn)
        {
            int atual_turn_int = conversion_object_toint(atual_turn);

            if (atual_turn_int != batt.Turn)
            {
                //erro
            }

            atual_turn_int = batt.Turn += 1;
            return atual_turn_int;
        }

        public int attack_red(object life)
        {
            int life_int = conversion_object_toint(life);
            return playc.attack(life_int);
        }

        public bool dead_confirmation(object life)
        {
            int life_int = conversion_object_toint(life);
            if (life_int <= 0)
            {
                return true;
            }

            return false;
        }

        public int conversion_object_toint(object obj)
        {
            int obj_int = int.Parse(obj.ToString());
            return obj_int;
        }
    }

}
