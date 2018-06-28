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
    /// Interaction logic for MenuAdminWindow.xaml
    /// </summary>
    public partial class MenuAdminWindow : Window
    {
        MainWindow Form = Application.Current.Windows[0] as MainWindow;

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
    }
}
