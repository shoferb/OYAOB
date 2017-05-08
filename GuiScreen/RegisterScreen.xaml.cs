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
    /// Interaction logic for RegisterScreen.xaml
    /// </summary>
    public partial class RegisterScreen : Window
    {

        private Window parentScreen;
        public RegisterScreen(Window parent)
        {
            InitializeComponent();
            parentScreen = parent;
        }

        private void IDtextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void NametextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
        
        }

        private void UserNametextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void EmailtextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void MoneytextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void PasswordFirstTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void PasswordSecTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void BackToMainButton_Click(object sender, RoutedEventArgs e)
        {
            parentScreen.Show();
            this.Hide();
        }
    }
}
