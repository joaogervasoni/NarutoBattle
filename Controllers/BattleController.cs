using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Controllers.DAL;



namespace Controllers
{

    public class BattleController : Tools
    {

        public int Turn { get; set; }
        public int Turn_play { get; set; }
        public string Character1_red { get; set; }
        public string Character2_red { get; set; }
        public string Character3_red { get; set; }


        private IAController iac;
        public ChakraController ChakraRed;
        public ChakraController ChakraBlue;


        public BattleController()
        {
            iac = new IAController();
            ChakraRed = new ChakraController();
            ChakraBlue = new ChakraController();
            Turn_play = 0;
            
        }


        public bool Have_Chakra(List<string> Skills)
        {
            if (ChakraRed.Taijutsu >= int.Parse(Skills[0]) && ChakraRed.Bloodline >= int.Parse(Skills[1]) &&
                ChakraRed.Ninjutsu >= int.Parse(Skills[2]) && ChakraRed.Genjutsu >= int.Parse(Skills[3]))
            {
                return true;
            }
            return false;
        }

        public List<string> Skills(string skillNumber, int char_select)
        {
            
            //retorna numero de chakras da skill
            List<string> chakras = new List<string>();

            if (char_select == 1)
            {
                chakras = Skills(skillNumber, Character1_red);
            }
            else if (char_select == 2)
            {
                chakras = Skills(skillNumber, Character2_red);
            }
            else if (char_select == 3)
            {
                chakras = Skills(skillNumber, Character3_red);
            }

            return chakras;
        }

        public List<int> Return_Chakras(int team)
        {
            if (team == 1)
            {
                return ChakraRed.returnChakras();
            }
            else if (team == 2)
            {
                return ChakraBlue.returnChakras();
            }

            return null;
        }

        public string printChakras()
        {
            return ChakraRed.printChakra() + ChakraBlue.printChakra();
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

        public string Skill_Type(int char_select, int skill_select)
        {
            string character = "";
            if (char_select == 1)
                character = Character1_red;
            else if (char_select == 2)
                character = Character2_red;
            else if (char_select == 3)
                character = Character3_red;

            return Skill_Type(character, skill_select);
        }

        //public int attack_red(object life, int skillSelect, string attackChar, string type_skill)
        //{
        //    int life_int;
        //    if (type_skill == "attack")
        //    {
        //        if (Turn_play == 1)
        //        {
        //            life_int = conversion_object_toint(life);
        //            return playc.attack(life_int, skillSelect, attackChar);
        //        }

        //    }
        //    else if (type_skill == "heal")
        //    {
        //        if (Turn_play == 1)
        //        {
        //            life_int = conversion_object_toint(life);
        //            return playc.heal(life_int, skillSelect, attackChar);
        //        }
        //    }

        //    life_int = conversion_object_toint(life);
        //    return life_int;
        //}

        public int attack_blue(object life)
        {
            int life_int = conversion_object_toint(life);

            return iac.attack(life_int);
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



        public string Attack_Char(int char_select)
        {
            string attack_char = "";
            if (char_select == 1)
            {
                attack_char = Character1_red;
            }
            else if (char_select == 2)
            {
                attack_char = Character2_red;
            }
            else if (char_select == 3)
            {
                attack_char = Character3_red;
            }

            return attack_char;
        }

        public void Withdraw_Chakra(List<string> skills)
        {
            ChakraRed.Taijutsu -= int.Parse(skills[0]);
            ChakraRed.Bloodline -= int.Parse(skills[1]);
            ChakraRed.Ninjutsu -= int.Parse(skills[2]);
            ChakraRed.Genjutsu -= int.Parse(skills[3]);
        }

        public int confirmation_teamDead(object content1, object content2, object content3, object content4, object content5, object content6)
        {
            if (conversion_object_toint(content1) <= 0 &&
                conversion_object_toint(content2) <= 0 &&
                conversion_object_toint(content3) <= 0)
            {
                return 2;
            }

            if (conversion_object_toint(content4) <= 0 &&
                conversion_object_toint(content5) <= 0 &&
                conversion_object_toint(content6) <= 0)
            {
                return 1;
            }

            return 0;
            
        }
    }

}
