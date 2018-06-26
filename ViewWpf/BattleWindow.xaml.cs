using Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ViewWpf
{
    /// <summary>
    /// Interaction logic for BattleWindow.xaml
    /// </summary>
    public partial class BattleWindow : Window
    {

        int skill_select;
        string attack_char = "0";
        BattleController bat = new BattleController();
        private Timer timer;
        private int timerzim = 0;
        private string type_skill;

        public BattleWindow(string character1, string character2, string character3)
        {
            InitializeComponent();

            if (bat.initial_turn()== true) //ia
            {
                ia_play();
            }
            else if ( bat.initial_turn() == false) //player
            {
                Timer_Function();
            }
            
            //Load character
            Character1_red.Source = Load_image(character1);
            Character2_red.Source = Load_image(character2);
            Character3_red.Source = Load_image(character3);

            //skills
            Character1_red_skill1.Source = Load_skill(character1, 1);
            Character1_red_skill2.Source = Load_skill(character1, 2);
            Character1_red_skill3.Source = Load_skill(character1, 3);
                                           
            Character2_red_skill1.Source = Load_skill(character2, 1);
            Character2_red_skill2.Source = Load_skill(character2, 2);
            Character2_red_skill3.Source = Load_skill(character2, 3);
                                           
            Character3_red_skill1.Source = Load_skill(character3, 1);
            Character3_red_skill2.Source = Load_skill(character3, 2);
            Character3_red_skill3.Source = Load_skill(character3, 3);

            bat.Character1_red = character1;
            bat.Character2_red = character2;
            bat.Character3_red = character3;

            //Chakra
            Load_Chakras();
        }

        //=====================Loaders=====================


        private ImageSource Load_image(string character)
        {
            return new BitmapImage(new Uri("Characters/" + character + "/" + character + "_default.png", UriKind.Relative));
        }

        private ImageSource Load_skill(string character, int number)
        {
            return new BitmapImage(new Uri("Characters/" + character + "/" + character + "_skill" + number + "_default.png", UriKind.Relative));
        }

        //load red chakra
        private void Load_Chakras()
        {
            List<int> Chakras = new List<int>();
            Chakras = bat.Return_Chakras(1);

            TaijutsuNumber.Content = Chakras[0];
            BloodlineNumber.Content = Chakras[1];
            NinjutsuNumber.Content = Chakras[2];
            GenjutsuNumber.Content = Chakras[3];
        }

        //===================== Turno =====================

        //Pass turn button
        private void Pass_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            pass_turn();
            ia_play();
        }

        //Alterar numeração da label de turno
        private void pass_turn()
        {
            Turn.Content = bat.pass_turn(Turn.Content);
            skill_select = 0;
            bat.ChakraRed.turnChakra();
            Load_Chakras();
            pb.Value = 0;
            try
            {
                timer.Start();
            }
            catch
            {
                Timer_Function();
            }
        }

        private void Timer_Function()
        {
            timer = new Timer(1000);
            timer.Elapsed += new ElapsedEventHandler(Timer_Elapsed);
            timer.Start();
        }

        void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            this.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, (Action)(() =>
            {
                if (pb.Value < 30)
                {
                    pb.Value += 1;
                }
                else
                {
                    timer.Stop();
                    pass_turn();
                    ia_play();
                }
            }));
        }

        //===================== Attack ====================

        //Receive number skill
        private void ReceiveSkillNumber(object sender, MouseButtonEventArgs e)
        {
            Image image = (Image)sender;

            //Numbers
            string skillNumber = bat.convert_name_int(image.Name).ToString();
            string skill_second = (skillNumber.Last()).ToString();
            int char_select = int.Parse(skillNumber.Remove(skillNumber.Length - 1));
            skill_select = int.Parse(skill_second);

            //skills chakra
            List<string> Skills = new List<string>();
            Skills = bat.Skills(skill_second, char_select);
            bool haveChakra = bat.Have_Chakra(Skills);

            if (haveChakra == true)
            {
                //retira o chakra
                bat.Withdraw_Chakra(Skills);

                //Change chakras content
                Load_Chakras();

                //pass character to attack char
                attack_char = bat.Attack_Char(char_select);

                //search skill type
                type_skill = bat.Skill_Type(char_select, skill_select);
            }
   
        }

        //Select enemy to atk
        private void Select_Enemy(object sender, MouseButtonEventArgs e)
        {
            if(attack_char != "" && type_skill == "attack")
            {
                Image image = (Image)sender;
                int characterNumber = bat.attack_choose(skill_select, Character1_blue_life.Content, Character2_blue_life.Content, Character3_blue_life.Content, image.Name);

                if (characterNumber == 1)
                {
                    Character1_blue_life.Content = bat.attack_red(Character1_blue_life.Content, skill_select, attack_char, type_skill);
                    Dead(sender, Character1_blue_life);
                    teamDead();
                }
                else if (characterNumber == 2)
                {
                    Character2_blue_life.Content = bat.attack_red(Character2_blue_life.Content, skill_select, attack_char, type_skill);
                    Dead(sender, Character2_blue_life);
                    teamDead();
                }
                else if (characterNumber == 3)
                {
                    Character3_blue_life.Content = bat.attack_red(Character3_blue_life.Content, skill_select, attack_char, type_skill);
                    Dead(sender, Character3_blue_life);
                    teamDead();
                }

                attack_char = "";
            }    
        }
        
        //Select friendly to heal
        private void Select_Friend(object sender, MouseButtonEventArgs e)
        {
            if (attack_char != "" && type_skill == "heal")
            {
                Image image = (Image)sender;
                int characterNumber = bat.attack_choose(skill_select, Character1_red_life.Content, Character2_red_life.Content, Character3_red_life.Content, image.Name);

                if (characterNumber == 1)
                {
                    Character1_red_life.Content = bat.attack_red(Character1_red_life.Content, skill_select, attack_char, type_skill);
                }
                else if (characterNumber == 2)
                {
                    Character2_red_life.Content = bat.attack_red(Character2_red_life.Content, skill_select, attack_char, type_skill);

                }
                else if (characterNumber == 3)
                {
                    Character3_red_life.Content = bat.attack_red(Character3_red_life.Content, skill_select, attack_char, type_skill);

                }

                attack_char = "";
            }
        }

        //===================== Death ====================

        private void teamDead()
        {
            int dead = bat.confirmation_teamDead(Character1_blue_life.Content, Character2_blue_life.Content, Character3_blue_life.Content,
                                                 Character1_red_life.Content, Character2_red_life.Content, Character3_red_life.Content);
            string result = "";
            if (dead == 2)
            {
                result = "Ganhou";
                MessageBox.Show("Você " + result);
                Close();
            }
            else if (dead == 1)
            {
                result = "Perdeu";
                MessageBox.Show("Você " + result);
                Close();
            }

        }

        private void Dead(object nome_e_cor, object life)
        {  
            Image image = (Image)nome_e_cor;
            Label label = (Label)life;

            bool confimation = bat.dead_confirmation(label.Content);
            if (confimation == true)
            {
                image.Source = new BitmapImage(new Uri("Others/dead_default.png", UriKind.Relative));
                image.Tag = "Others/dead_default.png";
                image.IsEnabled = false;
                label.Content = "0";
            }
        }

        //===================== Others ====================

        private void Generic_mouseEnter(object sender, MouseEventArgs e)
        {
            Image image = (Image)sender;
            string source = image.Tag.ToString();
            source = source.Remove(source.Length - 4);

           if (image.Tag.ToString() == "Others/dead_default.png")
           {

           }
           else if (skill_select != 0 && attack_char != "")
           {
               image.Source = new BitmapImage(new Uri(@source + "_select.png", UriKind.Relative));
           }
        }

        private void Generic_mouseLeave(object sender, MouseEventArgs e)
        {
            Image image = (Image)sender;
            string source = image.Tag.ToString();
            source = source.Remove(source.Length - 4);

            if (image.Tag.ToString() == "Others/dead_default.png")
            {

            }
            else if (skill_select != 0)
            {
                image.Source = new BitmapImage(new Uri(@source + ".png", UriKind.Relative));
            }
        }


        private void ShowSkill_MouseEnter(object sender, MouseEventArgs e)
        {
            //change image
            Image image = (Image)sender;
            panelSkillImage.Source = image.Source;

            //change chakras number
            string skillNumber = bat.convert_name_int(image.Name).ToString();
            string skill_second = (skillNumber.Last()).ToString();
            int char_select = int.Parse(skillNumber.Remove(skillNumber.Length - 1));
            skill_select = int.Parse(skillNumber.Substring(0, 1));

            List<string> skills = new List<string>();
            skills = bat.Skills(skill_second, char_select);

            TaijutsuNumber_Panel.Content = skills[0];
            BloodlineNumber_Panel.Content = skills[1];
            NinjutsuNumber_Panel.Content = skills[2];
            GenjutsuNumber_Panel.Content = skills[3];
        }

        //===================== IA ====================

        //Jogada da IA
        private void ia_play()
        {
            Random rand = new Random();
            int characterNumber = rand.Next(1, 4);
            //MessageBox.Show("" + characterNumber);

            if (characterNumber == 1)
            {
                Character1_red_life.Content = bat.attack_blue(Character1_red_life.Content);
                Dead(Character1_red, Character1_red_life);
                teamDead();
            }
            else if (characterNumber == 2)
            {
                Character2_red_life.Content = bat.attack_blue(Character2_red_life.Content);
                Dead(Character2_red, Character2_red_life);
                teamDead();
            }
            else if (characterNumber == 3)
            {
                Character3_red_life.Content = bat.attack_blue(Character3_red_life.Content);
                Dead(Character3_red, Character3_red_life);
                teamDead();
            }

            pass_turn();
        }

    }
}
