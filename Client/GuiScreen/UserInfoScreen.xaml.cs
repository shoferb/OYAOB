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
using TexasHoldem.GuiScreen;

namespace Client.GuiScreen
{
    /// <summary>
    /// Interaction logic for UserInfoScreen.xaml
    /// </summary>
    public partial class UserInfoScreen : Window
    {
        private ClientLogic cl;
        private Window parent;

        public UserInfoScreen(Window w,ClientLogic cli)
        {
            InitializeComponent();
            parent = w;
            cl = cli;
            IdtextBox.Text = cl.user.id.ToString();
            NameTextBox.Text = cl.user.name;
            UserNametextBox.Text = cl.user.username;
            EmailtextBox.Text = cl.user.email;
            MoneytextBox.Text = cl.user.money.ToString();
            LeaugeTextBox.Text = cl.user.leauge;
        }

        private void Backbutton_Click(object sender, RoutedEventArgs e)
        {
            parent.Show();
            this.Hide();
        }

        private void EditMenubutton_Click(object sender, RoutedEventArgs e)
        {
            EditUserInfo editUserInfo = new EditUserInfo(this,cl.user.id,cl);
            editUserInfo.Show();
            this.Hide();
        }

      
    }
}
