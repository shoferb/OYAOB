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
using Client.Logic;

namespace TexasHoldem.GuiScreen
{
    /// <summary>
    /// Interaction logic for LogoutScreen.xaml
    /// </summary>
    public partial class LogoutScreen : Window
    {
        private Window parent;
        private ClientLogic cl;
        private string username;
        private string password;
        private int currUserId;
        public LogoutScreen(Window w)
        {
            InitializeComponent();
            parent = w;
            cl = new ClientLogic();
          
        }

        private void UserNametextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            username = UserNametextBox.Text;
        }

        private void passwordBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            password = passwordBox.Text;
        }

        private void MainMenuBtton_Click(object sender, RoutedEventArgs e)
        {
            parent.Show();
            this.Hide();
        }

        private void Logoutbutton_Click(object sender, RoutedEventArgs e)
        {
            bool logoutOk = cl.Logout(username, password);
            if (logoutOk)
            {
                WellcomeScreen wellcomeScreen = new WellcomeScreen();

                wellcomeScreen.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Invalid input");
            }
        }

        private void passwordBox_MouseDown(object sender, MouseButtonEventArgs e)
        {
            passwordBox.Text = "";
            passwordBox.Opacity = 100;

        }
    }
}
