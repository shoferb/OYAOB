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
using System.Windows.Media.Media3D;
using System.Windows.Shapes;

namespace TexasHoldem.GuiScreen
{
    /// <summary>
    /// Interaction logic for LoginScreen.xaml
    /// </summary>
    public partial class LoginScreen : Window
    {
        private WellcomeScreen wsScreen;
        private RegisterScreen rgScreen; 
        private string userName;
        private string password;
        private ClientLogic cl;


        public LoginScreen(WellcomeScreen ws)
        {
            InitializeComponent();
            cl = new ClientLogic();
            wsScreen = ws;
        }


        private void UserNametextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            userName = UserNametextBox.Text;
        }
        private void MainMenuBtton_Click(object sender, RoutedEventArgs e)
        {
            wsScreen.Show();
            this.Hide();
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            rgScreen = new RegisterScreen(this);
            rgScreen.Show();
            this.Hide();
        }

        private void Loginbutton_Click(object sender, RoutedEventArgs e)
        {
            int loginOk = cl.login(userName, password);
            if (loginOk != -1)
            {
                MainAfterLogin mainAfterLogin = new MainAfterLogin(this, loginOk);
                mainAfterLogin.Show();
                this.Hide();
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

        private void UserNametextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            userName = UserNametextBox.Text;
        }

        private void passwordBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            password = passwordBox.Text;
        }
    }
}
