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

namespace TexasHoldem.GuiScreen
{
    /// <summary>
    /// Interaction logic for WellcomeScreen.xaml
    /// </summary>
    public partial class WellcomeScreen : Window
    {
        private LoginScreen loginScreen;
        private RegisterScreen registerScreen;
        private ClientLogic cl;
        public WellcomeScreen()
        {
            InitializeComponent();
            cl = new ClientLogic();
            //todo create new handlers
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            loginScreen = new LoginScreen(this,cl);
            loginScreen.Show();
            this.Hide(); 
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            registerScreen = new RegisterScreen(this,cl);
            registerScreen.Show();
            this.Hide();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            cl.CloseSystem();
            Application.Current.Shutdown();
        }
    }
}
