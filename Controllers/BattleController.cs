using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Controllers.DAL;
using System.Windows;



namespace Controllers
{

    public class BattleController : Tools
    {
        public int Turn { get; set; }
        public int Turn_play { get; set; }
        public string Character1_red { get; private set; }
        public string Character2_red { get; private set; }
        public string Character3_red { get; private set; }
        public string Account { get; set; }

        public int SkillSelect { get; set; }
        public string TypeSkill { get; set; }
        public string AttackChar { get; set; } = null;
        public int NumAttackChar { get; set; }

        private readonly Context Conn;
        private IAController iac;
        public ChakraController ChakraRed;
        public ChakraController ChakraBlue;
        List<string> ChakraSkill;

        public BattleController()
        {
            ChakraSkill = new List<string>();
            Conn = new Context();
            iac = new IAController();
            ChakraRed = new ChakraController();
            ChakraBlue = new ChakraController();
            Turn_play = 0;
        }

        //===================== Chakra ====================

        public bool Have_Chakra(List<string> Skill)
        {
            if (ChakraRed.Taijutsu >= int.Parse(Skill[0]) && ChakraRed.Bloodline >= int.Parse(Skill[1]) &&
                ChakraRed.Ninjutsu >= int.Parse(Skill[2]) && ChakraRed.Genjutsu >= int.Parse(Skill[3]))
            {
                return true;
            }
            return false;
        }

        public List<string> Chakra_Skill(int SkillNumber, int CharSelect)
        {
            string SkillNumberString = SkillNumber.ToString();
            List<string> SkillChakras = new List<string>();

            switch (CharSelect)
            {
                case 1:
                    {
                        SkillChakras = Conn.Skills(SkillNumberString, Character1_red);
                        break;
                    }
                case 2:
                    {
                        SkillChakras = Conn.Skills(SkillNumberString, Character2_red);
                        break;
                    }
                case 3:
                    {
                        SkillChakras = Conn.Skills(SkillNumberString, Character3_red);
                        break;
                    }
            }

            return SkillChakras;
        }

        public List<int> Return_Chakras(int Team)
        {
            if (Team == 1)
            {
                return ChakraRed.returnChakras();
            }
            else if (Team == 2)
            {
                return ChakraBlue.returnChakras();
            }

            return null;
        }

        public void Withdraw_Chakra(List<string> Skill)
        {
            ChakraRed.Taijutsu -= int.Parse(Skill[0]);
            ChakraRed.Bloodline -= int.Parse(Skill[1]);
            ChakraRed.Ninjutsu -= int.Parse(Skill[2]);
            ChakraRed.Genjutsu -= int.Parse(Skill[3]);
        }

        public void Deposit_Chakra(List<string> Skill)
        {
            ChakraRed.Taijutsu += int.Parse(Skill[0]);
            ChakraRed.Bloodline += int.Parse(Skill[1]);
            ChakraRed.Ninjutsu += int.Parse(Skill[2]);
            ChakraRed.Genjutsu += int.Parse(Skill[3]);
        }

        //===================== Turn ====================

        public string Return_Turn_Play()
        {
            if (Turn_play == 1)
            {
                return "Player";
            }
            else if (Turn_play == 2)
            {
                return "IA";
            }

            return null;
        }

        private void This_Turn()
        {
            //1 red //2 blue
            if (Turn_play == 1)
            {
                Turn_play = 2;
            }
            else if (Turn_play == 2)
            {
                Turn_play = 1;
            }
        }

        public bool Initial_Turn()
        {
            //1 red //2 blue
            Random random = new Random();
            Turn_play = random.Next(1, 3);

            if (Turn_play == 2)
            {
                return true;
            }

            return false;
        }

        public int Pass_Turn()
        {
            Turn += 1;
            This_Turn();
            return Turn;
        }

        //===================== Attack ====================

        public string Damage_Skill(int Skill, string Character)
        {
            string Damage = Conn.damage_skill(Skill.ToString(), Character);
            return Damage;
        }

        public string Heal_Skill(int Skill, string Character)
        {
            string Heal = Conn.heal_skill(Skill.ToString(), Character);
            return Heal;
        }

        public string Defense_Skill(int Skill, string Character)
        {
            string Defense = Conn.Defense_Skill(Skill.ToString(), Character);
            return Defense;
        }

        public string Skill_Type(int CharNumber, int SkillNumber)
        {
            string Character = Return_CharacterRed(CharNumber);
            return Conn.Skill_Type(Character, SkillNumber);
        }

        public bool Receive_Skill_Number(string SkillImageName)
        {
            string SkillName = Convert_Name_Int(SkillImageName).ToString();

            SkillSelect = int.Parse(SkillName.Last().ToString());
            NumAttackChar = int.Parse(SkillName.Remove(SkillName.Length - 1));

            ChakraSkill = Chakra_Skill(SkillSelect, NumAttackChar);
            bool HaveChakra = Have_Chakra(ChakraSkill);

            if (AttackChar == null && HaveChakra == true)
            {
                Withdraw_Chakra(ChakraSkill);
                AttackChar = Return_CharacterRed(NumAttackChar);
                TypeSkill = Skill_Type(NumAttackChar, SkillSelect);
                return true;
            }

            return false;
        }

        //===================== Others ====================

        public void Set_Characters_Red(string Char1, string Char2, string Char3)
        {
            Character1_red = Char1;
            Character2_red = Char2;
            Character3_red = Char3;
        }

        public void Set_Characters_Blue()
        {}

        public int attack_blue(object life)
        {
            int life_int = Conversion_Object_Toint(life);

            return iac.attack(life_int);
        }

        public bool Dead_Confirmation(object LabelContent)
        {
            int Life = Conversion_Object_Toint(LabelContent);
            if (Life <= 0)
            {
                return true;
            }

            return false;
        }

        public string Return_CharacterRed(int CharNumber)
        {
            string CharName = null;
            switch (CharNumber)
            {
                case 1:
                    {
                        CharName = Character1_red;
                        break;
                    }
                case 2:
                    {
                        CharName = Character2_red;
                        break;
                    }
                case 3:
                    {
                        CharName = Character3_red;
                        break;
                    }
            }
            return CharName;
        }

        public int Confirmation_TeamDead(object content1, object content2, object content3, object content4, object content5, object content6)
        {
            if (Conversion_Object_Toint(content1) <= 0 &&
                Conversion_Object_Toint(content2) <= 0 &&
                Conversion_Object_Toint(content3) <= 0)
            {
                return 2;
            }
            else
            if (Conversion_Object_Toint(content4) <= 0 &&
                Conversion_Object_Toint(content5) <= 0 &&
                Conversion_Object_Toint(content6) <= 0)
            {
                return 1;
            }

            return 0;
        }

        public void Save_Battle_Result(string Result)
        {
            Conn.WL(Account, Result);
        }

        public void Cancel_Attack()
        {
            Deposit_Chakra(ChakraSkill);
            AttackChar = null;
            SkillSelect = 0;
        }
    }

}
