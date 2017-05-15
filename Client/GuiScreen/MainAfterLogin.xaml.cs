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
using Client.GuiScreen;
using Client.Logic;

namespace TexasHoldem.GuiScreen
{
    /// <summary>
    /// Interaction logic for MainAfterLogin.xaml
    /// </summary>
    public partial class MainAfterLogin : Window
    {
        private ClientLogic cl;
        private Window parent;
        private LogoutScreen logout;
        private int currUserId;
        public MainAfterLogin(Window Parent,int id, ClientLogic cli)
        {
            InitializeComponent();
            cl = cli;      
            parent = Parent;
            currUserId = id;
           
        }

        private void Logoututton_Click(object sender, RoutedEventArgs e)
        {
           logout = new LogoutScreen(this,currUserId);
           logout.Show();
           this.Hide();
        }

        public void SetCurrId(int newId)
        {
            this.currUserId = newId;
        }

     
        private void EditUserbutton_Click(object sender, RoutedEventArgs e)
        {
            EditUserInfo editUserInfo = new EditUserInfo(this,currUserId,cl);
            editUserInfo.Show();
            this.Hide();
        }

        private void GameSearchMenuutton_Click(object sender, RoutedEventArgs e)
        {
            SearchScreen searchScreen = new SearchScreen(this,cl);
            searchScreen.Show();
            this.Hide();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            CreateNewRoom newRoomWindow = new CreateNewRoom(this, cl);
            newRoomWindow.Show();
        }

        private void UserInfobButton_Click(object sender, RoutedEventArgs e)
        {
            UserInfoScreen userIngoScreen = new UserInfoScreen(this, cl);
            userIngoScreen.Show();
            this.Hide();
        }
    }
}
