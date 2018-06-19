using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;



namespace Controllers
{

    public class BattleController
    {

        public int Turn { get; set; }
        public int Turn_play { get; set; }
        public string Character1_red { get; set; }
        public string Character2_red { get; set; }
        public string Character3_red { get; set; }


        private PlayerController playc;
        private IAController iac;

        public BattleController()
        {
            playc = new PlayerController();
            iac = new IAController();
            Turn_play = 0;
            
        }


        public int printturno()
        {
            return Turn_play;
        }

        public void turn()
        {
            //1 red
            //2 blue
            int atual_play = Turn_play;
            if (atual_play == 1)
            {
                Turn_play = 2;
            }
            else if (atual_play == 2)
            {
                Turn_play = 1;
            }
        }


        public bool initial_turn()
        {
            //1 red
            //2 blue
            Random random = new Random();
            Turn_play = random.Next(1,3);

            if (Turn_play == 2)
            {
                return true;
            }

            return false;
        }

        public int pass_turn(object atual_turn)
        {
            int atual_turn_int = conversion_object_toint(atual_turn);

            if (atual_turn_int != Turn)
            {
                //erro
            }

            atual_turn_int = Turn += 1;

            turn();
            return atual_turn_int;
        }

        public int attack_choose(int skillSelect ,object life1, object life2, object life3, string name)
        {
            if (skillSelect != 0 && Turn_play == 1)
            {
                int characterNumber = convert_name_int(name);
                if (characterNumber == 1)
                {
                    if (conversion_object_toint(life1) == 0)
                    {
                        return 0;
                    }
                    return 1;
                }
                else if (characterNumber == 2)
                {
                    if (conversion_object_toint(life2) == 0)
                    {
                        return 0;
                    }
                    return 2;
                }
                else if (characterNumber == 3)
                {
                    if (conversion_object_toint(life3) == 0)
                    {
                        return 0;
                    }
                    return 3;
                }
            }
            return 0;

        }

        public int attack_red(object life, int skillSelect, string attackChar)
        {
            

            if (Turn_play == 1)
            {
                int life_int = conversion_object_toint(life);
                return playc.attack(life_int, skillSelect, attackChar);
            }
            else
            {
                int life_int = conversion_object_toint(life);
                return life_int;
            }
            
        }

        public int attack_blue(object life)
        {
            int life_int = conversion_object_toint(life);

            return iac.attack(life_int);
        }

        public bool skill_select(int skill)
        {
            if (skill != 0)
            {

                return true;
            }
            return false;
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

        public int convert_name_int(string name)
        {

            name = String.Join("", System.Text.RegularExpressions.Regex.Split(name, @"[^\d]"));
            return int.Parse(name);
        }

        public int conversion_object_toint(object obj)
        {
            int obj_int = int.Parse(obj.ToString());
            return obj_int;
        }
    }

}
