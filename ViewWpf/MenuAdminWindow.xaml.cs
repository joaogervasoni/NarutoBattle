using Controllers;
using System;
using System.Collections.Generic;
using System.Data;
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
    /// Interaction logic for MenuAdminWindow.xaml
    /// </summary>
    public partial class MenuAdminWindow : Window
    {
        MainWindow Form = Application.Current.Windows[0] as MainWindow;
        Tools tools = new Tools();
        private string selected = "";
        private string account = "";

        public MenuAdminWindow()
        {
            InitializeComponent();
           
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Form.StartButton.IsEnabled = true;
            Form.RandomButton.IsEnabled = true;
            Form.loginCheck = true;
        }

        private void Grid1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ( selected == "Account")
            {
                try
                {
                    DataRowView row = (DataRowView)Grid1.SelectedItem;

                    account = row.Row["Login"].ToString();
                    login.Text = row.Row["Login"].ToString();
                    pass.Text = row.Row["Password"].ToString();
                    victories.Text = row.Row["Victories"].ToString();
                    loses.Text = row.Row["Loses"].ToString();
                    type.Text = row.Row["Type"].ToString();
                }
                catch
                {}

            }

        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string item = "";
            try
            {
                item = ((ComboBoxItem)ComboBox.SelectedItem).Content.ToString();
            }
            catch { item = "Account"; }

            Table_Refresh(item);

            selected = item;

            try
            {
                if (selected == "Skills")
                {
                    AccountChangeGrid.Visibility = Visibility.Hidden;
                }
                else if (selected == "Character")
                {
                    AccountChangeGrid.Visibility = Visibility.Hidden;
                }
                else if (selected == "Account")
                {
                    AccountChangeGrid.Visibility = Visibility.Visible;
                }
            }
            catch
            {}

        }

        private void Table_Refresh(string item)
        {
            DataTable dataTable = new DataTable();
            dataTable = tools.Grid_DataTable(item);
            Grid1.ItemsSource = dataTable.DefaultView;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            tools.Change_Account(account, login.Text, pass.Text, victories.Text, loses.Text, type.Text);
            Table_Refresh(selected);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            tools.Delete_Account(account);
            Table_Refresh(selected);
        }
    }
}
