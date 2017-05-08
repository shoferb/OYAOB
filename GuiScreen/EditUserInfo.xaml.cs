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
    /// Interaction logic for EditUserInfo.xaml
    /// </summary>
    public partial class EditUserInfo : Window
    {
        private ClientLogic cl;
        private Window parentScreen;
        private int currId;
        private int Id;
        private string name;
        private string username;
        private string email;
        private string firstPassword;
        private string secPassword;
        private int money;


        public EditUserInfo(Window parent, int id, ClientLogic cli)
        {
            InitializeComponent();
            parentScreen = parent;
            cl = cli;
            currId = id;
            cl.SetUserId(id);
        }

        private void currPasswordTextBox_MouseDown(object sender, MouseButtonEventArgs e)
        {
            currPasswordTextBox.Text = "";
            currPasswordTextBox.Opacity = 100;

        }
        private void currPasswordTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void NewPasswordTextBox_OnPreviewMouseDownPasswordTextBox_MouseDown(object sender, MouseButtonEventArgs e)
        {
            currPasswordTextBox.Text = "";
            currPasswordTextBox.Opacity = 100;
        }
    }
}
