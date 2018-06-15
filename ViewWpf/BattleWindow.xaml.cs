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
            int skillNumber = bat.convert_name_int(Character1_red_skill2.Name);
            skill_select = skillNumber;
        }

        //Select enemy to atk
        private void Select_Enemy(object sender, MouseButtonEventArgs e)
        {
            Image image = (Image)sender;
            int characterNumber = bat.attack_choose(skill_select, Character1_blue_life.Content, Character2_blue_life.Content, Character2_blue_life.Content, image.Name);

            if (characterNumber == 1)
            {
                Character1_blue_life.Content = bat.attack_red(Character1_blue_life.Content);
            }
            else if (characterNumber == 2)
            {
                Character2_blue_life.Content = bat.attack_red(Character2_blue_life.Content);
            }
            else if (characterNumber == 3)
            {
                Character3_blue_life.Content = bat.attack_red(Character3_blue_life.Content);
            }
        }

        //=====================Others====================

        //Jogada da IA
        private void ia_play()
        {
            Random rand = new Random();
            int characterNumber = rand.Next(1, 4);
            //MessageBox.Show("" + characterNumber);

            if (characterNumber == 1)
            {
                Character1_red_life.Content = bat.attack_blue(Character1_red_life.Content);
            }
            else if (characterNumber == 2)
            {
                Character2_red_life.Content = bat.attack_blue(Character2_red_life.Content);
            }
            else if (characterNumber == 3)
            {
                Character3_red_life.Content = bat.attack_blue(Character3_red_life.Content);
            }

            pass_turn();
        }


        private void Generic_mouseEnter(object sender, MouseEventArgs e)
        {
            Image image = (Image)sender;
            string source = image.Tag.ToString();

            source = source.Remove(source.Length - 4);

            if (bat.skill_select(skill_select) == true)
            {
                image.Source = new BitmapImage(new Uri(@source + "_select.png", UriKind.Relative));
            }
        }

        private void Generic_mouseLeave(object sender, MouseEventArgs e)
        {
            Image image = (Image)sender;
            string source = image.Tag.ToString();

            source = source.Remove(source.Length - 4);
            if (bat.skill_select(skill_select) == true)
            {
                image.Source = new BitmapImage(new Uri(@source + ".png", UriKind.Relative));
            }
        }
    }
}
