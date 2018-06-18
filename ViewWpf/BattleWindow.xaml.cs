using Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        static int skill_select;
        string attack_char;
        BattleController bat = new BattleController();

        public BattleWindow(string character1, string character2, string character3)
        {
            InitializeComponent();

            if (bat.initial_turn()== true)
            {
                ia_play();
            }
            
            //Load character
            Character1_red.Source = load_image(character1);
            Character2_red.Source = load_image(character2);
            Character3_red.Source = load_image(character3);

            //skills
            Character1_red_skill1.Source = load_skill(character1, 1);
            Character1_red_skill2.Source = load_skill(character1, 2);
            Character1_red_skill3.Source = load_skill(character1, 3);

            Character2_red_skill1.Source = load_skill(character2, 1);
            Character2_red_skill2.Source = load_skill(character2, 2);
            Character2_red_skill3.Source = load_skill(character2, 3);

            Character3_red_skill1.Source = load_skill(character3, 1);
            Character3_red_skill2.Source = load_skill(character3, 2);
            Character3_red_skill3.Source = load_skill(character3, 3);

            bat.Character1_red = character1;
            bat.Character2_red = character2;
            bat.Character3_red = character3;
        }

        private ImageSource load_image(string character)
        {
            return new BitmapImage(new Uri("Characters/" + character + "/" + character + "_default.png", UriKind.Relative));
        }

        private ImageSource load_skill(string character, int number)
        {
            return new BitmapImage(new Uri("Characters/" + character + "/" + character + "_skill" + number + "_default.png", UriKind.Relative));
        }

        //MessageBox.Show("Player jogada:" + bat.printturno());


        //=====================Turno=====================

        //Pass turn button
        private void Pass_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            pass_turn();
            ia_play();
            //MessageBox.Show("test: " +  bat.test());
        }

        //Alterar numeração da label de turno
        private void pass_turn()
        {
            Turn.Content = bat.pass_turn(Turn.Content);
            skill_select = 0;
        }

        //=====================Attack====================

        //Receiv number skill
        private void ReceiveSkillNumber(object sender, MouseButtonEventArgs e)
        {
            Image image = (Image)sender;

            string skillNumber = bat.convert_name_int(image.Name).ToString();
            skill_select = int.Parse(skillNumber.Substring(0, 1));

            string charNumberString = skillNumber.Remove(skillNumber.Length - 1);
            int charNumber = int.Parse(charNumberString);

            if (charNumber == 1)
            {
                attack_char = bat.Character1_red;
            }
            else if (charNumber == 2)
            {
                attack_char = bat.Character2_red;
            }
            else if (charNumber == 3)
            {
                attack_char = bat.Character3_red;
            }
   
        }

        //Select enemy to atk
        private void Select_Enemy(object sender, MouseButtonEventArgs e)
        {
            Image image = (Image)sender;
            int characterNumber = bat.attack_choose(skill_select, Character1_blue_life.Content, Character2_blue_life.Content, Character2_blue_life.Content, image.Name);
            

            if (characterNumber == 1)
            {
                Character1_blue_life.Content = bat.attack_red(Character1_blue_life.Content, skill_select, attack_char);
                dead(sender, Character1_blue_life.Content);
                teamDead();
            }
            else if (characterNumber == 2)
            {
                Character2_blue_life.Content = bat.attack_red(Character2_blue_life.Content, skill_select, attack_char);
                dead(sender, Character2_blue_life.Content);
                teamDead();
            }
            else if (characterNumber == 3)
            {
                Character3_blue_life.Content = bat.attack_red(Character3_blue_life.Content, skill_select, attack_char);
                dead(sender, Character3_blue_life.Content);
                teamDead();
            }
        }

        //=====================Others====================
        private void teamDead()
        {
            int dead = confirmation_teamDead();
            string d = "";
            if (dead == 2)
            {
                d = "Ganhou";
                MessageBox.Show("Você " + d);
                Close();
            }
            else if (dead == 1)
            {
                d = "Perdeu";
                MessageBox.Show("Você " + d);
                Close();
            }

        }

        private int confirmation_teamDead()
        {
            if (bat.conversion_object_toint(Character1_blue_life.Content) <= 0 &&
                bat.conversion_object_toint(Character2_blue_life.Content) <= 0 &&
                bat.conversion_object_toint(Character3_blue_life.Content) <= 0)
            {
                return 2;
            }

            if (bat.conversion_object_toint(Character1_red_life.Content) <= 0 &&
                bat.conversion_object_toint(Character2_red_life.Content) <= 0 &&
                bat.conversion_object_toint(Character3_red_life.Content) <= 0)
            {
                return 1;
            }

            return 0;
        }

        //Dead
        private void dead(object nome_e_cor, object life)
        {
            Image image = (Image)nome_e_cor;

            //string name = image.Name.ToString();
            bool confimation = bat.dead_confirmation(life);
            if (confimation == true)
            {
                image.Source = new BitmapImage(new Uri("Others/dead_default.png", UriKind.Relative));
                image.Tag = "Others/dead_default.png";
            }
            else
            {

            }
        }

        //Jogada da IA
        private void ia_play()
        {
            Random rand = new Random();
            int characterNumber = rand.Next(1, 4);
            //MessageBox.Show("" + characterNumber);

            if (characterNumber == 1)
            {
                Character1_red_life.Content = bat.attack_blue(Character1_red_life.Content);
                dead(Character1_red, Character1_red_life.Content);
                teamDead();
            }
            else if (characterNumber == 2)
            {
                Character2_red_life.Content = bat.attack_blue(Character2_red_life.Content);
                dead(Character2_red, Character2_red_life.Content);
                teamDead();
            }
            else if (characterNumber == 3)
            {
                Character3_red_life.Content = bat.attack_blue(Character3_red_life.Content);
                dead(Character3_red, Character3_red_life.Content);
                teamDead();
            }

            pass_turn();
        }


        private void Generic_mouseEnter(object sender, MouseEventArgs e)
        {
            Image image = (Image)sender;
            string source = image.Tag.ToString();

            source = source.Remove(source.Length - 4);

            if (image.Tag == "Others/dead_default.png")
            {

            }
            else if (bat.skill_select(skill_select) == true)
            {
                image.Source = new BitmapImage(new Uri(@source + "_select.png", UriKind.Relative));
            }
        }

        private void Generic_mouseLeave(object sender, MouseEventArgs e)
        {
            Image image = (Image)sender;
            string source = image.Tag.ToString();

            source = source.Remove(source.Length - 4);

            if (image.Tag == "Others/dead_default.png")
            {

            }
            else if (bat.skill_select(skill_select) == true)
            {
                image.Source = new BitmapImage(new Uri(@source + ".png", UriKind.Relative));
            }
        }
    }
}
