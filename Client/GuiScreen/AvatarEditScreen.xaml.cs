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
using TexasHoldemShared.CommMessages.ClientToServer;

namespace TexasHoldem.GuiScreen
{
    /// <summary>
    /// Interaction logic for AvatarEditScreen.xaml
    /// </summary>
    public partial class AvatarEditScreen : Window
    {
        private ClientLogic cl;
        private Window parent;

        private bool avatar1 = false;
        private bool avatar2 = false;
        private bool avatar3 = false;
        private bool avatar4 = false;
        private bool avatar5 = false;
        private bool avatar6 = false;
        private bool avatar7 = false;
        private bool avatar8 = false;
        private bool avatar9 = false;
        private bool avatar10 = false;
        public AvatarEditScreen(Window w, ClientLogic cli)
        {
            InitializeComponent();
            parent = w;
            cl = cli;

        }

        
        private void radioButtonAvatar1_Checked(object sender, RoutedEventArgs e)
        {
            if (radioButtonAvatar1.IsChecked == true)
            {
                avatar1 = true;
            }
        }

        private void radioButtonAvatar2_Checked(object sender, RoutedEventArgs e)
        {
            if (radioButtonAvatar2.IsChecked == true)
            {
                avatar2 = true;
            }
        }

        private void radioButtonAvatar3_Checked(object sender, RoutedEventArgs e)
        {
            if (radioButtonAvatar3.IsChecked == true)
            {
                avatar3 = true;
            }
        }

        private void radioButtonAvatar4_Checked(object sender, RoutedEventArgs e)
        {
            if (radioButtonAvatar4.IsChecked == true)
            {
                avatar4 = true;
            }
        }

        private void radioButtonAvatar5_Checked(object sender, RoutedEventArgs e)
        {
            if (radioButtonAvatar5.IsChecked == true)
            {
                avatar5 = true;
            }
        }

        private void radioButtonAvatar6_Checked(object sender, RoutedEventArgs e)
        {
            if (radioButtonAvatar6.IsChecked == true)
            {
                avatar6 = true;
            }
        }

        private void radioButtonAvatar7_Checked(object sender, RoutedEventArgs e)
        {
            if (radioButtonAvatar7.IsChecked == true)
            {
                avatar7 = true;
            }
        }

        private void radioButtonAvatar8_Checked(object sender, RoutedEventArgs e)
        {
            if (radioButtonAvatar8.IsChecked == true)
            {
                avatar8 = true;
            }
        }

        private void radioButtonAvatar9_Checked(object sender, RoutedEventArgs e)
        {
            if (radioButtonAvatar9.IsChecked == true)
            {
                avatar9 = true;
            }
        }

        private void radioButtonAvatar10_Checked(object sender, RoutedEventArgs e)
        {
            if (radioButtonAvatar10.IsChecked == true)
            {
                avatar10 = true;
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            parent.Show();
            this.Hide();
        }

        private void EditAvatar_Click(object sender, RoutedEventArgs e)
        {
            int select = GetSelect();
            if (select == -1)
            {
                MessageBox.Show("error - please select only 1 avatar");
                return;
            }
            string path;
            switch (select)
            {
                case 1:
                    path = "/GuiScreen/Photos/Avatar/andrew.png";
                    break;
                case 2:
                    path = "/GuiScreen/Photos/Avatar/default_female300x300-af1ea9327d6293733a8874dbd97ce49e.png";
                    break;
                case 3:
                    path = "/GuiScreen/Photos/Avatar/Male-Face-J1-icon.png";
                    break;
                case 4:
                    path = "/GuiScreen/Photos/Avatar/fd04.png";
                    break;
                case 5:
                    path = "/GuiScreen/Photos/Avatar/fh02.png";
                    break;
                case 6:
                    path = "/GuiScreen/Photos/Avatar/mummy.png";
                    break;
                case 7:
                    path = "/GuiScreen/Photos/Avatar/devil.png";
                    break;
                case 8:
                    path = "/GuiScreen/Photos/Avatar/frankenstein.png";
                    break;
                case 9:
                    path = "/GuiScreen/Photos/Avatar/k03.png";
                    break;
                case 10:
                    path = "/GuiScreen/Photos/Avatar/e01-1.png";
                    break;
                default:
                    MessageBox.Show("error - please select 1 avatar");
                    return;
                    break;
            }
            bool isEditAvatrOk = cl.EditDetails(EditCommMessage.EditField.Avatar, path);
            if (isEditAvatrOk)
            {
                MessageBox.Show("User avatar was edit");
                cl.user.avatar = path;
            }
            else
            {
                MessageBox.Show("User avatar Edit - fail");
            }
        }

        private int GetSelect()
        {
            if (avatar1 && !avatar2 && !avatar3 && !avatar4 && !avatar5 && !avatar6 && !avatar7 && !avatar8 &&
                !avatar9 && !avatar10)
            {
                return 1;
            }
            if (avatar2 && !avatar1 && !avatar3 && !avatar4 && !avatar5 && !avatar6 && !avatar7 && !avatar8 &&
                !avatar9 && !avatar10)
            {
                return 2;
            }
            if (avatar3 && !avatar2 && !avatar1 && !avatar4 && !avatar5 && !avatar6 && !avatar7 && !avatar8 &&
                !avatar9 && !avatar10)
            {
                return 3;
            }
            if (avatar4 && !avatar2 && !avatar3 && !avatar1 && !avatar5 && !avatar6 && !avatar7 && !avatar8 &&
                !avatar9 && !avatar10)
            {
                return 4;
            }
            if (avatar5 && !avatar2 && !avatar3 && !avatar4 && !avatar1 && !avatar6 && !avatar7 && !avatar8 &&
                !avatar9 && !avatar10)
            {
                return 5;
            }
            if (avatar6 && !avatar2 && !avatar3 && !avatar4 && !avatar5 && !avatar1 && !avatar7 && !avatar8 &&
                !avatar9 && !avatar10)
            {
                return 6;
            }
            if (avatar7 && !avatar2 && !avatar3 && !avatar4 && !avatar5 && !avatar6 && !avatar1 && !avatar8 &&
                !avatar9 && !avatar10)
            {
                return 7;
            }
            if (avatar8 && !avatar2 && !avatar3 && !avatar4 && !avatar5 && !avatar6 && !avatar7 && !avatar1 &&
                !avatar9 && !avatar10)
            {
                return 8;
            }
            if (avatar9 && !avatar2 && !avatar3 && !avatar4 && !avatar5 && !avatar6 && !avatar7 && !avatar8 &&
                !avatar1 && !avatar10)
            {
                return 9;
            }
            if (avatar10 && !avatar2 && !avatar3 && !avatar4 && !avatar5 && !avatar6 && !avatar7 && !avatar8 &&
                !avatar9 && !avatar1)
            {
                return 10;
            }
            return -1;
        }
    }
}
