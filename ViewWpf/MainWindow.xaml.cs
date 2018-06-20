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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Controllers;
using Controllers.DAL;

namespace ViewWpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Context context = new Context();
        List<string> charss = new List<string>();
        String Char1 = "";
        String Char2 = "";
        String Char3 = "";
        int atualChanged = 0;

        public MainWindow()
        {
            InitializeComponent();
            charss = context.return_Characters();

            //Load characters
            addChar(Character1, 0);
            addChar(Character2, 1);
            addChar(Character3, 2);
            addChar(Character4, 3);
            addChar(Character5, 4);
            addChar(Character6, 5);
            addChar(Character7, 6);
            addChar(Character8, 7);
            addChar(Character9, 8);
            addChar(Character10, 9);
            addChar(Character11, 10);
            addChar(Character12, 11);
        }

        //add uri
        public void addChar(Image image, int number)
        { 
            
            if (charss.Count > number)
            {
                image.Source = new BitmapImage(new Uri("Characters/" + charss[number] + "/" + charss[number] + "_default.png", UriKind.Relative));
                image.Tag = charss[number];
            }
            else
            {
                image.Source = new BitmapImage(new Uri("Others/invalid_default.png", UriKind.Relative));
            }
            
        }

        public void addSelectChar()
        {
            if (atualChanged == 1)
            {
                CharacterSelect1.Source = new BitmapImage(new Uri("Characters/" + Char1 + "/" + Char1 + "_default.png", UriKind.Relative));
                CharacterSelect1.Tag = Char1;
            }
            if (atualChanged == 2)
            {
                CharacterSelect2.Source = new BitmapImage(new Uri("Characters/" + Char2 + "/" + Char2 + "_default.png", UriKind.Relative));
                CharacterSelect2.Tag = Char2;
            }
            if (atualChanged == 3)
            {
                CharacterSelect3.Source = new BitmapImage(new Uri("Characters/" + Char3 + "/" + Char3 + "_default.png", UriKind.Relative));
                CharacterSelect3.Tag = Char3;
            }
                
        }   
         
        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            string char1 = CharacterSelect1.Tag.ToString();
            string char2 = CharacterSelect2.Tag.ToString();
            string char3 = CharacterSelect3.Tag.ToString();

            BattleWindow btWin = new BattleWindow(char1, char2, char3);
            btWin.ShowDialog();
        }

        private void Character_Select(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                //MessageBox.Show("Double");
                Image image = (Image)sender;
                atualChanged += 1;
                if (atualChanged == 1)
                {
                    Char1 = image.Tag.ToString();
                    addSelectChar();
                }
                else if (atualChanged == 2)
                {
                    Char2 = image.Tag.ToString();
                    addSelectChar();
                }
                else if (atualChanged == 3)
                {
                    Char3 = image.Tag.ToString();
                    addSelectChar();
                    atualChanged = 0;
                }
                

            }
        }

    }
}
