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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Client.GuiScreen;
using Client.Logic;
using TexasHoldem.GuiScreen;

namespace Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            ClientLogic cl = new ClientLogic();

            InitializeComponent();
            WellcomeScreen wellcomeScreen = new WellcomeScreen(cl);
            wellcomeScreen.Show();
            LoginScreen l = new LoginScreen(wellcomeScreen,cl);
            l.Show();
            AvatarEditScreen avatar = new AvatarEditScreen(l,cl);
            avatar.Show();
            EditUserInfo editUserInfo = new EditUserInfo(l,1,cl);
            editUserInfo.Show();
            LogoutScreen lo = new LogoutScreen(l,1);
            lo.Show();
            MainAfterLogin ma = new MainAfterLogin(l,1,cl);
            ma.Show();
            RegisterScreen r = new RegisterScreen(l,cl);
            r.Show();
            SearchScreen aSearchScreen = new SearchScreen(l,cl);
            aSearchScreen.Show();
            UserInfoScreen userInfo = new UserInfoScreen(l,cl);
            userInfo.Show();
            
            //this.Hide();
        }
       
    }
}
