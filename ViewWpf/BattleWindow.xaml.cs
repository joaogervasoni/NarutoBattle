using Controllers;
using System;
using System.Collections.Generic;
using System.IO;
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
        BattleController Btc = new BattleController();
        private Timer Tmr;

        public BattleWindow(string Char1, string Char2, string Char3, string Account)
        {
            InitializeComponent();
            Btc.Account = Account;
            Btc.Set_Characters_Red(Char1, Char2, Char3);
            Btc.Set_Characters_Blue();
            Load_Chakras();
            Btc.Initial_Turn();

            //Initialize Turn
            if (Btc.Return_Turn_Play() == "IA")
            {
                Ia_Play();
            }
            else if (Btc.Return_Turn_Play() == "Player")
            {
                Timer_Function();
            }

            //Load character
            Character1_red.Source = Load_image(Char1);
            Character2_red.Source = Load_image(Char2);
            Character3_red.Source = Load_image(Char3);

            Character1_blue.Source = Load_image(Btc.Character1_blue);
            Character2_blue.Source = Load_image(Btc.Character2_blue);
            Character3_blue.Source = Load_image(Btc.Character3_blue);

            Character1_red.Tag = "Characters/" + Char1 + "/" + Char1 + "_default.png";
            Character2_red.Tag = "Characters/" + Char2 + "/" + Char2 + "_default.png";
            Character3_red.Tag = "Characters/" + Char3 + "/" + Char3 + "_default.png";

            Character1_blue.Tag = "Characters/" + Btc.Character1_blue + "/" + Btc.Character1_blue + "_default.png";
            Character2_blue.Tag = "Characters/" + Btc.Character2_blue + "/" + Btc.Character2_blue + "_default.png";
            Character3_blue.Tag = "Characters/" + Btc.Character3_blue + "/" + Btc.Character3_blue + "_default.png";

            //skills
            Character1_red_skill1.Source = Load_skill(Char1, 1);
            Character1_red_skill2.Source = Load_skill(Char1, 2);
            Character1_red_skill3.Source = Load_skill(Char1, 3);
                                                      
            Character2_red_skill1.Source = Load_skill(Char2, 1);
            Character2_red_skill2.Source = Load_skill(Char2, 2);
            Character2_red_skill3.Source = Load_skill(Char2, 3);
                                                      
            Character3_red_skill1.Source = Load_skill(Char3, 1);
            Character3_red_skill2.Source = Load_skill(Char3, 2);
            Character3_red_skill3.Source = Load_skill(Char3, 3);
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

        private void Load_Chakras()
        {
            List<int> Chakras = new List<int>();
            Chakras = Btc.Return_Chakras(1);

            TaijutsuNumber.Content = Chakras[0];
            BloodlineNumber.Content = Chakras[1];
            NinjutsuNumber.Content = Chakras[2];
            GenjutsuNumber.Content = Chakras[3];
        }

        private void Change_Cursor(string Mode)
        {
            if (Mode == "Attack")
            {
                var info = Application.GetResourceStream(new Uri("Cursor/Skill_Select.ani", UriKind.Relative));
                var cursor = new Cursor(info.Stream);

                principal.Cursor = cursor;
            }

            if (Mode == "Normal")
            {
                var info = Application.GetResourceStream(new Uri("Cursor/Normal_Select.cur", UriKind.Relative));
                var cursor = new Cursor(info.Stream);

                principal.Cursor = cursor;
            }

        }

        //===================== Turno =====================

        private void Timer_Function()
        {
            Tmr = new Timer(1000);
            Tmr.Elapsed += new ElapsedEventHandler(Timer_Elapsed);
            Tmr.Start();
        }

        void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, (Action)(() =>
            {
                if (BarTime.Value < 30)
                {
                    BarTime.Value += 1;
                }
                else
                {
                    Tmr.Stop();
                    Pass_Turn();
                    Ia_Play();
                }
            }));
        }

        void Timer_Pass()
        {
            BarTime.Value = 0;
            try
            { Tmr.Start(); }
            catch
            { Timer_Function(); }
        }

        private void Pass_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Pass_Turn();
            Ia_Play();
        }

        private void Pass_Turn()
        {
            Btc.AttackChar = null;
            Btc.SkillSelect = 0;
            Turn.Content = Btc.Pass_Turn();
            Btc.ChakraRed.turnChakra();
            Load_Chakras();
            Unlock_Character();
            Receive_SkillsDH();
            Timer_Pass();
            Change_Life_GlobalColor();
        }

        //===================== Attack ====================

        private void Receive_Skill_Number(object sender, MouseButtonEventArgs e)
        {
            Image SkillImage = (Image)sender;

            if (Btc.Receive_Skill_Number(SkillImage.Name.ToString(), 1) == true)
            {
                Load_Chakras();
                Change_Cursor("Attack");
            }
            else
            {}
        }

        private void Cancel_Skill(object sender, MouseButtonEventArgs e)
        {
            if (Btc.AttackChar != null && Btc.SkillSelect != 0)
            {
                Btc.Cancel_Attack();
                Change_Cursor("Normal");
                Load_Chakras();
            }
        }

        private void Unlock_Character()
        {
            var Images = principal.Children.OfType<Image>();
            foreach (Image element in Images)
            {
                if (element.Name != null && element.Name != "" && element.Name.Length > 15 && element.Tag.ToString() != "dead" )
                {
                    try
                    {
                        if (element.IsEnabled == false)
                        {

                            element.Opacity = 1;
                            element.IsEnabled = true;
                        }
                    }
                    catch
                    {}
                }
            }
        }

        private void Receive_SkillsDH()
        {
            var SkillAddedSA = SkillAdded.Children.OfType<Image>();
            int Defense = 0;
            int Turn = 0;
            int autenti = 1;
            string SkillString = null;
            int SkillAddedNumber = 0;

            foreach (Image element in SkillAddedSA)
            {
                SkillAddedNumber = 0;
                try
                {
                    SkillString = Btc.Convert_Name_Int(element.Name).ToString();
                    SkillString = SkillString.Substring(0, 1);
                    SkillAddedNumber = int.Parse(SkillString);
                }
                catch{}

                if((SkillAddedNumber > 0 && SkillAddedNumber < 4) && element.Tag.ToString() != "1" && element.Name.Substring(0, 10) == "SkillAdded")
                {
                    string Damage = element.Tag.ToString();
                    Damage = Damage.Substring(1);
                    string TypeSkill = element.Tag.ToString().Substring(0, 1);

                    switch (TypeSkill)
                    {
                        case "q":
                            {
                                Turn = int.Parse(Damage.Substring(3));
                                Turn -= 1;
                                autenti = 0;
                                Defense = int.Parse(Damage.Substring(0, 2));
                                break;
                            }
                        case "d":
                            {
                                int vida = 0;
                                if (Defense != 0)
                                {
                                    vida = (Defense - int.Parse(Damage));
                                    if (vida > 0)
                                    {
                                        vida = 0;
                                    }
                                }
                                else
                                {
                                    vida = int.Parse(Damage);
                                }

                                if (SkillAddedNumber == 1)
                                {
                                   
                                    if (element.Name.Substring(12).Substring(0, 3) == "red")
                                    {
                                        vida = int.Parse(Character1_red_life.Content.ToString()) - vida;
                                        Character1_red_life.Content = vida.ToString();
                                        Dead(Character1_red, Character1_red_life);
                                        //Team_Dead();
                                        autenti = 1;
                                    }
                                    else
                                    {
                                        vida = int.Parse(Character1_blue_life.Content.ToString()) - vida;
                                        Character1_blue_life.Content = vida.ToString();
                                        Dead(Character1_blue, Character1_blue_life);
                                        //Team_Dead();
                                        autenti = 1;
                                    }

                                }
                                else if (SkillAddedNumber == 2)
                                {
                                    if (element.Name.Substring(12).Substring(0, 3) == "red")
                                    {
                                        vida = int.Parse(Character2_red_life.Content.ToString()) - vida;
                                        Character2_red_life.Content = vida.ToString();
                                        Dead(Character2_red, Character2_red_life);
                                        //Team_Dead();
                                        autenti = 1;
                                    }
                                    else
                                    {
                                        vida = int.Parse(Character2_blue_life.Content.ToString()) - vida;
                                        Character2_blue_life.Content = vida.ToString();
                                        Dead(Character2_blue, Character2_blue_life);
                                        //Team_Dead();
                                        autenti = 1;
                                    }
                                        
                                }
                                else if (SkillAddedNumber == 3)
                                {
                                    if (element.Name.Substring(12).Substring(0, 3) == "red")
                                    {
                                        vida = int.Parse(Character3_red_life.Content.ToString()) - vida;
                                        Character3_red_life.Content = vida.ToString();
                                        Dead(Character3_red, Character3_red_life);
                                        //Team_Dead();
                                        autenti = 1;
                                    }
                                    else
                                    {
                                        vida = int.Parse(Character3_blue_life.Content.ToString()) - vida;
                                        Character3_blue_life.Content = vida.ToString();
                                        Dead(Character3_blue, Character3_blue_life);
                                        //Team_Dead();
                                        autenti = 1;
                                    }  
                                }

                                break;
                            }
                        case "h":
                            {
                                int vida = 0; 
                                
                                if (SkillAddedNumber == 1)
                                {
                                    vida = int.Parse(Character1_red_life.Content.ToString()) + int.Parse(Damage);
                                    if (vida > 100)
                                        vida = 100;
                                    Character1_red_life.Content = vida.ToString();
                                }
                                else if (SkillAddedNumber == 2)
                                {
                                    vida = int.Parse(Character2_red_life.Content.ToString()) + int.Parse(Damage);
                                    if (vida > 100)
                                        vida = 100;
                                    Character2_red_life.Content = vida.ToString();
                                }
                                else if (SkillAddedNumber == 3)
                                {
                                    vida = int.Parse(Character3_red_life.Content.ToString()) + int.Parse(Damage);
                                    if (vida > 100)
                                        vida = 100;
                                    Character3_red_life.Content = vida.ToString();
                                }
                                break;
                            }
                        default:
                            {
                                break;
                            }

                    }
                }

                if (Turn >= 0 && autenti != 1)
                {
                    element.Tag = element.Tag.ToString().Substring(0, 4) + Turn;
                    autenti = 1;
                }
                else
                {
                    element.Tag = "1";
                    element.Source = new BitmapImage(new Uri("Others/invalid_default.png", UriKind.Relative));
                }

                Team_Dead();
            }
        }

        private void Block_Character(int CharSelect)
        {
            var Images = principal.Children.OfType<Image>();
            string NameSkill = "";
            int Skill = 1;
            NameSkill = "Character" + CharSelect + "_red_skill" + Skill;

            foreach (Image element in Images)
            {
                if (element.Name != null && element.Name != "" && element.Name.Length > 14)
                {
                    try
                    {
                        if (element.Name.Substring(0, 21) == NameSkill)
                        {
                            
                            element.Opacity = 0.5;
                            element.IsEnabled = false;
                            Skill += 1;
                            NameSkill = "Character" + CharSelect + "_red_skill" + Skill;
                        }
                    }
                    catch
                    {}
                }

                if (Skill == 4)
                {
                    break;
                }
            }
        }

        private void Geral_Select(object sender, MouseButtonEventArgs e)
        {
            if (Btc.AttackChar != null)
            {
                //-----------------Damage
                if (Btc.TypeSkill == "d" || Btc.TypeSkill == "da")
                {
                    Image EnemyImage = (Image)sender;

                    if (EnemyImage.Name == "Character1_red" || EnemyImage.Name == "Character2_red" || EnemyImage.Name == "Character3_red")
                    { }
                    else
                    {
                        int EnemyCharNumber = Btc.Convert_Name_Int(EnemyImage.Name);
                        var SkillAD = SkillAdded.Children.OfType<Image>();

                        if (Btc.TypeSkill == "d")
                        {
                            string Sa = null;
                            if (EnemyCharNumber == 1)
                                Sa = "SkillAdded1_blue";
                            if (EnemyCharNumber == 2)
                                Sa = "SkillAdded2_blue";
                            if (EnemyCharNumber == 3)
                                Sa = "SkillAdded3_blue";

                            foreach (Image element in SkillAD)
                            {
                                if (element.Tag.ToString() == "1" && element.Name.Substring(0, 16) == Sa)
                                {
                                    element.Source = new BitmapImage(new Uri("Characters/" + Btc.AttackChar + "/" + Btc.AttackChar + "_skill" + Btc.SkillSelect + "_default.png", UriKind.Relative));
                        
                                    //Tag is damage
                                    element.Tag = Btc.TypeSkill + Btc.Damage_Skill(Btc.SkillSelect, Btc.AttackChar);

                                    Block_Character(Btc.NumAttackChar);
                                    break;
                                }
                            }
                        }
                        else
                        //-----------------Damage Area
                        if (Btc.TypeSkill == "da")
                        {
                            List<string> SaList = new List<string>
                            {
                                "SkillAdded1_blue",
                                "SkillAdded2_blue",
                                "SkillAdded3_blue"
                            };

                            foreach (string elementList in SaList)
                            {
                                foreach (Image element in SkillAD)
                                {
                                    if (element.Tag.ToString() == "1" && element.Name.Substring(0, 16) == elementList)
                                    {
                                        element.Source = new BitmapImage(new Uri("Characters/" + Btc.AttackChar + "/" + Btc.AttackChar + "_skill" + Btc.SkillSelect + "_default.png", UriKind.Relative));

                                        //Tag is damage
                                        element.Tag = "d" + Btc.Damage_Skill(Btc.SkillSelect, Btc.AttackChar);

                                        Block_Character(Btc.NumAttackChar);
                                        break;
                                    }
                                }
                            }
                        }
                        Btc.AttackChar = null;
                        Change_Cursor("Normal");
                    }
                    
                }
                //-----------------Heal
                if (Btc.TypeSkill == "h" || Btc.TypeSkill == "ha" || Btc.TypeSkill == "y" || Btc.TypeSkill == "q" || Btc.TypeSkill == "r")
                {
                    Image FriendImage = (Image)sender;

                    if (FriendImage.Name == "Character1_blue" || FriendImage.Name == "Character2_blue" || FriendImage.Name == "Character3_blue")
                    {}
                    else
                    {
                        int EnemyCharNumber = Btc.Convert_Name_Int(FriendImage.Name);
                        var SkillAD = SkillAdded.Children.OfType<Image>();

                        if (Btc.TypeSkill == "h")
                        {
                            string Sa = null;
                            if (EnemyCharNumber == 1)
                                Sa = "SkillAdded1_red";
                            if (EnemyCharNumber == 2)
                                Sa = "SkillAdded2_red";
                            if (EnemyCharNumber == 3)
                                Sa = "SkillAdded3_red";

                            foreach (Image element in SkillAD)
                            {
                                if (element.Tag.ToString() == "1" && element.Name.Substring(0, 15) == Sa)
                                {
                                    element.Source = new BitmapImage(new Uri("Characters/" + Btc.AttackChar + "/" + Btc.AttackChar + "_skill" + Btc.SkillSelect + "_default.png", UriKind.Relative));

                                    //Tag is damage
                                    element.Tag = Btc.TypeSkill + Btc.Heal_Skill(Btc.SkillSelect, Btc.AttackChar);

                                    Block_Character(Btc.NumAttackChar);
                                    break;
                                }
                            }
                        }
                        else
                        //-----------------Heal Area
                        if (Btc.TypeSkill == "ha")
                        {
                            List<string> SaList = new List<string>
                            {
                                "SkillAdded1_red",
                                "SkillAdded2_red",
                                "SkillAdded3_red"
                            };

                            foreach (string elementList in SaList)
                            {
                                foreach (Image element in SkillAD)
                                {
                                    if (element.Tag.ToString() == "1" && element.Name.Substring(0, 15) == elementList)
                                    {
                                        element.Source = new BitmapImage(new Uri("Characters/" + Btc.AttackChar + "/" + Btc.AttackChar + "_skill" + Btc.SkillSelect + "_default.png", UriKind.Relative));

                                        //Tag is damage
                                        element.Tag = "h" + Btc.Heal_Skill(Btc.SkillSelect, Btc.AttackChar);

                                        Block_Character(Btc.NumAttackChar);
                                        break;
                                    }
                                }
                            }
                        }
                        else
                        //-----------------Heal Self
                        if (Btc.TypeSkill == "y")
                        {
                            if (FriendImage.Name == "Character" + Btc.NumAttackChar.ToString() + "_red")
                            {
                                string Sa = null;
                                if (EnemyCharNumber == 1)
                                    Sa = "SkillAdded1_red";
                                if (EnemyCharNumber == 2)
                                    Sa = "SkillAdded2_red";
                                if (EnemyCharNumber == 3)
                                    Sa = "SkillAdded3_red";

                                foreach (Image element in SkillAD)
                                {
                                    if (element.Tag.ToString() == "1" && element.Name.Substring(0, 15) == Sa)
                                    {
                                        element.Source = new BitmapImage(new Uri("Characters/" + Btc.AttackChar + "/" + Btc.AttackChar + "_skill" + Btc.SkillSelect + "_default.png", UriKind.Relative));

                                        //Tag is damage
                                        element.Tag = "h" + Btc.Heal_Skill(Btc.SkillSelect, Btc.AttackChar);

                                        Block_Character(Btc.NumAttackChar);

                                        //Self need prop
                                        Btc.AttackChar = null;
                                        Change_Cursor("Normal");
                                        break;
                                    }
                                }
                            }
                            else {}
                        }
                        else
                        //-----------------Defense
                        if (Btc.TypeSkill == "q")
                        {
                            string Sa = null;
                            if (EnemyCharNumber == 1)
                                Sa = "SkillAdded1_red";
                            if (EnemyCharNumber == 2)
                                Sa = "SkillAdded2_red";
                            if (EnemyCharNumber == 3)
                                Sa = "SkillAdded3_red";

                            foreach (Image element in SkillAD)
                            {
                                if (element.Tag.ToString() == "1" && element.Name.Substring(0, 15) == Sa)
                                {
                                    element.Source = new BitmapImage(new Uri("Characters/" + Btc.AttackChar + "/" + Btc.AttackChar + "_skill" + Btc.SkillSelect + "_default.png", UriKind.Relative));

                                    //Tag is damage
                                    element.Tag = Btc.TypeSkill + Btc.Defense_Skill(Btc.SkillSelect, Btc.AttackChar);

                                    Block_Character(Btc.NumAttackChar);
                                    break;
                                }
                            }
                        }
                        else
                        if (Btc.TypeSkill == "r")
                        {
                            if (FriendImage.Name == "Character" + Btc.NumAttackChar.ToString() + "_red")
                            {
                                string Sa = null;
                                if (EnemyCharNumber == 1)
                                    Sa = "SkillAdded1_red";
                                if (EnemyCharNumber == 2)
                                    Sa = "SkillAdded2_red";
                                if (EnemyCharNumber == 3)
                                    Sa = "SkillAdded3_red";

                                foreach (Image element in SkillAD)
                                {
                                    if (element.Tag.ToString() == "1" && element.Name.Substring(0, 15) == Sa)
                                    {
                                        element.Source = new BitmapImage(new Uri("Characters/" + Btc.AttackChar + "/" + Btc.AttackChar + "_skill" + Btc.SkillSelect + "_default.png", UriKind.Relative));

                                        //Tag is damage
                                        element.Tag = "q" + Btc.Defense_Skill(Btc.SkillSelect, Btc.AttackChar);

                                        Block_Character(Btc.NumAttackChar);

                                        //Self need prop
                                        Btc.AttackChar = null;
                                        Change_Cursor("Normal");
                                        break;
                                    }
                                }
                            }
                            else { }
                        }
                    }

                    if (Btc.TypeSkill != "y" && Btc.TypeSkill != "r")
                    {
                        Btc.AttackChar = null;
                        Change_Cursor("Normal");
                    }
                    
                }
            }
            //Finish
        }

        private bool Team_Dead()
        {
            int Dead = Btc.Confirmation_TeamDead(Character1_blue_life.Content, Character2_blue_life.Content, Character3_blue_life.Content,
                                                 Character1_red_life.Content, Character2_red_life.Content, Character3_red_life.Content);

            if (Dead == 1 || Dead == 2)
            {
                Close();
                return true;
            }
            return false;
        }

        private void Dead(object Char, object Life)
        {  
            Image Image = (Image)Char;
            Label Label = (Label)Life;
            bool Confimation = Btc.Dead_Confirmation(Label.Content);

            if (Confimation == true)
            {
                if (Label.Name.Substring(11) == "red_life")
                {
                    if (Btc.Convert_Name_Int(Label.Name) == 1)
                    {
                        Character1_red_skill1.Source = new BitmapImage(new Uri("Others/dead_default.png", UriKind.Relative));
                        Character1_red_skill2.Source = new BitmapImage(new Uri("Others/dead_default.png", UriKind.Relative));
                        Character1_red_skill3.Source = new BitmapImage(new Uri("Others/dead_default.png", UriKind.Relative));
                        Character1_red_skill1.IsEnabled = false;
                        Character1_red_skill2.IsEnabled = false;
                        Character1_red_skill3.IsEnabled = false;
                        Character1_red_skill1.Tag = "dead";
                        Character1_red_skill2.Tag = "dead";
                        Character1_red_skill3.Tag = "dead";
                    }
                    else if (Btc.Convert_Name_Int(Label.Name) == 2)
                    {
                        Character2_red_skill1.Source = new BitmapImage(new Uri("Others/dead_default.png", UriKind.Relative));
                        Character2_red_skill2.Source = new BitmapImage(new Uri("Others/dead_default.png", UriKind.Relative));
                        Character2_red_skill3.Source = new BitmapImage(new Uri("Others/dead_default.png", UriKind.Relative));
                        Character2_red_skill1.IsEnabled = false;
                        Character2_red_skill2.IsEnabled = false;
                        Character2_red_skill3.IsEnabled = false;
                        Character2_red_skill1.Tag = "dead";
                        Character2_red_skill2.Tag = "dead";
                        Character2_red_skill3.Tag = "dead";
                    }
                    else if (Btc.Convert_Name_Int(Label.Name) == 3)
                    {
                        Character3_red_skill1.Source = new BitmapImage(new Uri("Others/dead_default.png", UriKind.Relative));
                        Character3_red_skill2.Source = new BitmapImage(new Uri("Others/dead_default.png", UriKind.Relative));
                        Character3_red_skill3.Source = new BitmapImage(new Uri("Others/dead_default.png", UriKind.Relative));
                        Character3_red_skill1.IsEnabled = false;
                        Character3_red_skill2.IsEnabled = false;
                        Character3_red_skill3.IsEnabled = false;
                        Character3_red_skill1.Tag = "dead";
                        Character3_red_skill2.Tag = "dead";
                        Character3_red_skill3.Tag = "dead";
                    }
                }

                Image.Source = new BitmapImage(new Uri("Others/dead_default.png", UriKind.Relative));
                Image.Tag = "Others/dead_default.png";
                Image.IsEnabled = false;
                Label.Content = "0";
            }
        }

        //===================== Others ====================

        private void Change_Life_GlobalColor()
        {
            Change_Life_Color(Character1_red_life);
            Change_Life_Color(Character2_red_life);
            Change_Life_Color(Character3_red_life);
            Change_Life_Color(Character1_blue_life);
            Change_Life_Color(Character2_blue_life);
            Change_Life_Color(Character3_blue_life);
        }

        private void Change_Life_Color(Label label)
        {
            int LabelContent = int.Parse(label.Content.ToString());

            if (LabelContent > 50 && LabelContent <= 100)
                label.Background = Brushes.Green;
            if (LabelContent > 20 && LabelContent <= 50)
                label.Background = Brushes.Orange;
            if (LabelContent <= 20)
                label.Background = Brushes.Red;
        }

        private void Generic_mouseEnter(object sender, MouseEventArgs e)
        {
            Image image = (Image)sender;

            string Source = image.Tag.ToString();
            Source = Source.Remove(Source.Length - 4);
            
            string Name = image.Name;
            Name = Name.Substring(Name.Length - 3);

            if (Btc.SkillSelect != 0 && Btc.AttackChar != null && image.Tag.ToString() != "Others/dead_default.png")
            {
                if ((Btc.TypeSkill == "r" || Btc.TypeSkill == "y") && Name == "red" && image.Name == "Character" + Btc.NumAttackChar + "_red")
                {
                    image.Source = new BitmapImage(new Uri(Source + "_select.png", UriKind.Relative));
                }
                if ((Btc.TypeSkill == "h" || Btc.TypeSkill == "q" || Btc.TypeSkill == "ha") && Name == "red")
                {
                    image.Source = new BitmapImage(new Uri(Source + "_select.png", UriKind.Relative));
                }
                else if ((Btc.TypeSkill == "d" || Btc.TypeSkill == "da") && Name != "red")
                {
                    image.Source = new BitmapImage(new Uri(Source + "_select.png", UriKind.Relative));
                }
            }

        }

        private void Generic_mouseLeave(object sender, MouseEventArgs e)
        {
            if (Btc.SkillSelect != 0 || Btc.AttackChar != null)
            {
                Image image = (Image)sender;

                string Source = image.Tag.ToString();
                Source = Source.Remove(Source.Length - 4);
                image.Source = new BitmapImage(new Uri(Source + ".png", UriKind.Relative));
            }
        }

        private void ShowSkill_MouseEnter(object sender, MouseEventArgs e)
        {
            Image SkillImage = (Image)sender;
            PanelSkillImage.Source = SkillImage.Source;

            string Skill = Btc.Convert_Name_Int(SkillImage.Name).ToString();
            int SkillNumber = int.Parse((Skill.Last()).ToString());
            int CharNumber = int.Parse(Skill.Remove(Skill.Length - 1));
            string CharName = Btc.Return_CharacterRed(CharNumber);

            List<string> ChakraSkill = new List<string>();
            ChakraSkill = Btc.Chakra_Skill(SkillNumber, CharNumber);

            TaijutsuNumber_Panel.Content = ChakraSkill[0];
            BloodlineNumber_Panel.Content = ChakraSkill[1];
            NinjutsuNumber_Panel.Content = ChakraSkill[2];
            GenjutsuNumber_Panel.Content = ChakraSkill[3];

            DamageNumber_Panel.Content = Btc.Damage_Skill(SkillNumber, CharName);
            HealNumber_Panel.Content = Btc.Heal_Skill(SkillNumber, CharName);

            string Defense = Btc.Defense_Skill(SkillNumber, CharName);
            try
            {
                DefenseNumber_Panel.Content = Defense.Substring(0, 2);
            }
            catch { DefenseNumber_Panel.Content = Defense; }

            string SkillType = Btc.Skill_Type(CharNumber, SkillNumber);
            if (SkillType == "da" || SkillType == "ha")
            {
                AreaNumber_Panel.Content = "Yes";
            }
            else { AreaNumber_Panel.Content = "No"; }

            if (SkillType == "r" || SkillType == "y")
            {
                SelfNumber_Panel.Content = "Yes";
            }
            else { SelfNumber_Panel.Content = "No"; }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if(Btc.Confirmation_TeamDead(Character1_blue_life.Content, Character2_blue_life.Content, Character3_blue_life.Content,
                                                 Character1_red_life.Content, Character2_red_life.Content, Character3_red_life.Content) == 2)
            {
                string Result = null;
                Result = "Winner";
                Tmr.Close();
                Close();
                Btc.Save_Battle_Result(Result);
                MessageBox.Show(Result);
            }
            else
            {
                string Result = null;
                Result = "Loser";
                Tmr.Close();
                Close();
                Btc.Save_Battle_Result(Result);
                MessageBox.Show(Result);
            }
        }

        private void Surrender(object sender, MouseButtonEventArgs e)
        {
            Close();
        }

        //===================== IA ====================

        private void Ia_Play()
        {
            Random rand = new Random();
            int attackChar = rand.Next(1, 4);
            int EnemyCharNumber = Btc.Return_MoreLow_CharacterRed(Character1_red_life.Content.ToString(), Character2_red_life.Content.ToString(), Character3_red_life.Content.ToString());
            
            var SkillAD = SkillAdded.Children.OfType<Image>();
            
            string Sa = null;
            if (EnemyCharNumber == 1)
                Sa = "SkillAdded1_red";
            if (EnemyCharNumber == 2)
                Sa = "SkillAdded2_red";
            if (EnemyCharNumber == 3)
                Sa = "SkillAdded3_red";

            foreach (Image element in SkillAD)
            {
                if (element.Tag.ToString() == "1" && element.Name.Substring(0, 15) == Sa)
                {
                    //int attackSKill = 0;
                    string typeSkill = "";
                    //do
                    //{
                    //    attackSKill = rand.Next(1, 4);
                    //    typeSkill = Btc.Skill_Type(attackChar, attackSKill);
                    //}while (typeSkill != "d");

                    //if (Btc.Receive_Skill_Number(Btc.Return_CharacterBlue(attackChar) + "/" + Btc.Return_CharacterBlue(attackChar) + "_skill" + attackChar, 2) == true)
                    //{
                    //    element.Source = new BitmapImage(new Uri("Characters/" + Btc.Return_CharacterBlue(attackChar) + "/" + Btc.Return_CharacterBlue(attackChar) + "_skill" + attackChar + "_default.png", UriKind.Relative));

                    //    //Tag is damage
                    //    element.Tag = "d" + Btc.Damage_Skill(attackChar, Btc.Return_CharacterBlue(attackChar));

                    //};
                    
                    for(int i = 1; i < 4; i++)
                    {
                        for(int y = 1; y <4; y++)
                        {
                            typeSkill = Btc.Skill_Type(i, y);
                            if (Btc.Receive_Skill_Number((Btc.Return_CharacterBlue(i) + i +"/" + 
                                Btc.Return_CharacterBlue(i) + "_skill" + y), 2) == true && typeSkill == "d")
                            {
                                element.Source = new BitmapImage(new Uri("Characters/" + Btc.Return_CharacterBlue(attackChar) + "/" + Btc.Return_CharacterBlue(attackChar) + "_skill" + attackChar + "_default.png", UriKind.Relative));

                                //Tag is damage
                                element.Tag = "d" + Btc.Damage_Skill(attackChar, Btc.Return_CharacterBlue(attackChar));

                            };
                        }
                    }

                    break;
                }
            }       

            Pass_Turn();
        }

        //===================== Sound ====================

        private void Vol_More(object sender, MouseButtonEventArgs e)
        {
            if (Volume.Content.ToString() != "100")
            {
                MusicPlayer.Volume = MusicPlayer.Volume + 0.1;
                Volume.Content = Btc.Conversion_Object_Toint(Volume.Content) + 10;
            }
        }

        private void Vol_Less(object sender, MouseButtonEventArgs e)
        {
            if (Volume.Content.ToString() != "0")
            {
                MusicPlayer.Volume = MusicPlayer.Volume - 0.1;
                Volume.Content = Btc.Conversion_Object_Toint(Volume.Content) - 10;
            }       
        }
    }
}
