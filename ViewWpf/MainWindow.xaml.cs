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
        Tools tools = new Tools();
        List<string> CharacterList = new List<string>();
        private string Char1 = "";
        private string Char2 = "";
        private string Char3 = "";
        private int atualChanged = 0;
        private int charLoad = 0;
        public bool loginCheck { get; set; }
        private string account = "";
        private bool searchCheck = false;
        MenuAdminWindow menuAdmin;

        public MainWindow()
        {
            InitializeComponent();
            CharacterList = tools.return_Characters();
        }

        //=====================Character====================

        private void Character_Select(object sender, MouseButtonEventArgs e)
        {
            if (loginCheck == false)
            {
                MessageBox.Show("Login to select a character");
            }
            if (e.ClickCount == 2 && loginCheck == true)
            {
                //MessageBox.Show("Double");
                Image image = (Image)sender;
                atualChanged += 1;
                if (atualChanged == 1)
                {
                    Char1 = image.Tag.ToString();
                    CharacterSelect1.Source = new BitmapImage(new Uri("Characters/" + Char1 + "/" + Char1 + "_default.png", UriKind.Relative));
                    CharacterSelect1.Tag = Char1;
                }
                else if (atualChanged == 2)
                {
                    Char2 = image.Tag.ToString();
                    CharacterSelect2.Source = new BitmapImage(new Uri("Characters/" + Char2 + "/" + Char2 + "_default.png", UriKind.Relative));
                    CharacterSelect2.Tag = Char2;
                }
                else if (atualChanged == 3)
                {
                    Char3 = image.Tag.ToString();
                    CharacterSelect3.Source = new BitmapImage(new Uri("Characters/" + Char3 + "/" + Char3 + "_default.png", UriKind.Relative));
                    CharacterSelect3.Tag = Char3;
                    atualChanged = 0;
                }
            }
        }

        private void Character_Load(object sender, RoutedEventArgs e)
        {
            Image Character = (Image)sender;
            if (CharacterList.Count > charLoad)
            {
                Character.Source = new BitmapImage(new Uri("Characters/" + CharacterList[charLoad] + "/" + CharacterList[charLoad] + "_default.png", UriKind.Relative));
                Character.Tag = CharacterList[charLoad];
                charLoad += 1;
            }
            else
            {
                Character.Source = new BitmapImage(new Uri("Others/invalid_default.png", UriKind.Relative));
            }
        }

        //=====================Account====================

        private void Create_Account(object sender, RoutedEventArgs e)
        {
            string name = RegisLogin.Text;
            string pass = RegisPass.Password;
            string passCon = RegisPassCon.Password;

            if (pass == passCon)
            {
                if (tools.registerAccount(name, pass) == true)
                {
                    MessageBox.Show("Account created");
                    Grid_Visibility(1);
                }
                else
                {
                    MessageBox.Show("Change login");
                }
            }
            else if (pass != passCon)
            {
                MessageBox.Show("Password dont match");
            }
        }

        private void Login_Account(object sender, RoutedEventArgs e)
        {
            string login = LoginText.Text;
            string pass = PassText.Password;
            if (login == "" || pass == "")
            {
                MessageBox.Show("Login or Password incorrect");
            }
            else
            {
                bool loginAut = tools.loginAuthentication(login, pass);
                if (loginAut == true)
                {
                    Grid_Visibility(sender, e);
                    StartButton.IsEnabled = true;
                    loginCheck = true;
                    account = LoginText.Text;
                    Account_Status();
                    Loggout.Visibility = Visibility.Visible;
                }
                else if (loginAut == false)
                {
                    MessageBox.Show("Login or Password incorrect");
                }
            }
        }

        private void Account_Status()
        {
            try
            {
                List<string> status = new List<string>();
                status = tools.Account_Status(account);
                victoriesValue.Content = status[0];
                losesValue.Content = status[1];
                float kd = (float.Parse(status[0]) / float.Parse(status[1]));
                try
                {
                    wlValue.Content = kd.ToString().Substring(0, 4);
                }
                catch
                {
                    wlValue.Content = kd.ToString();
                }
            }
            catch
            {
                wlValue.Content = "error";
                victoriesValue.Content = "error";
                losesValue.Content = "error";
            }
            
        }

        private void Loggout_Account_Function()
        {
            if (loginCheck == true)
            {
                loginCheck = false;
                account = "";
                wlValue.Content = "0";
                victoriesValue.Content = "0";
                losesValue.Content = "0";
                StartButton.IsEnabled = false;
                Loggout.Visibility = Visibility.Hidden;
                Grid_Visibility(4);
            }
        }

        private void Loggout_Account(object sender, RoutedEventArgs e)
        {
            Loggout_Account_Function();
        }

        private void Reset_Account(object sender, RoutedEventArgs e)
        {
            if (loginCheck == true)
            {
                bool reset = tools.Reset_Account(account);
                if (reset == true)
                {
                    MessageBox.Show("Account status reset :)");
                    Account_Status();
                }
                else if (reset == false)
                {
                    MessageBox.Show("Error to reset");
                }
            }
        }

        private void Delete_Account(object sender, RoutedEventArgs e)
        {
            if (loginCheck == true)
            {
                bool reset = tools.Delete_Account(account);
                if (reset == true)
                {
                    Loggout_Account_Function();
                    MessageBox.Show("Account deleted");
                }
                else if (reset == false)
                {
                    MessageBox.Show("Error to delete");
                }
            }
        }

        private void Change_Password(object sender, RoutedEventArgs e)
        {
            if (loginCheck == true && (NewPassText.Password != null && NewPassText.Password != ""))
            {
                bool resetPass = tools.Reset_Pass_Account(this.account, NewPassText.Password);
                if (resetPass == true)
                {
                    MessageBox.Show("Password reset");
                    WelcomeMessage.Visibility = Visibility.Visible;
                    NewPass.Visibility = Visibility.Hidden;
                    NewPassText.Visibility = Visibility.Hidden;
                    NewPassLabel.Visibility = Visibility.Hidden;
                    NewPassReturn.Visibility = Visibility.Hidden;
                }
                else if (resetPass == false)
                {
                    MessageBox.Show("Error to reset");
                }
            }
            else
            {
                MessageBox.Show("Password is null");
            }
        }

        private void Change_Password_Show(object sender, RoutedEventArgs e)
        {
            if (loginCheck == true)
            {
                WelcomeMessage.Visibility = Visibility.Hidden;
                NewPass.Visibility = Visibility.Visible;
                NewPassText.Visibility = Visibility.Visible;
                NewPassLabel.Visibility = Visibility.Visible;
                NewPassReturn.Visibility = Visibility.Visible;
            }
        }

        private void Change_Password_Close(object sender, RoutedEventArgs e)
        {
            if (loginCheck == true)
            {
                WelcomeMessage.Visibility = Visibility.Visible;
                NewPass.Visibility = Visibility.Hidden;
                NewPassText.Visibility = Visibility.Hidden;
                NewPassLabel.Visibility = Visibility.Hidden;
                NewPassReturn.Visibility = Visibility.Hidden;
            }
        }

        //=====================Others====================

        private void Start_Battle(object sender, RoutedEventArgs e)
        {
            //precisa estar logado

            if (CharacterSelect1.Tag.ToString() == "" || CharacterSelect2.Tag.ToString() == "" || CharacterSelect3.Tag.ToString() == "")
            {
                MessageBox.Show("Select 3 characters");
            }
            else
            {
                string char1 = CharacterSelect1.Tag.ToString();
                string char2 = CharacterSelect2.Tag.ToString();
                string char3 = CharacterSelect3.Tag.ToString();
                BattleWindow btWin = new BattleWindow(char1, char2, char3, account);
                btWin.ShowDialog();
                searchCheck = false;
            }

        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            try
            {
                menuAdmin.Close();
                this.Close();
            }
            catch
            {
                this.Close();
            }
            

            
        }

        private void Grid_Visibility(int grid /*1 login, 2 regis*/)
        {
            if (grid == 1)
            {
                GridLogin.Visibility = Visibility.Visible;
                GridRegis.Visibility = Visibility.Hidden;
            }
            if (grid == 2)
            {
                GridLogin.Visibility = Visibility.Hidden;
                GridRegis.Visibility = Visibility.Visible;
            }
            if (grid == 3)
            {
                SubGridLogin.Visibility = Visibility.Hidden;
                WelcomeMessage.Visibility = Visibility.Visible;
                AccountOptions.Visibility = Visibility.Visible;
            }
            if (grid == 4)
            {
                SubGridLogin.Visibility = Visibility.Visible;
                WelcomeMessage.Visibility = Visibility.Hidden;
                AccountOptions.Visibility = Visibility.Hidden;
            }

        }

        private void Grid_Visibility(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            string btnContent = btn.Name;

            if (btnContent == "Return")
            {
                Grid_Visibility(1);
            }
            if (btnContent == "RegisterShow")
            {
                Grid_Visibility(2);
            }
            if (btnContent == "Login")
            {
                Grid_Visibility(3);
            }
            if (btnContent == "Loggout")
            {
                Grid_Visibility(4);
            }
        }

        private void Random_Characters(object sender, RoutedEventArgs e)
        {
            int charNumber = CharacterList.Count();
            Random random = new Random();
            int randomNum1;
            int randomNum2;
            int randomNum3;

            do
            {
                randomNum1 = random.Next(0, charNumber);
                randomNum2 = random.Next(0, charNumber);
                randomNum3 = random.Next(0, charNumber);

            } while (randomNum1 == randomNum2 || randomNum1 == randomNum3 || randomNum2 == randomNum3);

            CharacterSelect1.Source = new BitmapImage(new Uri("Characters/" + CharacterList[randomNum1] + "/" + CharacterList[randomNum1] + "_default.png", UriKind.Relative));
            CharacterSelect1.Tag = CharacterList[randomNum1];

            CharacterSelect2.Source = new BitmapImage(new Uri("Characters/" + CharacterList[randomNum2] + "/" + CharacterList[randomNum2] + "_default.png", UriKind.Relative));
            CharacterSelect2.Tag = CharacterList[randomNum2];

            CharacterSelect3.Source = new BitmapImage(new Uri("Characters/" + CharacterList[randomNum3] + "/" + CharacterList[randomNum3] + "_default.png", UriKind.Relative));
            CharacterSelect3.Tag = CharacterList[randomNum3];
        }

        private void Grid_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F5)
            {
                menuAdmin = new MenuAdminWindow();
                menuAdmin.Show();
            }
        }


        private void Grid_MouseMove(object sender, MouseEventArgs e)
        {
            if (searchCheck != true)
            {
                if (loginCheck == true)
                {
                    Account_Status();
                    searchCheck = true;
                }
            }
        }



        //private void Character1_MouseEnter(object sender, MouseEventArgs e)
        //{
        //    Character1.Height = 97;
        //    Character1.Width = 97;
        //}

        //private void Character1_MouseLeave(object sender, MouseEventArgs e)
        //{
        //    Character1.Height = 87;
        //    Character1.Width = 87;
        //}
    }
}
