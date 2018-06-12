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

        public BattleWindow()
        {
            InitializeComponent();

            if (bat.initial_turn()== true)
            {
                ia_play();
            }
            
            //MessageBox.Show("Player jogada:" + bat.printturno());
        }


        private void Character1_red_skill1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //personagem selecionado

            Character1_blue_life.Content = bat.attack_red(Character1_blue_life.Content);
            if (bat.dead_confirmation(Character1_blue_life.Content) == true)
            {
                MessageBox.Show("persongem / time blue morreu");
                //muda img para morto
            }
        }

        private void Pass_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            pass_turn();
            ia_play();
        }

        //Jogada da IA
        private void ia_play()
        {
            Character1_red_life.Content = bat.attack_blue(Character1_red_life.Content);
            pass_turn();
        }

        //Alterar numeração da label de turno
        private void pass_turn()
        {
            Turn.Content = bat.pass_turn(Turn.Content);
            skill_select = 0;
        }

        private void ReceiveSkillNumber(object sender, MouseButtonEventArgs e)
        {
            int skillNumber = bat.convert_name_int(Character1_red_skill2.Name);
            //MessageBox.Show("Result: " + skillNumber);
            skill_select = skillNumber;
        }

        private void Generic_mouseEnter(object sender, MouseEventArgs e)
        {
            Image image = (Image)sender;
            string source = image.Tag.ToString();
            
            source = source.Remove(source.Length - 4);
            //MessageBox.Show("Test:" + source);

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
            //MessageBox.Show("Test:" + source);
            if (bat.skill_select(skill_select) == true)
            {
                image.Source = new BitmapImage(new Uri(@source + ".png", UriKind.Relative));
            }
        }


        private void Select_Enemy(object sender, MouseButtonEventArgs e)
        {
            if (skill_select != 0)
            {
                //Confere o turno de quem ta atacando
                int turn_play = bat.printturno();
                //manda skill com o numero dela
                if (turn_play == 1)
                {
                    Image image = (Image)sender;
                    if (bat.convert_name_int(image.Name) == 1)
                    {
                        Character1_blue_life.Content = bat.attack_red(Character1_blue_life.Content);
                    }

                    //life.Content = bat.attack_red(life.Content);
                }
                else
                {

                }
                // Character1_blue_life.Content = bat.attack_red(Character1_blue_life.Content);
            }
        }
    }
}
