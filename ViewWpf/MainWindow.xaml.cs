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
            }
            else
            {
                image.Source = new BitmapImage(new Uri("Characters/Sarada/Sarada_default_select.png", UriKind.Relative));
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
    }
}
